//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Internal;
//using Utilize.Identity.Provider.DataSources;
//using Utilize.Identity.Provider.Helpers;
//using Utilize.Identity.Provider.Models;
//
//namespace Utilize.Identity.Provider.Services
//{
//    public interface IRoleService
//    {
//        Task<bool> RolePresent(PermissionScheme permissionScheme, string role);
//        Task CreateRole(PermissionScheme permissionScheme, Role role);
//        Task<List<Role>> GetRolesForPermissionScheme(PermissionScheme permissionScheme);
//    }
//
//    public class RoleService : IRoleService
//    {
//        private readonly AuthDbContext _dbContext;
//
//        public RoleService(AuthDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }
//
//        public async Task<bool> RolePresent(PermissionScheme permissionScheme, string role)
//        {
//            return await _dbContext.Roles
//                .Where(r => r.Name.Equals(role) && r.PermissionScheme.Equals(permissionScheme))
//                .AnyAsync();
//        }
//        
//        public async Task CreateRole(PermissionScheme permissionScheme, Role role)
//        {
//            await _dbContext.AddAsync(role);
//            await _dbContext.SaveChangesAsync();
//        }
//
//        public async Task<List<Role>> GetRolesForPermissionScheme(PermissionScheme permissionScheme)
//        {
//            return await _dbContext.Roles.Where(r => r.PermissionScheme.Equals(permissionScheme)).ToListAsync();
//        }
//        
//
//    }
//}