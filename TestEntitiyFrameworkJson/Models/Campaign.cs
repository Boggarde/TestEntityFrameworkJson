using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;
using TestEntityFrameworkJson.Models.Core;

namespace TestEntityFrameworkJson.Models
{
    public class Campaign : OwnershipEntity
    {
        public string Template { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string? ScheduleTimeZone { get; set; }
        public CampaignStatus Status { get; set; }
        public bool Unicode { get; set; }
        public bool Flash { get; set; }
        public string Sender { get; set; }
        public long SentNumber { get; set; }
        public long TotalCount { get; set; }
        public int SmsTrafficAccountId { get; set; }
        public long DistributionListId { get; set; }
        public byte[] Timestamp { get; protected set; }

        public string? Report { get; set; }

        //public ICollection<CampaignHistory> History { get; set; }

        //[JsonIgnore]
        //public DistributionList DistributionList { get; set; }

        /// <summary>Data will be stored as JSON string (e.g. {"BlockedEntries":3,"CustomerName":"Acme Ltd","TrafficAccountName":"101_campaign_2"})</summary>
        public string? AdditionalData { get; set; }

        public AdditionalData? AdditionalDataAsJson { get; set; }

        public void AdjustCampaignName()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = ScheduleDate != null
                    ? $"{Sender} {ScheduleDate.Value.ToShortDateString()}"
                    : Sender;
            }
        }
    }

    public enum CampaignStatus
    {
        Draft = 0,
        Ready,
        Scheduled,
        Running,
        Paused,
        Canceled,
        Faulted,
        Completed,
        Archived
    }

    public class CampaignHistory : OwnershipEntity
    {
        public long CampaignId { get; set; }

        public CampaignStatus Status { get; set; }
    }

    public class AdditionalData
    {
        public string CustomerName { get; set; }
        public string TrafficAccountName { get; set; }
        /// <summary>Number of entries/contacts that have been blocked.</summary>
        public long BlockedEntries { get; set; }
    }
}
