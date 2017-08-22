
using CQRSlite.Events;
using Microsoft.EntityFrameworkCore;
using UtilizeJwtProvider.Domain.Event;
using UtilizeJwtProvider.Domain.Event.Data;


namespace UtilizeJwtProvider
{
    public class EventDbContext : DbContext {
        
        public DbSet<EventData> Events { get; set; }
       
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseMySql(@"Server=localhost;database=ef;uid=root;pwd=root;");
        
    }
    
    
}