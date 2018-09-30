using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Utilize.Identity.Shared.DataSources;
using Utilize.Identity.Shared.Models;

namespace Utilize.Identity.Shared.Repository
{
    public interface ITenantRepository
    {
        void CreateTenant(Tenant tenant);
        void UpdateTenant(Tenant tenant);
        Tenant GetTenant(string tenantId);
        void DeleteTenant(Tenant tenant);
    }

    public class TenantRepository : ITenantRepository
    {
        private readonly AuthDbContext _authDbContext;
        private readonly ConfigurationDbContext _identityServerContext;

        public TenantRepository(AuthDbContext authDbContext, ConfigurationDbContext identityServerContext)
        {
            _authDbContext = authDbContext;
            _identityServerContext = identityServerContext;
        }

        public void CreateTenant(Tenant tenant)
        {
            _authDbContext.Tenants.Add(tenant);
            _authDbContext.SaveChanges();
            var client = new Client
            {
                ClientId = tenant.Id,
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword
            };
            _identityServerContext.Clients.Add(client.ToEntity());
            _identityServerContext.SaveChanges();
        }

        public void UpdateTenant(Tenant tenant)
        {
            _authDbContext.Tenants.Update(tenant);
            _authDbContext.SaveChanges();
        }

        public Tenant GetTenant(string tenantId)
        {
            return _authDbContext.Tenants.FirstOrDefault(t => t.Id.Equals(tenantId.ToLowerInvariant()));
        }

        public void DeleteTenant(Tenant tenant)
        {
            _authDbContext.Tenants.Remove(tenant);
            _authDbContext.SaveChanges();
        }

    }
}