using BusinessLogicLayer.Room_Cancellation;
using RepositoryPattern.Model_Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClassRoomAllocation.Areas.CancellationRoom.Controllers
{
    [RoutePrefix("api/RoomCancellation")]
    public class RoomCancellationController : ApiController
    {
        private readonly RoomCancellationOperation _roomCancellation = new RoomCancellationOperation();

        [HttpPost]
        [Route("AddRoomCancellation")]
        public async Task<IHttpActionResult> AddRoomCancellation(RoomCancellation room)
        {
            try
            {
                await _roomCancellation.AddRoomCancellation(room);
                return Ok("Successfully Cancel the Class from the room");
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        }
    }
}
