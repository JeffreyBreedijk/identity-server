using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using IdentityServer4.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Utilize.Identity.Provider.Options;

namespace Utilize.Identity.Provider.Repository
{
    public interface IRepository
    {
        IQueryable<T> All<T>() where T : class, new();
        IQueryable<T> Where<T>(Expression<Func<T, bool>> expression) where T : class, new();
        T Single<T>(Expression<Func<T, bool>> expression) where T : class, new();
        void Delete<T>(Expression<Func<T, bool>> expression) where T : class, new();
        void Add<T>(T item) where T : class, new();
        void Add<T>(IEnumerable<T> items) where T : class, new();
        bool CollectionExists<T>() where T : class, new();
        
        // Client specific
        Client ClientById(string clientId);
        void UpdateClient(Client item);
    }
    
        public class MongoRepository : IRepository
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;


        public MongoRepository(IOptions<ConfigurationOptions> optionsAccessor)
        {
            var configurationOptions = optionsAccessor.Value;

            _client = new MongoClient(configurationOptions.MongoConnection);
            _database = _client.GetDatabase(configurationOptions.MongoDatabaseName);
            
        }

        

        public IQueryable<T> All<T>() where T : class, new()
        {
            return _database.GetCollection<T>(typeof(T).Name).AsQueryable();
        }

        public IQueryable<T> Where<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return All<T>().Where(expression);
        }

        public void Delete<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            var result = _database.GetCollection<T>(typeof(T).Name).DeleteMany(predicate);

        }
        
      

        
        public T Single<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return All<T>().Where(expression).SingleOrDefault();
        }

        public bool CollectionExists<T>() where T : class, new()
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);
            var filter = new BsonDocument();
            var totalCount = collection.Count(filter);
            return (totalCount > 0) ? true : false;

        }

        public void Add<T>(T item) where T : class, new()
        {
            _database.GetCollection<T>(typeof(T).Name).InsertOne(item);
        }
        
        
      

        
        public void Add<T>(IEnumerable<T> items) where T : class, new()
        {
            _database.GetCollection<T>(typeof(T).Name).InsertMany(items);
        }

        
        // Client Specific
        public Client ClientById(string clientId)
        {
            var filter = Builders<Client>.Filter.Eq("_id", clientId);
            return _database.GetCollection<Client>(typeof(Client).Name).Find(filter).FirstOrDefault();
        }
        
        public void UpdateClient(Client item)
        {
            var filter = Builders<Client>.Filter.Eq("_id", item.ClientId);
            _database.GetCollection<Client>(typeof(Client).Name).ReplaceOne(filter, item);
        }

       
    }
}