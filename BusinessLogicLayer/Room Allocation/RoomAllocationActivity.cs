using BusinessLogicLayer.Some_Logic;
using MongoDB.Bson;
using RepositoryPattern.Model_Class;
using RepositoryPattern.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Room_Allocation
{
    public class RoomAllocationActivity
    {
        RoomAllocationRepository _roomAllocation;

        public RoomAllocationActivity()
        {
            _roomAllocation = new RoomAllocationRepository();
        }
        
        public async Task<Boolean> CheckAvailability(AvailableRoom room)
        {

            var roomAvailable = await _roomAllocation.Get(Date.GetLocalZoneDate(room.date),room.timeSlot,room.roomNo);

            if (roomAvailable==null)
                return true;
            else
                return false;
        }

        public async Task<IEnumerable<RoomAllocation>> GetIndividualRoomAllocationList(string TeachersInitial)
        {
            var ListOfRoomAllocation = await _roomAllocation.Get();

            var query = from roomList in ListOfRoomAllocation.AsQueryable().Where(r => r.TeachersInitial == TeachersInitial && r.Date>=DateTime.Today)
                        select roomList;
            return query.AsEnumerable();
        }

        public async Task<IEnumerable<RoomAllocation>> GetAllRoomAllocationList()
        {
            return await _roomAllocation.Get();
        }



    }
}
