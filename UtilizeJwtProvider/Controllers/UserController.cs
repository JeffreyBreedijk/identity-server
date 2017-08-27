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
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ILogger _logger;

        public UserController(ILoggerFactory _loggerFactory, IEventRepository eventRepository, IUserRepository userRepository, IPasswordService passwordService)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _passwordService = passwordService;
            _logger = _loggerFactory.CreateLogger("UserController");
        }


        [HttpGet]
        public User Get()
        {
            var usr = new User(Guid.NewGuid(), "h", "s", "e");
            _eventRepository.Save(usr);

            usr.UpdatePassword("Pwd2", "Slt2");
            _eventRepository.Save(usr);
            
            usr.UpdateFirstnameLastname("Jeffrey", "Breedijk");
            _eventRepository.Save(usr);
            return null;
            

        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public void Post([FromBody] NewUser user)
        {
            var name = "testname";
            _logger.LogInformation("Adding user {name}", name);
            if (!_userRepository.UserExists(user.Email)) return;
            var salt = _passwordService.CreateSalt();
            var hash = _passwordService.GetHash(user.Password, salt);
            var usr = new User(Guid.NewGuid(), hash, salt, user.Email);
            usr.AddRole("admin");
            _eventRepository.Save(usr);
            _userRepository.AddUserToCache(usr);            
        }

        public class NewUser
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}