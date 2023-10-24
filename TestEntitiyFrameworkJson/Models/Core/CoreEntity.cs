using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using TimeProvider = TestEntityFrameworkJson.Common.TimeProvider;

namespace TestEntityFrameworkJson.Models.Core
{
    public abstract class CoreEntity<TID, TEID> : ICoreEntity<TID, TEID>
    {
        /// <summary>
        ///     Get or Set Primary Id
        ///     All operational tables have one numeric column Id as Primary Key
        /// </summary>
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public TID Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TEID EntityId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
        [JsonIgnore]
        public Guid? ModifiedBy { get; set; }

        public string Name { get; set; }

        public CoreEntity()
        {
            this.ModifiedDate = TimeProvider.GetCurrentUniversalTime();
            this.CreatedDate = TimeProvider.GetCurrentUniversalTime();
        }

    }

    public abstract class CoreEntity : CoreEntity<long, Guid>
    {

    }
}
