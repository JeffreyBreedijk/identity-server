using System;
using System.ComponentModel.DataAnnotations;

namespace UtilizeJwtProvider.Models
{
    public class Licence
    {
        [Key] public Guid Id { get; set; }
        public string Name { get; set; }

        public Licence()
        {
            Id = Guid.NewGuid();
        }
    }
}