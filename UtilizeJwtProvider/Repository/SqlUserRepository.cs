using System.Linq;
using UtilizeJwtProvider.DataSources;
using UtilizeJwtProvider.Models;

namespace UtilizeJwtProvider.Repository
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly UserDbContext _userDbContext;

        public SqlUserRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
            _userDbContext.Database.EnsureCreated();
        }

        public User GetUser(string tenantId, string loginCode)
        {
            return _userDbContext.Users
                .FirstOrDefault(u => u.LoginCode.Equals(loginCode) && u.TenantId.Equals(tenantId));
        }

        public bool UserExists(string tenantId, string loginCode)
        {
            return _userDbContext.Users
                .Any(u => u.LoginCode.Equals(loginCode) && u.TenantId.Equals(tenantId));
        }

        public void CreateUser(User user)
        {
            _userDbContext.Add(user);
            _userDbContext.SaveChanges();
        }
    }
}