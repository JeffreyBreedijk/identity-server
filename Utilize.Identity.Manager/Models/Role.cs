using System;
using System.ComponentModel.DataAnnotations;

namespace Utilize.Identity.Manager.Models
{
    public class Role
    {
        [Key] private Guid Id { get; }
        public string Name { get; set; }

        public Role()
        {
            Id = Guid.NewGuid();
        }
    }
}