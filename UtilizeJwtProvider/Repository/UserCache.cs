using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using UtilizeJwtProvider.DataSources;
using UtilizeJwtProvider.Domain.Aggregates;

namespace UtilizeJwtProvider.Repository
{
    public interface IUserCache
    {
        void AddUserToCache(User user);
        User FindUserByLoginCode(string loginCode);
        bool UserExists(string loginCode);
    }

    public class UserCache : IUserCache
    {
        private readonly IMemoryCache _cache;
       

        public UserCache(IMemoryCache memoryCache)
        {
            _cache = memoryCache;    
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