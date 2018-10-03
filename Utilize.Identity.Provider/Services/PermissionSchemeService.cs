using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Utilize.Identity.Provider.DataSources;
using Utilize.Identity.Provider.DTO;
using Utilize.Identity.Provider.Models;
using Utilize.Identity.Provider.Repository;

namespace Utilize.Identity.Provider.Services
{
    public interface IPermissionSchemeService
    {
        Task CreatePermissionScheme(string clientId, PermissionSchemeDto permissionSchemeDto);
        Task<PermissionScheme> GetDefaultPermissionScheme();
        Task<List<PermissionScheme>> GetPermissionSchemesForTenant(string clientId);
    }

    public class PermissionSchemeService : IPermissionSchemeService
    {
        private readonly AuthDbContext _authDbContext;

        public PermissionSchemeService(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public async Task CreatePermissionScheme(string clientId, PermissionSchemeDto permissionSchemeDto)
        {
            await _authDbContext.PermissionSchemes.AddAsync(new PermissionScheme()
            {
                Id = Guid.NewGuid(),
                IsActive = permissionSchemeDto.IsActive,
                Name = permissionSchemeDto.Name,
                Tenant = clientId
            });
            await _authDbContext.SaveChangesAsync();
        }

        public Task<PermissionScheme> GetDefaultPermissionScheme()
        {
            return _authDbContext.PermissionSchemes.FirstOrDefaultAsync(p => p.Tenant == null);
        }

        public Task<List<PermissionScheme>> GetPermissionSchemesForTenant(string clientId)
        {
            return _authDbContext.PermissionSchemes.Where(p => p.Tenant.Equals(clientId)).ToListAsync();
        }
    }
}