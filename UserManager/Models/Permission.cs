using System;
using System.ComponentModel.DataAnnotations;

namespace UserManager.Models
{
    public class Permission
    {
        [Key] private Guid Id { get;  }
        public Licence Licence { get; set; }
        public Role Role { get; set; }

        public Permission()
        {
            Id = Guid.NewGuid();
        }
    }
}