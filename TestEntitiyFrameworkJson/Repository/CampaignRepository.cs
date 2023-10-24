using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TestEntityFrameworkJson.Data;
using TestEntityFrameworkJson.Interfaces;
using TestEntityFrameworkJson.Models;
using TestEntityFrameworkJson.Models.Core;

namespace TestEntityFrameworkJson.Repository
{
    public class CampaignRepository : ICampaignRepository
    {
        private DataContext _dataContext;

        public CampaignRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Campaign?> GetById(Guid id)
        {
            //Using AsNoTracking() to check weather AdditionalDataAsJson will be updated
            var query = _dataContext.Campaigns.Where(campaign => campaign.EntityId == id).AsNoTracking();
            var campaign = await query.FirstOrDefaultAsync();

            return campaign;
        }

        public async Task<ICollection<Campaign>> GetCampaigns() => await _dataContext.Campaigns.OrderBy(c => c.Id).ToListAsync();

        public async Task<int> Update(Campaign campaign, bool saveChanges = true)
        {
            campaign.ModifiedDate = DateTime.UtcNow;
            
            _dataContext.Set<Campaign>().Attach(campaign);
            _dataContext.Entry(campaign).State = EntityState.Modified;
            

            if (saveChanges)
                return await Save();

            return 0;

        }

        private async Task<int> Save()
        {
            _dataContext.ChangeTracker
               .Entries()
               .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added)
               .Select(x => x.Entity).OfType<OwnershipEntity>()
               .ToList().ForEach(x =>
               {
                   x.OwnerId = 101;
               });

            return await _dataContext.SaveChangesAsync();
        }
    }
}
