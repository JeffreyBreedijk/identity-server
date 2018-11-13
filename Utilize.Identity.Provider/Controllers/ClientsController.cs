using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilize.Identity.Provider.Repository;
using Utilize.Identity.Provider.Services;

namespace Utilize.Identity.Provider.Controllers
{
    [Route("[controller]")]
    public class ClientsController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IClientWriteStore _writeStore;

        public ClientsController(IClientStore clientStore, IClientWriteStore writeStore)
        {
            _clientStore = clientStore;
            _writeStore = writeStore;
        }

        [HttpGet]
        [Authorize]
        public string test()
        {
            return User.Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.ClientId))?.Value;
        }
        
        [HttpPost]
        [Route("{clientId}")]
        public async Task<ActionResult> AddApiClient([FromRoute] string clientId)
        {
            var client = new Client()
            {
                ClientId = clientId,
                AccessTokenType = AccessTokenType.Reference,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                ClientSecrets = new List<Secret>(),
                UpdateAccessTokenClaimsOnRefresh = true,
                AllowAccessTokensViaBrowser = true,
                AllowOfflineAccess = true,
                RequireClientSecret = true,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials
            };
            await _writeStore.Add(client);
            return Ok();
        }
        
        [HttpPost]
        [Route("{clientId}/secrets")]
        public async Task<ActionResult<string>> AddSecret([FromRoute] string clientId)
        {
            var client = await _clientStore.FindClientByIdAsync(clientId);
            if (client == null)
                return NotFound("Client not found");
            var secret = new Secret(Guid.NewGuid().ToString("N").Sha256());
            client.ClientSecrets.Add(secret);            
            _writeStore.UpdateClient(client);
            return Ok(secret.Value);
        }
        
        [HttpPost]
        [Route("{clientId}/scopes/{scopeId}")]
        public ActionResult AddScope([FromRoute] string clientId, [FromRoute] string scopeId)
        {
            var client = _clientStore.FindClientByIdAsync(clientId).Result;
            if (client == null)
                return NotFound();
            client.AllowedScopes.Add(scopeId);
            _writeStore.UpdateClient(client);    
           
            return NoContent();
        } 
        
    }
}