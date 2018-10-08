using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Utilize.Identity.Provider.DataSources;
using Utilize.Identity.Provider.Models;

namespace Utilize.Identity.Provider.Services
{
    public interface ILicenceService
    {
        Task AddLicence(string name);
        Task<Licence> GetLicence(string name);
        Task<List<Licence>> GetAllLicences();
    }

    public class LicenceService : ILicenceService
    {
        private readonly AuthDbContext _dbContext;

        public LicenceService(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddLicence(string name)
        {
            await _dbContext.Licences.AddAsync(new Licence()
            {
                Id = Guid.NewGuid(),
                Name = name
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Licence> GetLicence(string name)
        {
           return await _dbContext.Licences.FirstOrDefaultAsync(l => l.Name.Equals(name));
        }

        public async Task<List<Licence>> GetAllLicences()
        {
            return await _dbContext.Licences.ToListAsync();
        }
    }
}