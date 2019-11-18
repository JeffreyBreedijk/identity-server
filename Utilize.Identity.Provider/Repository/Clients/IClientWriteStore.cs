using System.Threading.Tasks;
using IdentityServer4.Models;

namespace Utilize.Identity.Provider.Repository.Clients
{
    public interface IClientWriteStore
    {
        Task UpdateClient(Client client);
        Task Add(Client client);
    }
}