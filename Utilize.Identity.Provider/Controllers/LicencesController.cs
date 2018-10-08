using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utilize.Identity.Provider.Models;
using Utilize.Identity.Provider.Services;

namespace Utilize.Identity.Provider.Controllers
{
    [Route("[controller]")]
    public class LicencesController : Controller
    {
        private readonly ILicenceService _licenceService;
        private readonly IPermissionService _permissionService;

        public LicencesController(ILicenceService licenceService, IPermissionService permissionService)
        {
            _licenceService = licenceService;
            _permissionService = permissionService;
        }

        [HttpPost]
        [Route("{licenceName}")]
        public async Task<ActionResult> CreateLicence([FromRoute] string licenceName)
        {
            await _licenceService.AddLicence(licenceName);
            return Ok();
        }
        
        [HttpGet]
        [Route("{licenceName}")]
        public async Task<ActionResult<Licence>> GetLicence([FromRoute] string licenceName)
        {
            return await _licenceService.GetLicence(licenceName);      
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Licence>>> GetAllLicences()
        {
            return await _licenceService.GetAllLicences();
        }
        
        [HttpPost]
        [Route("{licenceName}/permissions/{permission}")]
        public async Task<ActionResult> CreateLicence([FromRoute] string licenceName, [FromRoute] string permission)
        {
            var licence = await _licenceService.GetLicence(licenceName);
            if (licence == null)
                return NotFound();
            var perm = await _permissionService.GetPermission(permission);
            if (perm == null)
            {
                await _permissionService.AddPermission(permission, licence);
            }
            else
            {
                perm.Licence = licence;
                await _permissionService.SavePermission(perm);
            }
            return Ok();
        }
    }
}