using MongoDB.Driver;
using RepositoryPattern.Model_Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace RepositoryPattern
{
    public class DbContext
    {
        public IMongoDatabase _db { get; }

        public DbContext()
        {
            MongoClient client = new MongoClient(ConfigurationManager.AppSettings.Get("connectionString"));
            _db = client.GetDatabase(ConfigurationManager.AppSettings.Get("Database"));
        }


        public IMongoCollection<Routine> Routine
        {
            get
            {
                return _db.GetCollection<Routine>("Routine");
            }
        }

        public IMongoCollection<RoomAllocation> RoomAllocation
        {
            get
            {
                return _db.GetCollection<RoomAllocation>("RoomAllocation");
            }
        }

        public IMongoCollection<RoomCancellation> RoomCancellation
        {
            get
            {
                return _db.GetCollection<RoomCancellation>("RoomCancellation");
            }
        }

        public IMongoCollection<Teachers> Teachers
        {
            get
            {
                return _db.GetCollection<Teachers>("Teachers");
            }
        }
    }
}
