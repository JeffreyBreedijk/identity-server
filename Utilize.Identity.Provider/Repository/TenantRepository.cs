using System.Linq;
using Utilize.Identity.Provider.DataSources;
using Utilize.Identity.Provider.Models;

namespace Utilize.Identity.Provider.Repository
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
        public TenantRepository(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public void CreateTenant(Tenant tenant)
        {
            _authDbContext.Tenants.Add(tenant);
            _authDbContext.SaveChanges();

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