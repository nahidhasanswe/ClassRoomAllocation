using BusinessLogicLayer.Routines;
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
        public async Task<Routine> GetEmptyClassRoom(AllocateDate date)
        {
            date.date = DateTime.UtcNow.ToLocalTime().AddDays(1);
            return await _routine.GetEmptyClassRoom(date.date.Date);
        }

        [HttpGet]
        [Route("GetRoutineByDate/{dayName}")]
        public async Task<Routine> GetRoutineByDay(string dayName)
        {
            return await _routine.GetRoutineByDay(dayName);
        }
    }

    public class AllocateDate
    {
        public DateTime date { get; set; }
    }
}
