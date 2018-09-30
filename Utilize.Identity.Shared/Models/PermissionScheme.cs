using System;
using Utilize.Identity.Shared.DTO;

namespace Utilize.Identity.Shared.Models
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