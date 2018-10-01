using System.Linq;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Utilize.Identity.Provider.Repository;
using Utilize.Identity.Shared.DTO;
using Utilize.Identity.Shared.Services;
using ITenantService = Utilize.Identity.Provider.Services.ITenantService;
using TenantDto = Utilize.Identity.Provider.DTO.TenantDto;

namespace Utilize.Identity.Provider.Controllers
{
    [Route("[controller]")]
    public class TenantController : Controller
    {
        private readonly ITenantService _tenantService;
        private readonly IRepository _repository;

        public TenantController(ITenantService tenantService, IRepository repository)
        {
            _tenantService = tenantService;
            _repository = repository;
        }

        [HttpGet]
        [Route("{tenantId}")]
        public ActionResult<TenantDto> GetTenant([FromRoute] string tenantId)
        {
            var tenant = _tenantService.GetTenant(tenantId);
            if (tenant == null)
                return NotFound();
            return tenant.ToDto();
        }    
                
        [HttpPost]
        [Route("{tenantId}")]
        public ActionResult InsertTenant([FromRoute] string tenantId)
        {
            if (_repository.ClientById(tenantId) != null)
                return UnprocessableEntity();
            
            var client = new Client()
            {
                ClientId = tenantId,
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword
            };
            _repository.Add(client);

            return NoContent();
        }
        
        [HttpPost]
        [Route("{tenantId}/scope/{scopeId}")]
        public ActionResult AddScope([FromRoute] string tenantId, [FromRoute] string scopeId)
        {
            var client = _repository.ClientById(tenantId);
            if (client == null)
                return NotFound();
            client.AllowedScopes.Add(scopeId);
            _repository.UpdateClient(client);    
           
            return NoContent();
        } 
        
    }
}