using Microsoft.EntityFrameworkCore;
using UtilizeJwtProvider.Domain.Event.Data;

namespace UtilizeJwtProvider.DataSources
{
    public class EventDbContext : DbContext {
        
        public DbSet<EventData> Events { get; set; }

        public EventDbContext(DbContextOptions options) : base(options)
        {
        }
        
    }
    
    
}