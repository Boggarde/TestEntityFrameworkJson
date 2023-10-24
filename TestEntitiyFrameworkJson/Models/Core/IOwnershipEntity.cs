namespace TestEntityFrameworkJson.Models.Core
{
    public interface IOwnershipEntity<TID, TEID> : ICoreEntity
    {
        long OwnerId { get; set; }
    }
}
