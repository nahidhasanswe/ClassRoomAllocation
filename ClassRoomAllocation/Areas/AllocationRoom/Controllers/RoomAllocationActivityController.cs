using BusinessLogicLayer.Room_Allocation;
using RepositoryPattern.Model_Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClassRoomAllocation.Areas.AllocationRoom.Controllers
{
    [Authorize]
    [RoutePrefix("api/RoomAllocation/Activity")]
    public class RoomAllocationActivityController : ApiController
    {
        private readonly RoomAllocationActivity _activity = new RoomAllocationActivity();

        [Route("CheckAvailability")]
        [HttpPost]
        public async Task<IHttpActionResult> CheckAvailableForRoomAllocation(AvailableRoom room)
        {
            var result = await _activity.CheckAvailability(room);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("The room is already allocated by Other");
            }
        }

        [HttpGet]
        [Route("GetByIndividual")]
        public async Task<IEnumerable<RoomAllocation>> GetRoomAllocationList()
        {
            return await _activity.GetIndividualRoomAllocationList(User.Identity.Name);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IEnumerable<RoomAllocation>> GetAllRoomAllocationList()
        {
            return await _activity.GetAllRoomAllocationList();
        }
    }
}
