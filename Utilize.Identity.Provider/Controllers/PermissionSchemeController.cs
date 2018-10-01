using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Utilize.Identity.Shared.DTO;
using Utilize.Identity.Shared.Services;
using IPermissionSchemeService = Utilize.Identity.Provider.Services.IPermissionSchemeService;
using ITenantService = Utilize.Identity.Provider.Services.ITenantService;
using PermissionSchemeDto = Utilize.Identity.Provider.DTO.PermissionSchemeDto;

namespace Utilize.Identity.Provider.Controllers
{
    [Route("[controller]")]
    public class PermissionSchemeController : Controller
    {
        private readonly ITenantService _tenantService;
        private readonly IPermissionSchemeService _permissionSchemeService;

        public PermissionSchemeController(IPermissionSchemeService permissionSchemeService, 
            ITenantService tenantService)
        {
            _permissionSchemeService = permissionSchemeService;
            _tenantService = tenantService;
        }

       
        [HttpGet]
        public ActionResult<List<PermissionSchemeDto>> GetPermissionSchemes()
        {
            var x = User.Claims.Select(c => c.Value).ToList();;
            var tenantId = "";
            // todo check user claim for tenant id
            var tenant = _tenantService.GetTenant(tenantId);
            if (tenant == null)
                return NotFound();
            return _permissionSchemeService.GetPermissionSchemesForTenant(tenant).Select(p => p.ToDto()).ToList();
        }
       

        [HttpPut]
        public ActionResult PutPermissionScheme([FromBody] PermissionSchemeDto permissionSchemeDto)
        {
            var tenantId = "";
            // todo check user claim for tenant id
            var tenant = _tenantService.GetTenant(tenantId);
            if (tenant == null)
                return NotFound();
             _permissionSchemeService.CreatePermissionScheme(tenant, permissionSchemeDto);
            return NoContent();
        }
    }
}