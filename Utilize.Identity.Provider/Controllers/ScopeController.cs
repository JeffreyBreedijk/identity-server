using System.Linq;
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
        private readonly ConfigurationDbContext _configurationDbContext;
        private readonly ITenantService _tenantService;

        public ScopeController(ConfigurationDbContext configurationDbContext, ITenantService tenantService)
        {
            _configurationDbContext = configurationDbContext;
            _tenantService = tenantService;
        }

        [HttpPost]
        [Route("{scopeId}")]
        public ActionResult AddScope([FromRoute] string tenantId, [FromRoute] string scopeId)
        {
            // todo: check existence of scope
            var tenant = _tenantService.GetTenant(tenantId);
            if (tenant == null)
                return NotFound();
            
            var client = _configurationDbContext.Clients.AsNoTracking().FirstOrDefault(c => c.ClientId.Equals(tenantId));
            var id = client.Id;
            var clientModel = client.ToModel();
            clientModel.AllowedScopes.Add(scopeId);
            client = clientModel.ToEntity();
            client.Id = id;
            _configurationDbContext.Clients.Add(client);
            _configurationDbContext.Entry(client).State = EntityState.Modified;
            _configurationDbContext.SaveChanges();
            return NoContent();
        } 
        
        
    }
}