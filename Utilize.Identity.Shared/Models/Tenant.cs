using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Utilize.Identity.Shared.DTO;

namespace Utilize.Identity.Shared.Models
{
    public class Tenant
    {
        [Key] public string Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public List<PermissionScheme> PermissionSchemes{ get; set; }

        public TenantDto ToDto()
        {
            return new TenantDto()
            {
                Active = Active,
                Name = Name
            };
        }

        public void ApplyDto(TenantDto dto)
        {
            Name = dto.Name;
            Active = dto.Active;
        }
    }
}