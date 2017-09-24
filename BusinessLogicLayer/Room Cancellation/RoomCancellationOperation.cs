using BusinessLogicLayer.Some_Logic;
using MongoDB.Driver;
using RepositoryPattern.Model_Class;
using RepositoryPattern.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Room_Cancellation
{
    public class RoomCancellationOperation
    {
        private readonly RoomCancellationRepository _roomCancellation;

        public RoomCancellationOperation()
        {
            _roomCancellation = new RoomCancellationRepository();
        }

        public async Task AddRoomCancellation(RoomCancellation room)
        {
            room.Date = Date.GetLocalZoneDate(room.Date);
            await _roomCancellation.Add(room);
        }

        public async Task<RoomCancellation> GetRoomCancellationInfo(string id)
        {
           return await _roomCancellation.Get(id);
        }

        public async Task UpdateRoomCancellation(RoomCancellation room)
        {
            await _roomCancellation.Update(room.Id, room);
        }

        public async Task<DeleteResult> RemoveRoomCancellation(string id)
        {
            return await _roomCancellation.Remove(id);
        }

        public async Task<DeleteResult> RemoveAll()
        {
            return await _roomCancellation.RemoveAll();
        }
    }
}
