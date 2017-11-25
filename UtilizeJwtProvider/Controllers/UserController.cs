using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UtilizeJwtProvider.Domain.Aggregates;
using UtilizeJwtProvider.Repository;
using UtilizeJwtProvider.Services;


namespace UtilizeJwtProvider.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //[Authorize(Roles = "ADMIN")]
        [HttpPut]
        public void CreateUser([FromBody] NewUser user)
        {
           _userService.CreateUser(user.LoginCode, user.Password);
        }
        
        [HttpGet]
        [Route("{loginCode}/roles")]
        public HashSet<string> GetRoles([FromRoute] string loginCode)
        {
            var user = _userService.GetUser(loginCode);
            
            if (user?.Roles == null)
            {
                return new HashSet<string>();
            }
            
            return user.Roles;

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