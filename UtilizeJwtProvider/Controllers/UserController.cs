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
        private readonly IUserCache _userCache;

        public UserController(IEventRepository eventRepository, IUserCache userCache)
        {
            _eventRepository = eventRepository;
            _userCache = userCache;
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

//        [HttpPost]
//        public void Post([FromBody] User user)
//        {
//            _userRepository.CreateUser(user);
//        }
    }
}