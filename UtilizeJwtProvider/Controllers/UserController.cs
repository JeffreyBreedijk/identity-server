using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using UtilizeJwtProvider.Models;
using UtilizeJwtProvider.Repository;

namespace UtilizeJwtProvider.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        [HttpGet("{userId}")]
        public User Get(string userId)
        {
            return _userRepository.GetUser(userId);
        }
        
        [HttpPost]
        public void Post([FromBody] User user)
        {
            _userRepository.CreateUser(user);
        }
    }
}