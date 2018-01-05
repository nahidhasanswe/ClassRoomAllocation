using BusinessLogicLayer.Admin;
using BusinessLogicLayer.Mail_Service;
using BusinessLogicLayer.Some_Logic;
using MongoDB.Driver;
using RepositoryPattern.Model_Class;
using RepositoryPattern.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Room_Allocation
{
    public class RoomAllocationOperation
    {
        private readonly RoomAllocationRepository _roomAllocation;

        public RoomAllocationOperation()
        {
            _roomAllocation = new RoomAllocationRepository();
        }

        public async Task AddRoomAllocation(RoomAllocation room)
        {
            AdminActivity _admin = new AdminActivity();
            var adminList = await _admin.GetAdminEmailList();

            var subject = "Request for Room Allocation";

            var teacherName = await _admin.GetTeacherByUserNameAsync(room.TeachersInitial);

            var messageBody = "Dear Sir<br>I need a extra class for course " + room.CourseCode + ". I choose " + room.RoomNo + " room for the extra class. Please approve my request.<br>Thanks By<br>" + teacherName.TeacherFullName;

            

            // room.Date = ;
            room.Submited_Date = DateTime.Now;
            room.Date = Date.GetLocalZoneDate(room.Date);
            await _roomAllocation.Add(room);

            foreach (var email in adminList)
            {
                await Email.SendMailAsync(subject, messageBody, email.Email);
            }
        }

        public async Task UpdateRoomAllocation(RoomAllocation room)
        {
            await _roomAllocation.Update(room.Id,room);
        }

        public async Task<DeleteResult> RemoveRoomAllocation(string id)
        {
            return await _roomAllocation.Remove(id);
        }

        public async Task<DeleteResult> RemoveAll()
        {
            return await _roomAllocation.RemoveAll();
        }
    }
}
