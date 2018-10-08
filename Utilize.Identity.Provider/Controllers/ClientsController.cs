using IdentityServer4.Models;
using IdentityServer4.Stores;
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
        private readonly IPermissionSchemeService _permissionSchemeService;

        public ClientsController(IClientStore clientStore, IClientWriteStore writeStore, IPermissionSchemeService permissionSchemeService)
        {
            _clientStore = clientStore;
            _writeStore = writeStore;
            _permissionSchemeService = permissionSchemeService;
        }

        [HttpGet]
        [Route("{clientId}")]
        public ActionResult<Client> GetTenant([FromRoute] string clientId)
        {
            var client = _clientStore.FindClientByIdAsync(clientId).Result;
            if (client == null)
                return NotFound();
            return client;
        }    
                
        [HttpPost]
        [Route("{clientId}")]
        public ActionResult InsertClient([FromRoute] string clientId)
        {
            if (_clientStore.FindClientByIdAsync(clientId).Result != null)
                return UnprocessableEntity();
            
            var client = new Client()
            {
                ClientId = clientId,
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword
            };
            _writeStore.Add(client);

            return NoContent();
        }
        
        [HttpPost]
        [Route("{clientId}/scope/{scopeId}")]
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