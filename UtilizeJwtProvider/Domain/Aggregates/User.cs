using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CQRSlite.Domain;
using CQRSlite.Events;
using UtilizeJwtProvider.Domain.Event;


namespace UtilizeJwtProvider.Domain.Aggregates
{
    public sealed class User : AggregateRoot
    {
        [IgnoreDataMember]
        public string Hash { get; private set; }
        
        [IgnoreDataMember]
        public string Salt { get; private set; }
        
        public string Firstname { get; private set;}
        
        public string Lastname { get; private set; }
        
        public string Email { get; private set;}
        
        public bool IsActive { get;  private set;}
        
        public bool IsDeleted { get;  private set;}
        
        public string DebtorId { get; private set;}

        public User() {}

        public User(Guid id, string hash, string salt, string email)
        {
            ApplyChange(new UserCreatedEvent(id, Version, DateTimeOffset.UtcNow, email, hash, salt));
        }

        public void UpdatePassword(string hash, string salt)
        {
            if (Hash.Equals(hash)) return;
            ApplyChange(new PasswordUpdatedEvent(Id, Version, DateTimeOffset.UtcNow, hash, salt));
        }

        public void UpdateFirstnameLastname(string firstname, string lastname)
        {
            ApplyChange(new UserDataUpdatedEvent(Id, Version, DateTimeOffset.UtcNow, firstname, lastname));
        }

        public void UpdateDebtor(string debtorId)
        {
            ApplyChange(new UserAssignedToDebtorEvent(Id, Version, DateTimeOffset.Now, debtorId));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) 
                return false;
            var other = (User) obj;
            return Email.Equals(other.Email);
        }

        public override int GetHashCode()
        {
            return Email.GetHashCode();
        }

        private void Apply(UserCreatedEvent e)
        {
            Id = e.Id;
            Email = e.Email;
            Hash = e.Hash;
            Salt = e.Salt;
            IsActive = true;
            IsDeleted = false;
        }

        private void Apply(PasswordUpdatedEvent e)
        {
            Hash = e.Hash;
            Salt = e.Salt;
        }

        private void Apply(UserDataUpdatedEvent e)
        {
            Firstname = e.Firstname;
            Lastname = e.Lastname;
        }

        private void Apply(UserAssignedToDebtorEvent e)
        {
            DebtorId = e.DebtorId;
        }
        
    }
}