using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Utilize.Identity.Provider.DTO;

namespace Utilize.Identity.Provider.Models
{
    public class Tenant
    {
        [Key] public string Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public List<PermissionScheme> PermissionSchemes{ get; set; }
        public List<User> Users { get; set; }

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