using System;
using System.ComponentModel.DataAnnotations.Schema;
using Utilize.Identity.Provider.DTO;

namespace Utilize.Identity.Provider.Models
{
    [Table(name:"permissionschemes")]
    public class PermissionScheme
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Tenant { get; set; }
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