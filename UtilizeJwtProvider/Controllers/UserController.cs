using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilizeJwtProvider.Services;


namespace UtilizeJwtProvider.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut]
        public void CreateUser([FromBody] NewUser user)
        {
           _userService.CreateUser(user.LoginCode, user.Password);
        }
        
        [HttpGet]
        [Route("{loginCode}/roles")]
        public HashSet<string> GetRoles([FromRoute] string loginCode)
        {   
            return _userService.GetUser(loginCode)?.Roles ?? new HashSet<string>();
        }
     
        [HttpPut]
        [Route("{loginCode}/roles/{role}")]
        public void AddRole([FromRoute] string loginCode, [FromRoute] string role )
        {
            _userService.SetUserRole(loginCode, role);
        }
        

        public class NewUser
        {
            public string LoginCode { get; set; }
            public string Password { get; set; }
        }
    }
}