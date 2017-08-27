using System;
using System.Collections.Generic;
using CQRSlite.Events;

namespace UtilizeJwtProvider.Domain.Event
{
    public class UserAddRoleEvent : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string Role { get; set; }

        public UserAddRoleEvent(Guid id, int version, DateTimeOffset timeStamp, string role)
        {
            Id = id;
            Version = version;
            TimeStamp = timeStamp;
            Role = role;
        }
    }
}