using System;
using System.ComponentModel.DataAnnotations;

namespace Utilize.Identity.Provider.Models
{
    public class PermissionBinding
    {
        [Key] public Guid Id { get; set; }
        public PermissionScheme PermissionScheme { get; set; }
        public Shared.Models.Role Role { get; set; }
        public Shared.Models.Permission Permission { get; set; }

        public PermissionBinding()
        {
            Id = Guid.NewGuid();
        }
        
        
    }
}