using AspNet.Identity.MongoDB;
using BusinessLogicLayer.Admin;
using ClassRoomAllocation.App_Start;
using ClassRoomAllocation.Models;
using Microsoft.AspNet.Identity;
using RepositoryPattern.Model_Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClassRoomAllocation.Areas.Admin.Controllers
{
    [RoutePrefix("api/Admin/Activity")]
    public class AdminActivityController : ApiController
    {
        private readonly AdminActivity _activity = new AdminActivity();

        [HttpGet]
        [Route("GetRoutine")]
        public async Task<IEnumerable<Routine>> GetRoutine()
        {
            return await _activity.GetRoutine();
        }

        [HttpGet]
        [Route("GetTeacher")]
        public async Task<IEnumerable<object>> GetTeacher()
        {
            return await _activity.GetTeachersList();
        }

        [HttpGet]
        [Route("GetPendingList")]
        public async Task<IEnumerable<object>> GetPendingList ()
        {
            return await _activity.GetPendingRoomAllocation();
        }

        
    }
}
