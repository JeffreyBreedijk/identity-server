using System.Linq;
using System.Threading.Tasks;
using UtilizeJwtProvider.Models;

namespace UtilizeJwtProvider.Repository
{
    public interface IUserRepository
    {
        User GetUser(string username);
        void CreateUser(User user);
        Task<User> FindAsync(string username);
    }
    
    public class UserRepository : IUserRepository
    {
        public User GetUser(string username)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Users.First(x => x.UserId.Equals(username));
            }
        }

        public void CreateUser(User user)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public async Task<User> FindAsync(string username)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Users.First(x => x.UserId.Equals(username));
            }
        }
    }
}