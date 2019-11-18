using System.Threading.Tasks;
using IdentityServer4.Models;

namespace Utilize.Identity.Provider.Repository
{
    public interface IResourceWriteStore
    {
        Task UpdateApiResource(ApiResource resource);
        Task AddApiResource(ApiResource resource);
    }
}