using System;
using System.Security.Cryptography;
using System.Text;
using UtilizeJwtProvider.Models;
using UtilizeJwtProvider.Repository;

namespace UtilizeJwtProvider.Services
{
    public interface IUserService
    {
        User GetUser(string tenantId, string loginCode);
        void CreateUser(Tenant tenant, string loginCode, string password);
    }

    public class UserService : IUserService
    {
        private readonly IPasswordService _passwordService;
        private readonly IUserRepository _userRepository;

        public UserService(IPasswordService passwordService, IUserRepository userRepository)
        {
            _passwordService = passwordService;
            _userRepository = userRepository;
        }

        public User GetUser(string tenantId, string loginCode)
        {
            return _userRepository.GetUser(tenantId, loginCode);
        }
        
        public void CreateUser(Tenant tenant, string loginCode, string password)
        {
            
            if (_userRepository.UserExists(tenant.Id, loginCode)) return;
            
            var salt = _passwordService.CreateSalt();
            var hash = _passwordService.GetHash(password, salt);
            var usr = new User()
            {
                Id = GetUserHash(tenant.Id, loginCode),
                Tenant = tenant,
                LoginCode = loginCode,
                Hash = hash,
                Salt = salt
            };
            _userRepository.CreateUser(usr);      
        } 
        
        private string GetUserHash(string tenantId, string loginCode)
        {
            var sha1 = SHA1.Create();
            var inputBytes = Encoding.ASCII.GetBytes(tenantId + loginCode);
            var hash = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (var t in hash)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }
      
    }
}