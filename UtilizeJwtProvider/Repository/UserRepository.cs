using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using UtilizeJwtProvider.DataSources;
using UtilizeJwtProvider.Domain.Aggregates;

namespace UtilizeJwtProvider.Repository
{
    public interface IUserRepository
    {
        void AddUserToCache(User user);
        User FindUserByLoginCode(string loginCode);
        bool UserExists(string loginCode);
    }

    public class InMemoryUserRepository : IUserRepository
    {
        private readonly IMemoryCache _cache;
        private readonly EventDbContext _eventDbContext;
        private readonly IEventRepository _eventRepository;

        public InMemoryUserRepository(EventDbContext dbContext, IEventRepository eventRepository, IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            _eventDbContext = dbContext;
            _eventRepository = eventRepository;
            dbContext.Database.EnsureCreated();
            PopulateCache();
        }

        private void PopulateCache()
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

        public void AddUserToCache(User user)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.NeverRemove);
            _cache.Set(user.LoginCode, user, cacheEntryOptions);
        }

        public User FindUserByLoginCode(string loginCode)
        {
            return _cache.TryGetValue(loginCode, out User cacheEntry) ? cacheEntry : null;
        }

        public bool UserExists(string loginCode)
        {
            return _cache.TryGetValue(loginCode, out User _);
        }


    }
}