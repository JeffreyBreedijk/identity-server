using System;
using Microsoft.EntityFrameworkCore;
using Utilize.Identity.Provider.Models;
using Licence = Utilize.Identity.Shared.Models.Licence;
using Permission = Utilize.Identity.Shared.Models.Permission;
using Role = Utilize.Identity.Shared.Models.Role;

namespace Utilize.Identity.Provider.DataSources
{
    public class AuthDbContext : Microsoft.EntityFrameworkCore.DbContext {
        
        public DbSet<User> Users { get; set; }
        
        public DbSet<Licence> Licences { get; set; }
        
        public DbSet<Role> Roles { get; set; }
        
        public DbSet<Permission> Permissions { get; set; }
        
        public DbSet<PermissionScheme> PermissionSchemes { get; set; }
        
        public DbSet<Tenant> Tenants { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {

            

           base.OnModelCreating(builder);

            
            builder.Entity<PermissionScheme>().HasData(new PermissionScheme()
            {
                Id = Guid.NewGuid(),
                Name = "Default Permission Scheme",
                Tenant = null
            });
            
        }
        
    }
    
    
}