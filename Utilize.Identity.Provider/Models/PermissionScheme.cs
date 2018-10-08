using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Utilize.Identity.Provider.Helpers;

namespace Utilize.Identity.Provider.Models
{
    [Table(name:"permissionschemes")]
    public class PermissionScheme
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Client { get; set; }
        public bool IsActive { get; set; }
        public HashSet<Role> Roles { get; set; }

        public PermissionScheme()
        {
            
        }
        
        public PermissionScheme(string clientId, string name)
        {
            Id = Hasher.GetHash(name + clientId);
            Name = name;
            Client = clientId;
            IsActive = false;
        }
    }
    
    
}