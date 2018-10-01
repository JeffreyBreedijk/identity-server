using System;
using System.ComponentModel.DataAnnotations;

namespace Utilize.Identity.Provider.Models
{
    public class Licence
    {
        [Key] 
        public Guid Id { get; set; }
        public string Name { get; set; }


    }
}