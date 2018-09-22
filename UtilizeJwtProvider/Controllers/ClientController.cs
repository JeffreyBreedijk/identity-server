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
    public class ClientController  : ControllerBase
    {
        private readonly ConfigurationDbContext _configurationDbContext;

        public ClientController(ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }

        [HttpPost]
        [Route("{clientId}")]
        public void CreateClient([FromRoute] string clientId)
        {
            var client = new Client
            {
                ClientId = clientId,
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword
            };
            _configurationDbContext.Clients.Add(client.ToEntity());
            _configurationDbContext.SaveChanges();
        } 
        
        [HttpPost]
        [Route("{clientId}/scopes/{scopeId}")]
        public void AddScope([FromRoute] string clientId, [FromRoute] string scopeId)
        {
            var client = _configurationDbContext.Clients.AsNoTracking().FirstOrDefault(c => c.ClientId.Equals(clientId));
            var id = client.Id;
            var clientModel = client.ToModel();
            clientModel.AllowedScopes.Add(scopeId);
            client = clientModel.ToEntity();
            client.Id = id;
            _configurationDbContext.Clients.Add(client);
            _configurationDbContext.Entry(client).State = EntityState.Modified;
            _configurationDbContext.SaveChanges();
        } 
        
        [HttpGet]
        [Route("{clientId}")]
        public Client GetClient([FromRoute] string clientId)
        {
            return _configurationDbContext.Clients.FirstOrDefault(c => c.ClientId.Equals(clientId)).ToModel();
        }
        
    }
}