using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Utilize.Identity.Provider.Repository;
using Utilize.Identity.Provider.Services;
using TenantDto = Utilize.Identity.Provider.DTO.TenantDto;

namespace Utilize.Identity.Provider.Controllers
{
    [Route("[controller]")]
    public class ClientController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IClientWriteStore _writeStore;

        public ClientController(IClientStore clientStore, IClientWriteStore writeStore)
        {
            _clientStore = clientStore;
            _writeStore = writeStore;
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