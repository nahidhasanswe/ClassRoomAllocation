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
    public class TeachersRepository : ITeachers
    {
        private readonly DbContext _context;

        public TeachersRepository()
        {
            _context = new DbContext();
        }

        public async Task Add(Teachers teacher)
        {
            await _context.Teachers.InsertOneAsync(teacher);
        }

        public async Task<IEnumerable<Teachers>> Get()
        {
            return await _context.Teachers.Find(x=>true).ToListAsync();
        }

        public async Task<Teachers> Get(string id)
        {
            return await _context.Teachers.Find(Builders<Teachers>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
        }

        public async Task<Teachers> GetByUserNameAsync(string userName)
        {
            return await _context.Teachers.Find(Builders<Teachers>.Filter.Eq("TeacherInitial", userName)).FirstOrDefaultAsync();
        }

        public async Task<DeleteResult> Remove(string id)
        {
            return await _context.Teachers.DeleteOneAsync(Builders<Teachers>.Filter.Eq("Id", id));
        }

        public async Task<DeleteResult> RemoveAll()
        {
            return await _context.Teachers.DeleteManyAsync(new BsonDocument());
        }

        public async Task Update(string id, Teachers teacher)
        {
            await _context.Teachers.ReplaceOneAsync(Builders<Teachers>.Filter.Eq("Id", id),teacher);
        }
    }
}
