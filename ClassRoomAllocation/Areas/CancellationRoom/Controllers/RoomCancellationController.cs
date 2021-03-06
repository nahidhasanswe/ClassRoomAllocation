﻿using BusinessLogicLayer.Room_Allocation;
using BusinessLogicLayer.Room_Cancellation;
using MongoDB.Driver;
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
        [Route("Add")]
        public async Task<IHttpActionResult> AddRoomCancellation(RoomCancellation room)
        {
            try
            {
                room.TeachersInitial = User.Identity.Name;
                await _roomCancellation.AddRoomCancellation(room);
                return Ok("Successfully Cancel the Class from the room");
            }
            catch (MongoWriteException)
            {
                return BadRequest("Sorry! The Classroom is already canceled");
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        }

        [HttpPost]
        [Route("Update/{id}")]
        public async Task<IHttpActionResult> UpdateRoomCancellation(RoomCancellation room)
        {
            try
            {
                await _roomCancellation.UpdateRoomCancellation(room);
                return Ok("Successfully Update the Class from the room");
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        }

        [HttpPost]
        [Route("Remove")]
        public async Task<IHttpActionResult> RemoveRoomCancellation(RoomCancellation room)
        {
            RoomAllocationActivity _activity = new RoomAllocationActivity();

            try
            {

                var _result = await _activity.CheckAvailability(new AvailableRoom { date =room.Date, roomNo =room.RoomNo, timeSlot =room.TimeSlot });
                if (_result)
                {
                    await _roomCancellation.RemoveRoomCancellation(room.Id);
                    return Ok("Successfully remove room cancellation");
                }
                else
                {
                    return BadRequest("The room is already allocated by someone");
                }
                
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        }

        [HttpGet]
        [Route("RemoveAll/{id}")]
        public async Task<IHttpActionResult> RemoveAll()
        {
            try
            {
                await _roomCancellation.RemoveAll();
                return Ok();
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        }
    }
}
