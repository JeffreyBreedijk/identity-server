using System.Collections.Generic;
using System.Linq;
using System.Net;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UtilizeJwtProvider.DTO;
using UtilizeJwtProvider.Models;
using UtilizeJwtProvider.Services;


namespace UtilizeJwtProvider.Controllers
{
    [Route("[controller]")]
    public class TenantController : Controller
    {
        private readonly ITenantService _tenantService;
        private readonly ConfigurationDbContext _configurationDbContext;

        public TenantController(ITenantService tenantService, ConfigurationDbContext configurationDbContext)
        {
            _tenantService = tenantService;
            _configurationDbContext = configurationDbContext;
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
        public ActionResult UpdateTenant([FromRoute] string tenantId)
        {
            _tenantService.CreateTenant(tenantId);

            return NoContent();
        }
        
        [HttpPost]
        [Route("{tenantId}/scope/{scopeId}")]
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