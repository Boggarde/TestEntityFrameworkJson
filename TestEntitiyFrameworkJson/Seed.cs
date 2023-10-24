using Microsoft.EntityFrameworkCore;
using TestEntityFrameworkJson.Data;
using TestEntityFrameworkJson.Models;

namespace TestEntityFrameworkJson
{
    public static class Seed
    {
        public static void SeedDataContext(this IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var configuration = serviceScope.ServiceProvider.GetService<IConfiguration>();

            var context = serviceScope.ServiceProvider.GetService<DataContext>();

            context!.Database.Migrate();

            if (context!.Campaigns.Any()) return;

            var campaigns = new List<Campaign>
                {
                    new Campaign { Name = "Campaign 1", EntityId = Guid.NewGuid(), Template = "Test", Status = CampaignStatus.Ready, Unicode = false, Flash = false, Sender = "1111", SentNumber = 0, TotalCount = 0, SmsTrafficAccountId = 1, DistributionListId = 1, OwnerId = 1, AdditionalDataAsJson = new AdditionalData { BlockedEntries = 0, CustomerName = "Customer 1", TrafficAccountName = "TrafficAccount 1" } },
                    new Campaign { Name = "Campaign 2", EntityId = Guid.NewGuid(), Template = "Test two", Status = CampaignStatus.Ready, Unicode = false, Flash = false, Sender = "1212", SentNumber = 0, TotalCount = 0, SmsTrafficAccountId = 1, DistributionListId = 1, OwnerId = 101, AdditionalDataAsJson = new AdditionalData { BlockedEntries = 0, CustomerName = "Customer 2", TrafficAccountName = "TrafficAccount 1" } }
                };

            context.Campaigns.AddRange(campaigns);
            context.SaveChanges();
        }
    }
}
