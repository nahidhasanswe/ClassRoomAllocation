﻿using BusinessLogicLayer.Room_Cancellation;
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
    [RoutePrefix("api/RoomCancellation/Activity")]
    public class RoomCancellationActivityController : ApiController
    {
       private readonly RoomCancellationActivity _activity = new RoomCancellationActivity();

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
                return BadRequest("The room is already canceled by you");
            }
        }

        [HttpGet]
        [Route("GetByIndividual")]
        public async Task<IEnumerable<RoomCancellation>> GetIndividualRoomCancellationList()
        {
            return await _activity.GetIndividualRoomCancellationList(User.Identity.Name);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IEnumerable<RoomCancellation>> GetAllRoomCancellationList()
        {
            return await _activity.GetAllRoomCancellationList();
        }
    }
}
