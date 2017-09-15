using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using RepositoryPattern.Model_Class;
using static RepositoryPattern.interfaces.IRepository;
using MongoDB.Bson;

namespace RepositoryPattern.Repository
{
    public class RoomAllocationRepository : IRoomAllocation
    {
        private readonly DbContext _context;
        public RoomAllocationRepository()
        {
            _context = new DbContext();
        }

        public async Task Add(RoomAllocation room)
        {
            await _context.RoomAllocation.InsertOneAsync(room);
        }

        public async Task<IEnumerable<RoomAllocation>> Get()
        {
            return await _context.RoomAllocation.Find(x=>true).ToListAsync();
        }

        public async Task<RoomAllocation> Get(string id)
        {
            return await _context.RoomAllocation.Find(Builders<RoomAllocation>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
        }

        public async Task<RoomAllocation> Get(DateTime date, string timeSlot, string roomNo)
        {
            var builder = Builders<RoomAllocation>.Filter;
            var filt = builder.Eq("TimeSlot", timeSlot) & builder.Eq("RoomNo", roomNo) & builder.Gt("Date", date);
            return await _context.RoomAllocation.Find(filt).FirstOrDefaultAsync();
        }

        public async Task<DeleteResult> Remove(string id)
        {
            return  await _context.RoomAllocation.DeleteOneAsync(Builders<RoomAllocation>.Filter.Eq("Id", id));
        }

        public async Task<DeleteResult> RemoveAll()
        {
            return await _context.RoomAllocation.DeleteManyAsync(new BsonDocument());
        }

        public async Task Update(string id, RoomAllocation room)
        {
            await _context.RoomAllocation.ReplaceOneAsync(m => m.Id == id, room);
        }
    }
}
