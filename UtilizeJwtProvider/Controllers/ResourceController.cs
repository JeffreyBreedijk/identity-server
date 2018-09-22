using System.Linq;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UtilizeJwtProvider.Controllers
{
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly ConfigurationDbContext _configurationDbContext;

        public ResourceController(ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }

        [HttpPost]
        [Route("{resourceId}")]
        public void CreateApiResource([FromRoute] string resourceId)
        {
            var resource = new ApiResource
            {
                Name = resourceId

            };
            _configurationDbContext.ApiResources.Add(resource.ToEntity());
            _configurationDbContext.SaveChanges();
        }

        [HttpGet]
        [Route("{resourceId}/scopes")]
        public ApiResource GetApiScopes([FromRoute] string resourceId)
        {
            return _configurationDbContext.ApiResources.FirstOrDefault(r => r.Name.Equals(resourceId)).ToModel();
        }
        
        [HttpPost]
        [Route("{resourceId}/scopes/{scopeId}")]
        public void InsertScope([FromRoute] string resourceId, [FromRoute] string scopeId)
        {
            var resource = _configurationDbContext.ApiResources.AsNoTracking().FirstOrDefault(r => r.Name.Equals(resourceId));
            var id = resource.Id;
            var resourceModel = resource.ToModel();
            var scope = new Scope()
            {
                Name = scopeId,
                DisplayName = scopeId,
                UserClaims =
                {
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Email,
                    JwtClaimTypes.FamilyName,
                    JwtClaimTypes.GivenName,
                    JwtClaimTypes.Role,
                    "debtor_id"
                }
            };
            resourceModel.Scopes.Add(scope);
            resource = resourceModel.ToEntity();
            resource.Id = id;
            _configurationDbContext.ApiResources.Add(resource);
            _configurationDbContext.Entry(resource).State = EntityState.Modified;
            _configurationDbContext.SaveChanges();
        } 
    }
}