using UtilizeJwtProvider.Models;

namespace UtilizeJwtProvider.Repository
{
    public interface IUserRepository
    {
        User GetUser(string tenantId, string loginCode);
        bool UserExists(string tenantId, string loginCode);
        void CreateUser(User user);
    }
}