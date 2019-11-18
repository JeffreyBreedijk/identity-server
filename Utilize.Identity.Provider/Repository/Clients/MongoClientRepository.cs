using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Utilize.Identity.Provider.Options;

namespace Utilize.Identity.Provider.Repository.Clients
{
    public class MongoClientRepository : IdentityServer4.Stores.IClientStore, IClientWriteStore
    {
        private static IMongoDatabase _database;

        public MongoClientRepository(IOptions<ConfigurationOptions> optionsAccessor)
        {
            var configurationOptions = optionsAccessor.Value;

            IMongoClient client = new MongoClient(configurationOptions.MongoConnection);
            _database = client.GetDatabase(configurationOptions.MongoDatabaseName);
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var filter = Builders<Client>.Filter.Eq("id", clientId);
            return await _database.GetCollection<Client>(typeof(Client).Name).Find(database => database.ClientId == clientId).FirstOrDefaultAsync();
        }

        public async Task UpdateClient(Client client)
        {
            var filter = Builders<Client>.Filter.Eq("_id", client.ClientId);
            await _database.GetCollection<Client>(typeof(Client).Name).ReplaceOneAsync(filter, client);
        }

        public async Task Add(Client client)
        {
           await _database.GetCollection<Client>(typeof(Client).Name).InsertOneAsync(client);
        }
    }
}