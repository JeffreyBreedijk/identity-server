using System.Linq;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Utilize.Identity.Provider.DataSources;
using Utilize.Identity.Provider.Repository;
using Utilize.Identity.Provider.Services;

namespace Utilize.Identity.Provider.Controllers
{
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IClientStore _clientStore;
        private readonly AuthDbContext _authDbContext;

        public UsersController(IUserService userService, IClientStore clientStore, AuthDbContext authDbContext)
        {
            _userService = userService;
            _clientStore = clientStore;
            _authDbContext = authDbContext;
        }

        [HttpPost]
        public ActionResult CreateUser([FromForm] string clientId, [FromForm] string username, [FromForm] string password)
        {

            if (_clientStore.FindClientByIdAsync(clientId).Result == null)
                return NotFound("Client not found");
            if (_userService.GetUser(clientId, username).Result != null)
                return UnprocessableEntity();
            // todo: password strengt check otherwiser return UnprocessableEntity() --HTTP 422
            
           _userService.CreateUser(clientId, username, password);
           return NoContent();
        }
        
        
        

    }
}