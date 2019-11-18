using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Utilize.Identity.Provider.Repository;

namespace Utilize.Identity.Provider.Controllers
{
    [Route("[controller]")]
    public class ResourceController : Controller
    {
        private readonly IResourceStore _resourceStore;
        private readonly IResourceWriteStore _writeStore;

        public ResourceController(IResourceStore resourceStore, IResourceWriteStore writeStore)
        {
            _resourceStore = resourceStore;
            _writeStore = writeStore;
        }

        [HttpPost]
        [Route("{resourceId}")]
        public async Task<ActionResult> AddApiResource([FromRoute] string resourceId)
        {
            var resource1 = new ApiResource
            {
                Name = "A",
                DisplayName = "Custom API",
                Description = "Custom API Access",
                ApiSecrets = new List<Secret> {new Secret("scopeSecret".Sha256())},
                Scopes = new List<Scope>
                {
                    new Scope("product.write")
                }
            };
            await _writeStore.AddApiResource(resource1);

            var resource2 = new ApiResource
            {
                Name = "B",
                DisplayName = "Custom API",
                Description = "Custom API Access",
                ApiSecrets = new List<Secret> {new Secret("scopeSecret".Sha256())},
                Scopes = new List<Scope>
                {
                    new Scope("price.write")
                }
            };
            await _writeStore.AddApiResource(resource2);
            return Ok();
        }
    }
}