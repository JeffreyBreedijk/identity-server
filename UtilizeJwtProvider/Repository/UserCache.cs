using System.Collections.Generic;
using System.Linq;
using UtilizeJwtProvider.Domain.Aggregates;

namespace UtilizeJwtProvider.Repository
{
    public interface IUserCache
    {
        User FindUserByEmail(string email);
    }

    public class UserCache : IUserCache
    {
        private readonly HashSet<User> _users;
        
        public UserCache(IEventRepository eventRepository)
        {
            using (var context = new ApplicationDbContext())
            {
                _users = new HashSet<User>(context.Events
                    .Where(e => e.AggregateType.Equals("User"))
                    .Select(e => e.AggregateId)
                    .Distinct()
                    .Select(eventRepository.GetById<User>));
                    
            }
        }

        public User FindUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email.Equals(email));
        }

       
    }
}