namespace TestEntityFrameworkJson.Models.Core
{
    public interface ICoreEntity : ICoreEntity<long, Guid>
    {

    }

    public interface ICoreEntity<TID, TEID>
    {
        TID Id { get; set; }

        TEID EntityId { get; set; }

        DateTime? CreatedDate { get; set; }

        DateTime? ModifiedDate { get; set; }

        Guid? ModifiedBy { get; set; }

        string Name { get; set; }
    }
}
