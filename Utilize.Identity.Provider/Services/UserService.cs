using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Utilize.Identity.Provider.DataSources;
using Utilize.Identity.Provider.Models;
using Utilize.Identity.Provider.Repository;

namespace Utilize.Identity.Provider.Services
{
    public interface IUserService 
    {
        Task<User> GetUser(string tenantId, string loginCode);
        Task CreateUser(string clientId, string loginCode, string password);
    }

    public class UserService : IUserService
    {
        private readonly IPasswordService _passwordService;
        private readonly AuthDbContext _authDbContext;
        
        public UserService(IPasswordService passwordService, AuthDbContext authDbContext)
        {
            _passwordService = passwordService;
            _authDbContext = authDbContext;
        }

        public async Task<User> GetUser(string tenantId, string loginCode)
        {
            return await _authDbContext.Users.FirstOrDefaultAsync(u => u.LoginCode.Equals(loginCode) && u.Tenant.Equals(tenantId));
        }
        
        public async Task CreateUser(string clientId, string loginCode, string password)
        {
           
            var salt = _passwordService.CreateSalt();
            var hash = _passwordService.GetHash(password, salt);
            var usr = new User()
            {
                Id = GetUserHash(clientId, loginCode),
                Tenant = clientId,
                LoginCode = loginCode,
                Hash = hash,
                Salt = salt
            };
            await _authDbContext.Users.AddAsync(usr);
            await _authDbContext.SaveChangesAsync();  
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