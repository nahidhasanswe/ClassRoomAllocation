using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using ClassRoomAllocation.Models;
using ClassRoomAllocation.Providers;
using ClassRoomAllocation.Results;
using AspNet.Identity.MongoDB;
using BusinessLogicLayer.Admin;
using BusinessLogicLayer.Teacher;
using RepositoryPattern.Model_Class;
using System.Net;
using BusinessLogicLayer.Mail_Service;

namespace ClassRoomAllocation.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }


        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Provide all required information");
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest("Your old password is not correct");
            }

            return Ok("Password is change Successfully");
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await UserManager.FindByNameAsync(model.userName);

            IdentityResult result = await UserManager.AddPasswordAsync(user.Id, "123456");

            if (!result.Succeeded)
            {
                return BadRequest("Internal Server Problem or user is not exist");
            }

            return Ok("Password set successfully and password is 123456");
        }

        [Route("ForgotPassword")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.teacherInitial);
                if (user == null)
                {
                    return BadRequest("Sorry! Teacher Initial is not exist");
                }
                if (user.Email == null)
                {
                    return BadRequest("Sorry! You have no mail attach. Please contact admin to recover Password");
                }
                try
                {
                    var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    code = WebUtility.UrlEncode(code);
                    var Domain = System.Configuration.ConfigurationManager.AppSettings.Get("Domain");
                    var url = Domain + "ResetPassword?userId=" + user.Id + "&resetCode=" + code;

                    await Email.SendMailAsync("Reset Password", "Dear Sir,<br>Please reset your password by clicking <a href=\"" + new Uri(url) + "\">here</a>", user.Email);
                    return Ok("Please check your Email and recovery Password");
                }
                catch (Exception e)
                {
                    return BadRequest(e.ToString());
                }
            }

            return BadRequest("Employee Id is required");
        }

        [Route("ResetPassword")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.id);

                if (user == null)
                {
                    return BadRequest("User is not exists");
                }


                var result = await UserManager.ResetPasswordAsync(user.Id, model.code, model.NewPassword);
                if (result.Succeeded)
                {
                    return Ok("Password Reset Successfully");

                }
                else
                {
                   return BadRequest("Code or id is incorrect plz resend again");
                }
            }
            return BadRequest("Internal Server Problem");
        }


        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TeachersOperation _teacher = new TeachersOperation();

            var user = new ApplicationUser() { UserName = model.TeachersInitial, Email = model.Email};

            var password = "123456";

            IdentityResult result = await UserManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return BadRequest(model.TeachersInitial+" is already taken");
            }
            else
            {
                try
                {
                    await UserManager.AddToRoleAsync(user.Id,model.Role);
                    await _teacher.AddTeacher(new Teachers {TeacherInitial=model.TeachersInitial,TeacherFullName=model.TeacherFullName });
                    return Ok("Successfull add and default password is : 123456");
                }
                catch
                {
                    return BadRequest("Internal Server Problem");
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("UpdateTeacher")]
        public async Task<IHttpActionResult> UpdateTeacher(UpdateTeacherModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TeachersOperation _teacher = new TeachersOperation();

            try
            {
                var user = await UserManager.FindByNameAsync(model.OldInitial);
                user.Roles.Clear();
                user.UserName = model.TeachersInitial;
                user.Email = model.Email;
                user.Roles.Add(model.Role);

                await UserManager.UpdateAsync(user);

                await _teacher.UpdateTeacher(new Teachers { Id = model.Id, TeacherFullName = model.TeacherFullName, TeacherInitial = model.TeachersInitial });


                return Ok("Successfully updated Teacher Information");
            }
            catch 
            {
                return BadRequest("Internal Serve Problem");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("RemoveTeacher")]
        public async Task<IHttpActionResult> RemoveTeacher(RemoveModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TeachersOperation _teacher = new TeachersOperation();

            try
            {
                var user = await UserManager.FindByNameAsync(model.Initial);
                await UserManager.DeleteAsync(user);

                await _teacher.RemoveTeacher(model.Id);


                return Ok("Successfully remove Teacher Information");
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
