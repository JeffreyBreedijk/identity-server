using System;
using CQRSlite.Events;

namespace UtilizeJwtProvider.Domain.Event
{
    public class UserDataUpdatedEvent : IEvent
    {
        public readonly string Firstname;
        public readonly string Lastname;
        
        public UserDataUpdatedEvent(Guid id, int version, DateTimeOffset timestamp, string firstname, string lastname)
        {
            Id = id;
            Version = version;
            TimeStamp = timestamp;
            Firstname = firstname;
            Lastname = lastname;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}