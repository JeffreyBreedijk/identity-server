﻿using System;
using System.ComponentModel.DataAnnotations;

namespace UtilizeJwtProvider.Models
{
    public class Permission
    {
        [Key] public Guid Id { get; set; }
        public string Name { get; set; }
        public Tenant Tenant { get; set; }

        public Permission()
        {
            Id = Guid.NewGuid();
        }
    }
}