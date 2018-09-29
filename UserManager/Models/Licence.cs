using System;
using System.ComponentModel.DataAnnotations;

namespace UserManager.Models
{
    public class Licence
    {
        [Key] 
        public Guid Id { get; set; }
        public string Name { get; set; }


    }
}