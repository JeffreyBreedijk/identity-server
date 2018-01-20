using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using UtilizeJwtProvider.DataSources;
using UtilizeJwtProvider.Domain.Aggregates;
using UtilizeJwtProvider.Repository;

namespace UtilizeJwtProvider.Services
{
    public interface ICacheWarmer
    {
        void WarmCache();
    }

    public class CacheWarmer : ICacheWarmer
    {
        private readonly IMemoryCache _cache;
        private readonly EventDbContext _eventDbContext;
        private readonly IEventRepository _eventRepository;

        public CacheWarmer(EventDbContext dbContext, IEventRepository eventRepository, IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            _eventDbContext = dbContext;
            _eventRepository = eventRepository;
            dbContext.Database.EnsureCreated();
            //TODO: Add system user
        }

        public void WarmCache()
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.NeverRemove);
            foreach (var user in _eventDbContext.Events
                .Where(e => e.AggregateType.Equals("User"))
                .Select(e => e.AggregateId)
                .Distinct().AsEnumerable()
                .Select(_eventRepository.GetById<User>))
            {
                _cache.Set(user.LoginCode, user, cacheEntryOptions);
            }
        }
    }
}