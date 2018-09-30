﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Utilize.Identity.Shared.Models
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