using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Utilize.Identity.Provider.Services
{
    public class ReferenceTokenStore : IReferenceTokenStore
    {
        private readonly IDistributedCache _cache;

        public ReferenceTokenStore(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<string> StoreReferenceTokenAsync(Token token)
        {
            var options = new DistributedCacheEntryOptions(); 
            options.SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));   
            var reference = Guid.NewGuid();
            var claims = JsonConvert.SerializeObject(token.Claims.ToDictionary(c => c.Type, c => c.Value));
            token.Claims = new List<Claim>();
            var json = JsonConvert.SerializeObject(token);
            await _cache.SetStringAsync(reference.ToString(), json, options);
            await _cache.SetStringAsync(reference + ".claims", claims, options);
            return reference.ToString();
        }

        public async Task<Token> GetReferenceTokenAsync(string handle)
        {
            var rawToken = await _cache.GetStringAsync(handle);
            var rawClaims = await _cache.GetStringAsync(handle + ".claims");
            var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawClaims)
                .Select(c => new Claim(c.Key, c.Value))
                .ToList();
            var token = JsonConvert.DeserializeObject<Token>(rawToken);
            token.Claims = claims;
            return token;
        }

        public async Task RemoveReferenceTokenAsync(string handle)
        {
            await _cache.RemoveAsync(handle);
            await _cache.RemoveAsync(handle + ".claims");
        }

        public Task RemoveReferenceTokensAsync(string subjectId, string clientId)
        {
            throw new System.NotImplementedException();
        }
    }
}