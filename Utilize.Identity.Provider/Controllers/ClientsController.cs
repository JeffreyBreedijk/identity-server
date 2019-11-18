using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Utilize.Identity.Provider.Models;
using Utilize.Identity.Provider.Repository.Clients;

namespace Utilize.Identity.Provider.Controllers
{
    [Route("[controller]")]
    public class ClientsController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IValidator<SimpleClient> _clientValidator;
        private readonly IClientWriteStore _writeStore;

        public ClientsController(IClientStore clientStore, IClientWriteStore writeStore,
            IValidator<SimpleClient> clientValidator)
        {
            _clientStore = clientStore;
            _writeStore = writeStore;
            _clientValidator = clientValidator;
        }

        [HttpPost]
        [Route("{clientId}")]
        public async Task<ActionResult> AddApiClient([FromRoute] string clientId, [FromBody] SimpleClient client)
        {
            var res = _clientValidator.Validate(client);
            if (!res.IsValid)
                return BadRequest(res.Errors.Select(x => x.ErrorMessage).ToList());
            var defaultSecret = Guid.NewGuid().ToString("N");
            var isClient = client.ToIdentityServer4Client();
            isClient.ClientSecrets = new List<Secret>()
            {
                new Secret()
                {
                    Description = "Default Secret",
                    Value = defaultSecret.Sha256(),
                    Type = "SharedSecret"
                }
            };
            await _writeStore.Add(isClient);
            return Ok(defaultSecret);
        }
    }
}