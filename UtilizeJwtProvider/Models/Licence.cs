using System;
using System.ComponentModel.DataAnnotations;

namespace UtilizeJwtProvider.Models
{
    public class Licence
    {
        [Key] private Guid Id { get; }
        public string Name { get; set; }

        public Licence()
        {
            Id = Guid.NewGuid();
        }
    }
}