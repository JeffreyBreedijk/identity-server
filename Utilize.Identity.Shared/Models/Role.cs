using System;
using System.ComponentModel.DataAnnotations;

namespace Utilize.Identity.Shared.Models
{
    public class Role
    {
        [Key] public Guid Id { get; set; }
        public string Name { get; set; }
        public PermissionScheme permissionScheme { get; set; }

        public Role()
        {
            Id = Guid.NewGuid();
        }
        
    }
}