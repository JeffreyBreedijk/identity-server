using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UtilizeJwtProvider.Models;

namespace UtilizeJwtProvider
{
    public class ApplicationDbContext : DbContext {
        
        public DbSet<User> Users { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseMySql(@"Server=localhost;database=ef;uid=root;pwd=root;");
        
    }
    
    
}