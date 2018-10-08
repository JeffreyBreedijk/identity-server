﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilize.Identity.Provider.Models
{
    [Table(name:"licences")]
    public class Licence
    {
        [Key] 
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Permission> Permissions { get; set; }

    }
}