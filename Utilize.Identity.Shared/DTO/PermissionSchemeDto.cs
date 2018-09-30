using System;

namespace Utilize.Identity.Shared.DTO
{
    public class PermissionSchemeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}