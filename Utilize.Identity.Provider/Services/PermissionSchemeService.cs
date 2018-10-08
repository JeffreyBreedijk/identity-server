using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Utilize.Identity.Provider.DataSources;
using Utilize.Identity.Provider.Helpers;
using Utilize.Identity.Provider.Models;
using Utilize.Identity.Provider.Repository;

namespace Utilize.Identity.Provider.Services
{
    public interface IPermissionSchemeService
    {
        Task CreatePermissionScheme(PermissionScheme permissionScheme);
        Task<List<PermissionScheme>> GetPermissionSchemes(string clientId);
        Task SavePermissionScheme(PermissionScheme permissionScheme);
    }

    public class PermissionSchemeService : IPermissionSchemeService
    {
        private readonly AuthDbContext _authDbContext;

        public PermissionSchemeService(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public async Task CreatePermissionScheme(PermissionScheme permissionScheme)
        {
            await _authDbContext.PermissionSchemes.AddAsync(permissionScheme);
            await _authDbContext.SaveChangesAsync();
        }

        public async Task<List<PermissionScheme>> GetPermissionSchemes(string clientId)
        {
            return await _authDbContext.PermissionSchemes.Where(p => p.Client.Equals(clientId)).Include(p => p.Roles).ToListAsync();
        }

        public async Task SavePermissionScheme(PermissionScheme permissionScheme)
        {
            _authDbContext.Update(permissionScheme);
            await _authDbContext.SaveChangesAsync();
        }


    }
}