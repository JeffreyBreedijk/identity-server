using System;
using CQRSlite.Events;

namespace UtilizeJwtProvider.Domain.Event
{
    public class PasswordUpdatedEvent : IEvent
    {
        public readonly string Hash;
        public readonly string Salt;
        
        public PasswordUpdatedEvent(Guid id, int version, DateTimeOffset timestamp, string hash, string salt)
        {
            Id = id;
            Version = version;
            TimeStamp = timestamp;
            Hash = hash;
            Salt = salt;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}