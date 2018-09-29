using System.Linq;
using Microsoft.EntityFrameworkCore;
using UtilizeJwtProvider.DataSources;
using UtilizeJwtProvider.Models;

namespace UtilizeJwtProvider.Repository
{
    public interface IUserRepository
    {
        User GetUser(string tenantId, string loginCode);
        bool UserExists(string tenantId, string loginCode);
        void CreateUser(User user);
    }
    
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _authDbContext;

        public UserRepository(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public User GetUser(string tenantId, string loginCode)
        {
            return _authDbContext.Users
                .Include(u => u.Tenant)
                .FirstOrDefault(u => u.LoginCode.Equals(loginCode) && u.Tenant.Id.Equals(tenantId));
        }

        public bool UserExists(string tenantId, string loginCode)
        {
            return _authDbContext.Users
                .Include(u => u.Tenant)
                .Any(u => u.LoginCode.Equals(loginCode) && u.Tenant.Id.Equals(tenantId));
        }

        public void CreateUser(User user)
        {
            _authDbContext.Add(user);
            _authDbContext.SaveChanges();
        }
    }
}