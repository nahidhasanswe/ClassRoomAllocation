using BusinessLogicLayer.Teacher;
using RepositoryPattern.Model_Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClassRoomAllocation.Areas.Teacher.Controllers
{
    [RoutePrefix("api/Teacher/Operation")]
    public class TeacherOperationController : ApiController
    {
        private readonly TeachersOperation _teacher = new TeachersOperation();

        [HttpPost]
        [Route("Add")]
        public async Task<IHttpActionResult> Add(Teachers teacher)
        {
            try
            {
                await _teacher.AddTeacher(teacher);
                return Ok("Successfully added");
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        } 
    }
}
