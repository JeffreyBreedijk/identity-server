using System;

namespace Utilize.Identity.Provider.Models
{
    public class Permission
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Licence Licence { get; set; }
    }
}