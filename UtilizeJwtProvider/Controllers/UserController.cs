using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
           _userService.CreateUser(user.TenantId, user.LoginCode, user.Password);
        }
        
        public class NewUser
        {
            public string TenantId { get; set; }
            public string LoginCode { get; set; }
            public string Password { get; set; }
        }
    }
}