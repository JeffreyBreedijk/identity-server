using System;
using System.Collections.Generic;
using System.Linq;
using Utilize.Identity.Shared.DTO;
using Utilize.Identity.Shared.Models;
using Utilize.Identity.Shared.Repository;

namespace Utilize.Identity.Shared.Services
{
    public interface IPermissionSchemeService
    {
        void CreatePermissionScheme(Tenant tenant, PermissionSchemeDto permissionSchemeDto);
        void ActivatePermissionScheme(Tenant tenant, PermissionScheme permissionScheme);
        PermissionScheme GetDefaultPermissionScheme();
        List<PermissionScheme> GetPermissionSchemesForTenant(Tenant tenant);
    }

    public class PermissionSchemeService : IPermissionSchemeService
    {
        private readonly IPermissionSchemeRepository _permissionSchemeRepository;

        public PermissionSchemeService(IPermissionSchemeRepository permissionSchemeRepository)
        {
            _permissionSchemeRepository = permissionSchemeRepository;
        }

        public void CreatePermissionScheme(Tenant tenant, PermissionSchemeDto permissionSchemeDto)
        {
            _permissionSchemeRepository.CreatePermissionScheme(new PermissionScheme()
            {
                Id = Guid.NewGuid(),
                IsActive = permissionSchemeDto.IsActive,
                Name = permissionSchemeDto.Name,
                Tenant = tenant
            });
        }

        public List<PermissionScheme> GetPermissionSchemesForTenant(Tenant tenant)
        {
            return _permissionSchemeRepository.GetPermissionSchemeByTenant(tenant);
        }
        
        public void ActivatePermissionScheme(Tenant tenant, PermissionScheme permissionScheme)
        {
            foreach (var scheme in _permissionSchemeRepository.GetPermissionSchemeByTenant(tenant)
                .Where(s => !s.Equals(permissionScheme)))
            {
                scheme.IsActive = false;
                _permissionSchemeRepository.UpdatePermissionScheme(scheme);
            }

            permissionScheme.IsActive = true;
            _permissionSchemeRepository.UpdatePermissionScheme(permissionScheme);
        }

        public PermissionScheme GetDefaultPermissionScheme()
        {
            return _permissionSchemeRepository.GetPermissionSchemeByTenant(null).First();
        }
    }
}