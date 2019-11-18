using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Utilize.Identity.Provider.Options;

namespace Utilize.Identity.Provider.Repository.Mongo
{
    public class ResourceRepository : IResourceStore, IResourceWriteStore
    {
        private static IMongoDatabase _database;

        public ResourceRepository(IOptions<ConfigurationOptions> optionsAccessor)
        {
            var configurationOptions = optionsAccessor.Value;

            IMongoClient client = new MongoClient(configurationOptions.MongoConnection);
            _database = client.GetDatabase(configurationOptions.MongoDatabaseName);
        }

        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return Task.FromResult(_database
                .GetCollection<ApiResource>(typeof(ApiResource).Name)
                .AsQueryable()
                .SingleOrDefault(a => a.Name.Equals(name)));
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var result = new Resources(new List<IdentityResource>(), GetAllApiResources());
            return Task.FromResult(result);
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            return Task.FromResult((IEnumerable<IdentityResource>) new List<IdentityResource>());
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            return Task.FromResult((IEnumerable<ApiResource>) _database
                .GetCollection<ApiResource>(typeof(ApiResource).Name)
                .AsQueryable()
                .Where(a => a.Scopes.Any(s => scopeNames.Contains(s.Name)))
                .ToList());
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

        private IEnumerable<ApiResource> GetAllApiResources()
        {
            return _database.GetCollection<ApiResource>(typeof(ApiResource).Name).AsQueryable();
        }
    }
}