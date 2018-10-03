using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Utilize.Identity.Provider.Repository;

namespace Utilize.Identity.Provider.Services
{
    public class ResourceService : IResourceStore
    {
        private readonly IIdentityServerRepository _identityServerRepository;

        public ResourceService(IIdentityServerRepository identityServerRepository)
        {
            _identityServerRepository = identityServerRepository;
        }

        private IEnumerable<ApiResource> GetAllApiResources()
        {
            return _identityServerRepository.All<ApiResource>();
        }

        private IEnumerable<IdentityResource> GetAllIdentityResources()
        {
            return _identityServerRepository.All<IdentityResource>();
        }

        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return Task.FromResult(_identityServerRepository.Single<ApiResource>(a => a.Name == name));
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var result = new Resources(GetAllIdentityResources(), GetAllApiResources());
            return Task.FromResult(result);
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var list = _identityServerRepository.Where<ApiResource>(a => a.Scopes.Any(s => scopeNames.Contains(s.Name)));

            return Task.FromResult(list.AsEnumerable());
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var list = _identityServerRepository.Where<IdentityResource>(e => scopeNames.Contains(e.Name));

            return Task.FromResult(list.AsEnumerable());
        }

        private Func<IdentityResource, bool> BuildPredicate(Func<IdentityResource, bool> predicate)
        {
            return predicate;
        }

    }
}