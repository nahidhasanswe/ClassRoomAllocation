using BusinessLogicLayer.Routines;
using RepositoryPattern.Model_Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClassRoomAllocation.Areas.Routines.Controllers
{
    [RoutePrefix("api/Routine")]
    public class RoutineDataController : ApiController
    {
        private readonly RoutineOperation _routine=new RoutineOperation();

        [HttpPost]
        [Route("AddRoutine")]
        public async Task<IHttpActionResult> AddRoutine(Routine routine)
        {
            await _routine.AddRoutine(routine);
            return Ok("Successfully Added Routine");
        }

        [HttpPost]
        [Route("UpdateRoutine")]
        public async Task<IHttpActionResult> UpdateRoutine(Routine routine)
        {
            try
            {
                await _routine.UpdateRoutine(routine);
                return Ok();
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        }

        [HttpGet]
        [Route("Remove/{id}")]
        public async Task<IHttpActionResult> RemoveRoutine(string id)
        {
            try
            {
               return Ok(await _routine.DeleteRoutine(id));
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        }

        [HttpGet]
        [Route("RemoveAll")]
        public async Task<IHttpActionResult> RemoveAll()
        {
            try
            {
                return Ok(await _routine.DeleteAll());
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        }
    }
}
