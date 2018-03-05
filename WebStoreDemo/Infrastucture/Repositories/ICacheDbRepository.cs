using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiantBomb.Api.Model;
using LiteDB;

namespace WebStoreDemo.Infrastucture.Repositories
{
    public interface ICacheDbRepository
    {
        void AddToCacheDb<T>(T objToStore, string storagePartiton);
        IEnumerable<T> GetItemsInPartition<T>(string storagePartiton);
        T GetItemInPartition<T>(string storagePartition, int id);
        bool IsItemInPartition(int id, string storagePartiton);
    }

    public class CacheDbRepository : ICacheDbRepository
    {
        public void AddToCacheDb<T>(T objToStore, string storagePartiton)
        {
            using (var db = new LiteDatabase($"cacheDb.db"))
            {
                var collection = db.GetCollection<T>(storagePartiton);
                collection.Insert(objToStore);
            }
        }

        public T GetItemInPartition<T>(string storagePartition, int id)
        {
            using (var db = new LiteDatabase($"cacheDb.db"))
            {
                var collection = db.GetCollection<T>(storagePartition);
                return collection.FindById(id);
            }
        }

        public IEnumerable<T> GetItemsInPartition<T>(string storagePartiton)
        {
            using (var db = new LiteDatabase($"cacheDb.db"))
            {
                var collection = db.GetCollection<T>(storagePartiton);
                return collection.FindAll();
            }
        }

        public bool IsItemInPartition(int id, string storagePartiton)
        {
            using (var db = new LiteDatabase($"cacheDb.db"))
            {
                var collection = db.GetCollection<Game>(storagePartiton);
                return collection.Exists(x => x.Id == id);
            }
        }
    }
}
