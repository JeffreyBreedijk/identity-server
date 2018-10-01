using System;
using System.Collections.Generic;
using System.Linq;
using Utilize.Identity.Provider.DataSources;
using Utilize.Identity.Provider.Models;

namespace Utilize.Identity.Provider.Repository
{
    public interface IPermissionSchemeRepository
    {
        void CreatePermissionScheme(PermissionScheme permissionScheme);
        void UpdatePermissionScheme(PermissionScheme permissionScheme);
        PermissionScheme GetPermissionSchemeById(Guid id);
        List<PermissionScheme> GetPermissionSchemeByTenant(Tenant tenant);
        void DeletePermissionScheme(PermissionScheme permissionScheme);
    }

    public class PermissionSchemeRepository : IPermissionSchemeRepository
    {
        private readonly AuthDbContext _authDbContext;

        public PermissionSchemeRepository(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public void CreatePermissionScheme(PermissionScheme permissionScheme)
        {
            _authDbContext.PermissionSchemes.Add(permissionScheme);
            _authDbContext.SaveChanges();
        }

        public void UpdatePermissionScheme(PermissionScheme permissionScheme)
        {
            _authDbContext.PermissionSchemes.Update(permissionScheme);
            _authDbContext.SaveChanges();
        }

        public PermissionScheme GetPermissionSchemeById(Guid id)
        {
            return  _authDbContext.PermissionSchemes.FirstOrDefault(p => p.Id.Equals(id));
        }
        
        public List<PermissionScheme> GetPermissionSchemeByTenant(Tenant tenant)
        {
            return  _authDbContext.PermissionSchemes.Where(p => p.Tenant.Equals(tenant)).ToList();
        }

        public void DeletePermissionScheme(PermissionScheme permissionScheme)
        {
            _authDbContext.PermissionSchemes.Remove(permissionScheme);
            _authDbContext.SaveChanges();
        }
    }
}