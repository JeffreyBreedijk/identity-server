using Microsoft.EntityFrameworkCore;
using UtilizeJwtProvider.Models;

namespace UtilizeJwtProvider.DataSources
{
    public class UserDbContext : DbContext {
        
        public DbSet<User> Users { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        
    }
    
    
}