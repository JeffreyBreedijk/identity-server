using System;
using System.ComponentModel.DataAnnotations;

namespace Utilize.Identity.Manager.Models
{
    public class Licence
    {
        [Key] 
        public Guid Id { get; set; }
        public string Name { get; set; }


    }
}