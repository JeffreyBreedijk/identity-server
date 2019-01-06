using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Utilize.Identity.Provider.Options;

namespace Utilize.Identity.Provider.Repository
{
    public interface IResourceWriteStore
    {
        Task UpdateApiResource(ApiResource resource);
        Task AddApiResource(ApiResource resource);
    }
    
    public class ResourceRepository : IResourceStore, IResourceWriteStore
    {
        private static IMongoDatabase _database;

        public ResourceRepository(IOptions<ConfigurationOptions> optionsAccessor)
        {
            var configurationOptions = optionsAccessor.Value;

            IMongoClient client = new MongoClient(configurationOptions.MongoConnection);
            _database = client.GetDatabase(configurationOptions.MongoDatabaseName);
        }

        private IEnumerable<ApiResource> GetAllApiResources()
        {
            return _database.GetCollection<ApiResource>(typeof(ApiResource).Name).AsQueryable();
        }

        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return await _database.GetCollection<ApiResource>(typeof(ApiResource).Name)
                .AsQueryable().Where(a => a.Name.Equals(name))
                .SingleOrDefaultAsync();       
        }

        public  Task<Resources> GetAllResourcesAsync()
        {
            var result = new Resources(new List<IdentityResource>(), GetAllApiResources());
            return Task.FromResult(result);
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            return await _database.GetCollection<ApiResource>(typeof(ApiResource).Name)
                .AsQueryable()
                .Where(a => a.Scopes.Any(s => scopeNames.Contains(s.Name)))
                .ToListAsync();
        }


        public async Task UpdateApiResource(ApiResource resource)
        {
            var filter = Builders<ApiResource>.Filter.Eq("_id", resource.Name);
            await _database.GetCollection<ApiResource>(typeof(ApiResource).Name).ReplaceOneAsync(filter, resource);
        }

        public async Task AddApiResource(ApiResource resource)
        {
            await _database.GetCollection<ApiResource>(typeof(ApiResource).Name).InsertOneAsync(resource);
        }
    }
}