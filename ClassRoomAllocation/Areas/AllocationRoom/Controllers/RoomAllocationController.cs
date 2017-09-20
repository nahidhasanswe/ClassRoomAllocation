using BusinessLogicLayer.Room_Allocation;
using MongoDB.Driver;
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
        [Route("Add")]
        public async Task<IHttpActionResult> AddAllocationRoom(RoomAllocation room)
        {
            try
            {
                room.TeachersInitial = User.Identity.Name;
                await _roomAllocation.AddRoomAllocation(room);
                return Ok("Successfully submit request for room allocation");
            }
            catch(MongoWriteException)
            {
                return BadRequest("Sorry! The Classroom is already allocated");
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IHttpActionResult> UpdateAllocation(RoomAllocation room)
        {
            try
            {
                await _roomAllocation.UpdateRoomAllocation(room);
                return Ok("Successfully Updated Room Allocation");
            }
            catch
            {
                return BadRequest("Internal server Problem");
            }
        }

        [HttpGet]
        [Route("Remove/{id}")]
        public async Task<IHttpActionResult> DeleteRoomAllocation(string id)
        {
            await _roomAllocation.RemoveRoomAllocation(id);
            return Ok("Successfully remove");
        }

        [HttpGet]
        [Route("RemoveAll")]
        public async Task<IHttpActionResult> DeleteAll()
        {
            await _roomAllocation.RemoveAll();
            return Ok("Successfully remove all Room Allocation");
        }
    }
}
