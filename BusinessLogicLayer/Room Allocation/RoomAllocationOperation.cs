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
            room.Date = DateTime.UtcNow.ToLocalTime().Date.AddDays(2);
            await _roomAllocation.Add(room);
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
