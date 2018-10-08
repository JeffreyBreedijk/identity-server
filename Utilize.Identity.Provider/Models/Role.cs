using System;
using System.Collections.Generic;
using Utilize.Identity.Provider.Helpers;

namespace Utilize.Identity.Provider.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Permission> Permissions { get; set; }

        public Role()
        {
            
        }

        public Role(string name) 
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Role item))
            {
                return false;
            }

            return Name.Equals(item.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}