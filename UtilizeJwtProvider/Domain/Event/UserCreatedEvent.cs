using System;
using CQRSlite.Events;

namespace UtilizeJwtProvider.Domain.Event
{
    public class UserCreatedEvent : IEvent
    {
        public readonly string Email;
        public readonly string Hash;
        public readonly string Salt;
        
        public UserCreatedEvent(Guid id, int version, DateTimeOffset timestamp, string email, string hash, string salt)
        {
            Id = id;
            Version = version;
            TimeStamp = timestamp;
            Email = email;
            Hash = hash;
            Salt = salt;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}