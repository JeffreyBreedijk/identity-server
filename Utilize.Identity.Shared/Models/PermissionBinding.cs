using System;
using System.ComponentModel.DataAnnotations;

namespace Utilize.Identity.Shared.Models
{
    public class PermissionBinding
    {
        [Key] public Guid Id { get; set; }
        public PermissionScheme PermissionScheme { get; set; }
        public Role Role { get; set; }
        public Permission Permission { get; set; }

        public PermissionBinding()
        {
            Id = Guid.NewGuid();
        }
        
        
    }
}