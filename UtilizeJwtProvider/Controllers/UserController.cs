using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UtilizeJwtProvider.Services;


namespace UtilizeJwtProvider.Controllers
{
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITenantService _tenantService;

        public UserController(IUserService userService, ITenantService tenantService)
        {
            _userService = userService;
            _tenantService = tenantService;
            
        }

        [HttpPost]
        public ActionResult CreateUser([FromForm] string tenantId, [FromForm] string username, [FromForm] string password)
        {
            var tenant = _tenantService.GetTenant(tenantId);
            if (tenant == null)
                return NotFound();
            // todo: password strengt check otherwiser return UnprocessableEntity() --HTTP 422
            
           _userService.CreateUser(tenant, username, password);
            return NoContent();
        }

    }
}