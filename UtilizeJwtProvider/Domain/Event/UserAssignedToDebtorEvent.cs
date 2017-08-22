using System;
using CQRSlite.Events;

namespace UtilizeJwtProvider.Domain.Event
{
    public class UserAssignedToDebtorEvent : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string DebtorId { get; set; }

        public UserAssignedToDebtorEvent(Guid id, int version, DateTimeOffset timestamp, string debtorId)
        {
            Id = id;
            Version = version;
            TimeStamp = timestamp;
            DebtorId = debtorId;
        }
    }
}