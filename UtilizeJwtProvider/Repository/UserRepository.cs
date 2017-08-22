using System.Collections.Generic;
using System.Linq;
using UtilizeJwtProvider.Domain.Aggregates;

namespace UtilizeJwtProvider.Repository
{
    public interface IUserRepository
    {
        User FindUserByEmail(string email);
        bool UserExists(string email);
        void AddUserToCache(User user);
    }

    public class InMemoryUserRepository : IUserRepository
    {
        private readonly HashSet<User> _users;

        public InMemoryUserRepository(EventDbContext dbContext, IEventRepository eventRepository)
        {
            _users = new HashSet<User>(dbContext.Events
                    .Where(e => e.AggregateType.Equals("User"))
                    .Select(e => e.AggregateId)
                    .Distinct()
                    .Select(eventRepository.GetById<User>));
        }

        public User FindUserByEmail(string email) => _users.FirstOrDefault(u => u.Email.Equals(email));

        public bool UserExists(string email) => FindUserByEmail(email) == null;

        public void AddUserToCache(User user) => _users.Add(user);
    }
}