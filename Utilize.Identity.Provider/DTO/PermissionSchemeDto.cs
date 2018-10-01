using System;

namespace Utilize.Identity.Provider.DTO
{
    public class PermissionSchemeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}