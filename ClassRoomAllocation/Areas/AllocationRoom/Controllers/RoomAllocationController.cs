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
    [RoutePrefix("api/RoomAllocation")]
    public class RoomAllocationController : ApiController
    {
        private readonly RoomAllocationOperation _roomAllocation = new RoomAllocationOperation();

        [HttpPost]
        [Route("AddRoomAllocation")]
        public async Task<IHttpActionResult> AddAllocationRoom(RoomAllocation room)
        {
            try
            {
                await _roomAllocation.AddRoomAllocation(room);
                return Ok("Successfully submit request for room allocation");
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        }
    }
}
