using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilize.Identity.Provider.Services;

namespace Utilize.Identity.Provider.Controllers
{
    [Route("[controller]")]
    public class ScopeController  : ControllerBase
    {
        private readonly ClientService _clientService;
        private readonly IClientWriteStore _clientWriteStore;

        public ScopeController(ClientService clientService, IClientWriteStore clientWriteStore)
        {
            _clientService = clientService;
            _clientWriteStore = clientWriteStore;
        }


        [HttpPost]
        [Route("{scopeId}")]
        public ActionResult AddScope([FromRoute] string clientId, [FromRoute] string scopeId)
        {
            // todo: check existence of scope
            var client = _clientService.FindClientByIdAsync(clientId).Result;
            if (client == null)
                return NotFound();
            
            client.AllowedScopes.Add(scopeId);
            _clientWriteStore.UpdateClient(client);
            return NoContent();
        } 
        
        
    }
}