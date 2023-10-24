using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TestEntityFrameworkJson.DTOs;
using TestEntityFrameworkJson.Models;
using TestEntityFrameworkJson.Interfaces;

namespace TestEntityFrameworkJson.Business
{
    public interface ICampaignManager
    {
        Task<ICollection<Campaign>> GetAll();
        Task<CampaignStatusChangeDTO?> Save(CampaignDTO model);
    }

    public class CampaignManager : ICampaignManager
    {
        private ICampaignRepository _campaignRepository;

        public CampaignManager(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository;
        }

        public async Task<ICollection<Campaign>> GetAll()
        {
            var campaigns = await _campaignRepository.GetCampaigns();
            return campaigns;
        }

        public async Task<CampaignStatusChangeDTO?> Save(CampaignDTO model)
        {
            Campaign? campaign;

            if (model.Id is not null)
            { // Update campaign
                campaign = await _campaignRepository.GetById(model.Id.Value);

                if (campaign is null)
                {
                    //_logger.LogWarning("Trying to update non-existing/non-accessible campaign with (id = {CampaignEntityId}) with UserId = {AppUserId}.", model.Id, _context.Context.User.ApplicationUserId);
                    return null;
                }

                if (campaign.Status != CampaignStatus.Ready)
                {
                    //_logger.LogWarning("Campaign with (id = {CampaignEntityId}) with UserId = {AppUserId} is in a state that doesn't allow updating.", model.Id, _context.Context.User.ApplicationUserId);
                    return null;//Errors.Validation("Campaign is in invalid state!");
                }


            }
            else
            { // Insert campaign
                campaign = new Campaign
                {
                    Id = 0,
                    EntityId = Guid.NewGuid(),
                    Status = CampaignStatus.Ready
                };
            }

            campaign.Name = model.Name;
            campaign.Sender = model.Sender;
            campaign.Template = model.Template;
            campaign.SmsTrafficAccountId = model.SmsTrafficAccountId;
            campaign.DistributionListId = 1;
            campaign.TotalCount = 750;

            var additionalData = new Dictionary<string, object>
            {
                { "BlockedEntries", 0 },
                { "CustomerName", "AcmeLtd" },
                { "TrafficAccountName", $"TrafficAccountName - {campaign.SmsTrafficAccountId}" }
            };

            campaign.AdditionalData = JsonSerializer.Serialize(additionalData);

            var jsonAdditionalData = new AdditionalData()
            {
                BlockedEntries = 0,
                CustomerName = "AcmeLtd 2",
                TrafficAccountName = $"TrafficAccountName - {campaign.SmsTrafficAccountId}"
            };

            campaign.AdditionalDataAsJson = jsonAdditionalData;

            if (!string.IsNullOrEmpty(model.ScheduleTimeUTC) && DateTime.TryParse(model.ScheduleTimeUTC, out var scheduledDate))
            {
                campaign.ScheduleDate = scheduledDate.ToUniversalTime();
                campaign.ScheduleTimeZone = model.ScheduleTimeZone;
            }
            else
                campaign.ScheduleDate = null;

            try
            {
                var saveCampaign = await _campaignRepository.Update(campaign);

                if (saveCampaign == 0)
                {
                    
                    return null;//Failure<CampaignStatusChangeDTO>("Campaign could not be saved.");
                }

                if (model.Run && campaign.Status == CampaignStatus.Ready)
                {
                    //var scheduleResult = await Schedule(campaign.EntityId);

                    //if (!scheduleResult.IsSuccess)
                    //    return Failure<CampaignStatusChangeDTO>(scheduleResult.Error.Select(e => Errors.NotAcceptable(e.Message)).ToArray());

                    return new CampaignStatusChangeDTO
                    {
                        Id = model.Id.Value, //scheduleResult.Value.Id,
                        CampaignStatus = (int)CampaignStatus.Scheduled //scheduleResult.Value.CampaignStatus
                    };
                }

                return new CampaignStatusChangeDTO
                {
                    Id = campaign.EntityId,
                    CampaignStatus = (int)campaign.Status
                };
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //_logger.LogError(ex, "Cannot update campaign id = {CampaignEntityId} due to concurrency conflict.", model.Id);
                return null;// Failure<CampaignStatusChangeDTO>(Errors.Conflict("Concurrency conflict."));
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Cannot save campaign id = {CampaignEntityId}.", model.Id);
                return null; //Failure<CampaignStatusChangeDTO>("Campaign could not be saved.");
            }
        }
    }
}
