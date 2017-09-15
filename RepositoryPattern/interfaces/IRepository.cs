using MongoDB.Bson;
using MongoDB.Driver;
using RepositoryPattern.Model_Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.interfaces
{
    public class IRepository
    {
        public interface IRoutine
        {
            Task<IEnumerable<Routine>> Get();
            Task<Routine> Get(string dayName);
            Task Add(Routine routine);
            Task Update(string id, Routine routine);
            Task<DeleteResult> Remove(string id);
            Task<DeleteResult> RemoveAll();
        }

        public interface IRoomAllocation
        {
            Task<IEnumerable<RoomAllocation>> Get();
            Task<RoomAllocation> Get(string id);
            Task<RoomAllocation> Get(DateTime date,string timeSlot,string roomNo);
            Task Add(RoomAllocation room);
            Task Update(string id, RoomAllocation room);
            Task<DeleteResult> Remove(string id);
            Task<DeleteResult> RemoveAll();
        }

        public interface IRoomCancellation
        {
            Task<IEnumerable<RoomCancellation>> Get();
            Task<RoomCancellation> Get(string id);
            Task<RoomCancellation> Get(DateTime date,string timeSlot, string roomNo);
            Task Add(RoomCancellation room);
            Task Update(string id, RoomCancellation room);
            Task<DeleteResult> Remove(string id);
            Task<DeleteResult> RemoveAll();
        }

        public interface ITeachers
        {
            Task<IEnumerable<Teachers>> Get();
            Task<Teachers> Get(string id);
            Task Add(Teachers teacher);
            Task Update(string id, Teachers teacher);
            Task<DeleteResult> Remove(string id);
            Task<DeleteResult> RemoveAll();
        }

    }
}
