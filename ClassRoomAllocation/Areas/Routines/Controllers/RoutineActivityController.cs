using BusinessLogicLayer.Routines;
using BusinessLogicLayer.Some_Logic;
using MongoDB.Bson;
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
    [RoutePrefix("api/Routine/Activity")]
    public class RoutineActivityController : ApiController
    {
        private readonly RoutineActivity _routine = new RoutineActivity();

        [HttpGet]
        [Route("GetFullRoutine")]
        public async Task<IEnumerable<object>> GetRoutine()
        {
            return await _routine.GetRoutine();
        }

        [HttpPost]
        [Route("GetEmptyClassroom")]
        public async Task<IHttpActionResult> GetEmptyClassRoom(AllocateDate date)
        {
            date.date = Date.GetLocalZoneDate(date.date);
            var routine= await _routine.GetEmptyClassRoom(date.date);
            if (routine == null)
                return BadRequest("No routine found for this date");
            else
                return Ok(routine);
        }

        [HttpGet]
        [Route("GetRoutineByDate/{dayName}")]
        public async Task<Routine> GetRoutineByDay(string dayName)
        {
            return await _routine.GetRoutineByDay(dayName);
        }

        [HttpGet]
        [Route("GetRoutineByTeacher")]
        public async Task<IEnumerable<object>> GetRoutineByTeacher()
        {
            return await _routine.GetIndividualRoutine(User.Identity.Name);
        }
    }

    public class AllocateDate
    {
        public DateTime date { get; set; }
    }
}
