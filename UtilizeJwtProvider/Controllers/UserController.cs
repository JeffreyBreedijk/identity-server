using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
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

        public UserController(IEventRepository eventRepository, IUserRepository userRepository, IPasswordService passwordService)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _passwordService = passwordService;
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
            
//             var usr = _userCache.FindUserByEmail("e");
//            usr.UpdateFirstnameLastname("Test", "Test");
//            _eventRepository.Save(usr);
//            return _userCache.FindUserByEmail("e");
        }

        [HttpPost]
        public void Post([FromBody] NewUser user)
        {
            if (!_userRepository.UserExists(user.Email)) return;
            var salt = _passwordService.CreateSalt();
            var hash = _passwordService.GetHash(user.Password, salt);
            var usr = new User(Guid.NewGuid(), hash, salt, user.Email);
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