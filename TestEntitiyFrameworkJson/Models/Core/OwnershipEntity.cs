using System.Text.Json.Serialization;

namespace TestEntityFrameworkJson.Models.Core
{
    public abstract class OwnershipEntity : CoreEntity, IOwnershipEntity<long, Guid>
    {
        [JsonIgnore]
        public long OwnerId { get; set; }
    }
}
