using System;
using UtilizeJwtProvider.Domain.Aggregates;
using UtilizeJwtProvider.Repository;

namespace UtilizeJwtProvider.Services
{
    public interface IUserService
    {
        User GetUser(string loginCode);
        void CreateUser(string loginCode, string password);
        void SetUserRole(string loginCode, string role);
    }

    public class UserService : IUserService
    {
        private readonly IUserCache _userCache;
        private readonly IPasswordService _passwordService;
        private readonly IEventRepository _eventRepository;

        public UserService(IUserCache userCache, IPasswordService passwordService, IEventRepository eventRepository)
        {
            _userCache = userCache;
            _passwordService = passwordService;
            _eventRepository = eventRepository;
        }
        
        public User GetUser(string loginCode)
        {
            return _userCache.FindUserByLoginCode(loginCode);
        }
        
        public void CreateUser(string loginCode, string password)
        {
            if (_userCache.UserExists(loginCode)) return;
            var salt = _passwordService.CreateSalt();
            var hash = _passwordService.GetHash(password, salt);
            var usr = new User(Guid.NewGuid(), hash, salt, loginCode);
            _eventRepository.Save(usr);
            _userCache.AddUserToCache(usr);        
        } 
        
        public void SetUserRole(string loginCode, string role)
        {
            var user = _userCache.FindUserByLoginCode(loginCode);
            user.AddRole(role);
            _eventRepository.Save(user);
        }
    }
}