using System.Threading.Tasks;
using IdentityServer4.Models;
using Utilize.Identity.Provider.Repository;

namespace Utilize.Identity.Provider.Services
{
    public class CustomClientStore : IdentityServer4.Stores.IClientStore
    {

        private readonly IRepository _repository;

        public CustomClientStore(IRepository repository)
        {
            _repository = repository;
        }

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = _repository.ClientById(clientId);

            return Task.FromResult(client);
        }
    }
}