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
    public class RoutineRepository : IRoutine
    {
        private readonly DbContext _context;

        public RoutineRepository()
        {
            _context = new DbContext();
        }


        public async Task Add(Routine routine)
        {
            await _context.Routine.InsertOneAsync(routine);
        }

        public async Task<IEnumerable<Routine>> Get()
        {
           return await _context.Routine.Find(x=>true).ToListAsync();
        }

        public async Task<Routine> Get(string id)
        {
            return await _context.Routine.Find(Builders<Routine>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
        }

        public async Task<DeleteResult> Remove(string id)
        {
            return await _context.Routine.DeleteOneAsync(Builders<Routine>.Filter.Eq("Id", id));
        }

        public async Task<DeleteResult> RemoveAll()
        {
            return await _context.Routine.DeleteManyAsync(new BsonDocument());
        }

        public async Task Update(string id, Routine routine)
        {
            await _context.Routine.ReplaceOneAsync(m => m.Id == id, routine);
        }
    }
}
