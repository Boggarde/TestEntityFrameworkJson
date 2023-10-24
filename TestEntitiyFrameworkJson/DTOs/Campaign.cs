using System.ComponentModel.DataAnnotations;

namespace TestEntityFrameworkJson.DTOs
{
    public class CampaignDTO : IValidatableObject
    {
        public Guid? Id { get; set; }

        [StringLength(16, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        public string Sender { get; set; }

        [MinLength(1, ErrorMessage = "{0} must have at least {1} character(s).")]
        public string Template { get; set; }

        public bool Unicode { get; set; }

        [Required]
        public Guid DistributionListId { get; set; }

        public bool Flash { get; set; }

        public string ScheduleTimeUTC { get; set; }

        public string ScheduleTimeZone { get; set; }

        public bool Run { get; set; }

        [Required]
        public int SmsTrafficAccountId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(ScheduleTimeUTC) && string.IsNullOrEmpty(ScheduleTimeZone))
                yield return new ValidationResult("The schedule time zone is required.", new[] { nameof(ScheduleTimeZone) });
        }
    }

    public class CampaignStatusChangeDTO
    {
        public Guid Id { get; set; }
        public int CampaignStatus { get; set; }
    }
}
