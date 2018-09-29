using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using UtilizeJwtProvider.DTO;
using UtilizeJwtProvider.Helpers;

namespace UtilizeJwtProvider.Models
{
    public class PermissionScheme
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Tenant Tenant { get; set; }
        public bool IsActive { get; set; }

        public PermissionSchemeDto ToDto()
        {
            return new PermissionSchemeDto()
            {
                Id = Id,
                IsActive = IsActive,
                Name = Name
            };
        }
        
        public void ApplyDto(PermissionSchemeDto dto)
        {
            Name = dto.Name;
            IsActive = dto.IsActive;
        }
    }
}