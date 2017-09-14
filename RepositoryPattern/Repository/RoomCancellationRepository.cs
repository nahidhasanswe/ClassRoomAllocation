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
    public class RoomCancellationRepository : IRoomCancellation
    {
        private readonly DbContext _context;

        public RoomCancellationRepository()
        {
            _context = new DbContext();
        }


        public async Task Add(RoomCancellation room)
        {
            await _context.RoomCancellation.InsertOneAsync(room);
        }

        public async Task<IEnumerable<RoomCancellation>> Get()
        {
            return await _context.RoomCancellation.Find(x=>true).ToListAsync();
        }

        public async Task<RoomCancellation> Get(string id)
        {
            return await _context.RoomCancellation.Find(Builders<RoomCancellation>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
        }

        public async Task<DeleteResult> Remove(string id)
        {
            return await _context.RoomCancellation.DeleteOneAsync(Builders<RoomCancellation>.Filter.Eq("Id", id));
        }

        public async Task<DeleteResult> RemoveAll()
        {
           return await _context.RoomCancellation.DeleteManyAsync(new BsonDocument());
        }

        public async Task Update(string id, RoomCancellation room)
        {
            await _context.RoomCancellation.ReplaceOneAsync(Builders<RoomCancellation>.Filter.Eq("Id", id),room);
        }
    }
}
