using TestEntityFrameworkJson.Models;

namespace TestEntityFrameworkJson.Interfaces
{
    public interface ICampaignRepository
    {
        Task<ICollection<Campaign>> GetCampaigns();
        Task<Campaign?> GetById(Guid entityId);
        Task<int> Update(Campaign campaign, bool saveChanges = true);
    }
}
