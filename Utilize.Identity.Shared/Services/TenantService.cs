using Utilize.Identity.Shared.Models;
using Utilize.Identity.Shared.Repository;

namespace Utilize.Identity.Shared.Services
{
    public interface ITenantService
    {
        Tenant GetTenant(string tenantId);
        void UpdateTenant(Tenant tenant);
        void CreateTenant(string tenantId);
    }

    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantService(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public Tenant GetTenant(string tenantId)
        {
            return _tenantRepository.GetTenant(tenantId);
        }

        public void UpdateTenant(Tenant tenant)
        {
           _tenantRepository.UpdateTenant(tenant);
        }

        public void CreateTenant(string tenantId)
        {
            _tenantRepository.CreateTenant(new Tenant()
            {
                Id = tenantId.ToLowerInvariant(),
                Name = null,
                Active = true
            });
            
            
        }
        
    }
}