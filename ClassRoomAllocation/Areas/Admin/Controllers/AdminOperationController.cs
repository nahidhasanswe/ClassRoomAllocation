using BusinessLogicLayer.Admin;
using BusinessLogicLayer.Room_Allocation;
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
    [RoutePrefix("api/Admin/Operation")]
    public class AdminOperationController : ApiController
    {
        private readonly AdminOperation _operation = new AdminOperation();

        [HttpPost]
        [Route("AddRoutine")]
        public async Task<IHttpActionResult> AddRoutine (ICollection<Routine> routineList)
        {
            var result = await _operation.AddRoutine(routineList);

            if (result)
                return Ok("Successfully added Routine");
            else
                return BadRequest("Internal Server Problem");
        }

        [HttpPost]
        [Route("AcceptAllocation")]
        public async Task<IHttpActionResult> AcceptAllocation(RoomAllocation room)
        {
            RoomAllocationOperation _roomAllocation = new RoomAllocationOperation();

            room.isAccept = true;

            try
            {
                await _roomAllocation.UpdateRoomAllocation(room);
                return Ok("Successfully accept the room Allocation");
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        }

        [HttpGet]
        [Route("RejectAllocation/{id}")]
        public async Task<IHttpActionResult> RejectAllocation(string id)
        {
            RoomAllocationOperation _roomAllocation = new RoomAllocationOperation();

            try
            {
                await _roomAllocation.RemoveRoomAllocation(id);
                return Ok("Successfully reject the room Allocation");
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        }



    }

}
