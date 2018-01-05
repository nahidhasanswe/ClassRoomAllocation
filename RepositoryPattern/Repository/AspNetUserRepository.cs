using MongoDB.Driver;
using RepositoryPattern.Model_Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.Repository
{
    public class AspNetUserRepository
    {
        private readonly DbContext _db;

        public AspNetUserRepository()
        {
            _db = new DbContext();
        }

        public async Task<string> GetRoleName(string name)
        {
            var result = await _db.Users.Find(Builders<AspNetUsers>.Filter.Eq("UserName", name)).FirstOrDefaultAsync();

            return result.Roles.FirstOrDefault();
        }

        public async Task<IEnumerable<AspNetUsers>> GetAll()
        {
            return await _db.Users.Find(x => true).ToListAsync();
        } 

        


    }
}
