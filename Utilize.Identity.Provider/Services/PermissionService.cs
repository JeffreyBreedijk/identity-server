using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Utilize.Identity.Provider.DataSources;
using Utilize.Identity.Provider.Models;

namespace Utilize.Identity.Provider.Services
{
    public interface IPermissionService
    {
        Task AddPermission(string name, Licence licence);
        Task<Permission> GetPermission(string name);
        Task<List<Permission>> GetPermissionByLicence(Licence licence);
        Task SavePermission(Permission permission);
    }

    public class PermissionService : IPermissionService
    {
        private readonly AuthDbContext _dbContext;

        public PermissionService(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPermission(string name, Licence licence)
        {
            await _dbContext.Permissions.AddAsync(new Permission()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Licence = licence
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Permission> GetPermission(string name)
        {
            return await _dbContext.Permissions.FirstOrDefaultAsync(p => p.Name.Equals(name));
        }
        
        public async Task<List<Permission>> GetPermissionByLicence(Licence licence)
        {
            return await _dbContext.Permissions.Where(l => l.Licence.Equals(licence)).ToListAsync();
        }

        public async Task SavePermission(Permission permission)
        {
            _dbContext.Update(permission);
            await _dbContext.SaveChangesAsync();
        }
    }
}