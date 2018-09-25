using Microsoft.EntityFrameworkCore;
using UtilizeJwtProvider.Models;

namespace UtilizeJwtProvider.DataSources
{
    public class UserDbContext : DbContext {
        
        public DbSet<User> Users { get; set; }
        
//        public DbSet<Licence> Licences { get; set; }
//        
//        public DbSet<Roles> Roles { get; set; }
//        
//        public DbSet<Permission> Permissions { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        
    }
    
    
}