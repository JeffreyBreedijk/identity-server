using System.Threading.Tasks;
using IdentityServer4.Models;
using Utilize.Identity.Provider.Repository;

namespace Utilize.Identity.Provider.Services
{
    public interface IClientWriteStore
    {
        void UpdateClient(Client client);
        Task Add(Client client);
    }
    
    public class ClientService : IdentityServer4.Stores.IClientStore, IClientWriteStore
    {
        private readonly IIdentityServerRepository _identityServerRepository;

        public ClientService(IIdentityServerRepository identityServerRepository)
        {
            _identityServerRepository = identityServerRepository;
        }

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = _identityServerRepository.ClientById(clientId);

            return Task.FromResult(client);
        }

        public void UpdateClient(Client client)
        {
            _identityServerRepository.UpdateClient(client);
        }

        public async Task Add(Client client)
        {
           await _identityServerRepository.Add(client);
        }
    }
}