using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using IPermissionSchemeService = Utilize.Identity.Provider.Services.IPermissionSchemeService;
using PermissionSchemeDto = Utilize.Identity.Provider.DTO.PermissionSchemeDto;

namespace Utilize.Identity.Provider.Controllers
{
    [Route("[controller]")]
    public class PermissionSchemeController : Controller
    {
        private readonly IPermissionSchemeService _permissionSchemeService;
        private readonly IClientStore _clientStore;

        public PermissionSchemeController(IPermissionSchemeService permissionSchemeService, IClientStore clientStore)
        {
            _permissionSchemeService = permissionSchemeService;
            _clientStore = clientStore;
        }

       
        [HttpGet]
        public ActionResult<List<PermissionSchemeDto>> GetPermissionSchemes()
        {
//            var x = User.Claims.Select(c => c.Value).ToList();;
            var clientId = "";
            // todo check user claim for tenant id
            var client = _clientStore.FindClientByIdAsync(clientId).Result;
            if (client == null)
                return NotFound();
            return _permissionSchemeService.GetPermissionSchemesForTenant(clientId).Result.Select(p => p.ToDto()).ToList();
        }
       

        [HttpPut]
        public ActionResult PutPermissionScheme([FromBody] PermissionSchemeDto permissionSchemeDto)
        {
            var clientId = "";
            // todo check user claim for tenant id
            var client = _clientStore.FindClientByIdAsync(clientId).Result;
            if (client == null)
                return NotFound();
             _permissionSchemeService.CreatePermissionScheme(clientId, permissionSchemeDto);
            return NoContent();
        }
    }
}