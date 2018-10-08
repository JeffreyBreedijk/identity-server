using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilize.Identity.Provider.Models;
using Utilize.Identity.Provider.Services;
using IPermissionSchemeService = Utilize.Identity.Provider.Services.IPermissionSchemeService;

namespace Utilize.Identity.Provider.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class PermissionSchemesController : Controller
    {
        private readonly IPermissionSchemeService _permissionSchemeService;
        private readonly IClientStore _clientStore;

        public PermissionSchemesController(IPermissionSchemeService permissionSchemeService, IClientStore clientStore)
        {
            _permissionSchemeService = permissionSchemeService;
            _clientStore = clientStore;
        }

        [HttpGet]
        public async Task<ActionResult<List<PermissionScheme>>> GetPermissionSchemes()
        {
            var clientId = User.Claims.Where(c => c.Type.Equals(JwtClaimTypes.ClientId)).Select(c => c.Value).First();
            return await _permissionSchemeService.GetPermissionSchemes(clientId);
        }   

        [HttpPost]
        [Route("{permissionScheme}")]
        public async Task<ActionResult> PutPermissionScheme([FromRoute] string permissionScheme)
        {
            var clientId = User.Claims.Where(c => c.Type.Equals(JwtClaimTypes.ClientId)).Select(c => c.Value).First();
            var client = _clientStore.FindClientByIdAsync(clientId).Result;
            if (client == null)
                return NotFound();
            var scheme = new PermissionScheme(clientId, permissionScheme);
            await _permissionSchemeService.CreatePermissionScheme(scheme);
            return Created(new Uri("/[Controller]/" + scheme.Id, UriKind.Relative), scheme);
        }
        
        [HttpGet]
        [Route("{permissionScheme}")]
        public async Task<ActionResult<PermissionScheme>> GetPermissionScheme([FromRoute] string permissionScheme)
        {
            var clientId = User.Claims.Where(c => c.Type.Equals(JwtClaimTypes.ClientId)).Select(c => c.Value).First();
            var schemes = await _permissionSchemeService.GetPermissionSchemes(clientId);
            var selection = schemes.FirstOrDefault(s => s.Id.Equals(permissionScheme));
            if (selection == null)
            {
                return NotFound();
            }
            return selection;
        }
        
        [HttpGet]
        [Route("{permissionScheme}/roles")]
        public async Task<ActionResult<HashSet<Role>>> GetPermissionSchemeRoles([FromRoute] string permissionScheme)
        {
            var clientId = User.Claims.Where(c => c.Type.Equals(JwtClaimTypes.ClientId)).Select(c => c.Value).First();
            var schemes = await _permissionSchemeService.GetPermissionSchemes(clientId);
            var selectedScheme = schemes.FirstOrDefault(s => s.Id.Equals(permissionScheme));
            if (selectedScheme == null)
                return NotFound();
            return selectedScheme.Roles;
        }
        
        [HttpPut]
        [Route("{permissionScheme}/roles/{roleName}")]
        public async Task<ActionResult> UpdatePermissionSchemeRoles([FromRoute] string permissionScheme, [FromRoute] string roleName)
        {
            var clientId = User.Claims.Where(c => c.Type.Equals(JwtClaimTypes.ClientId)).Select(c => c.Value).First();
            var schemes = await _permissionSchemeService.GetPermissionSchemes(clientId);
            var selectedScheme = schemes.FirstOrDefault(s => s.Id.Equals(permissionScheme));
            if (selectedScheme == null)
                return NotFound();
            var role = new Role(roleName);
            selectedScheme.Roles.Add(role);
            await _permissionSchemeService.SavePermissionScheme(selectedScheme);
            return Ok();
        } 
        
        
    }
}