using Microsoft.EntityFrameworkCore;
using TestEntityFrameworkJson.Models;

namespace TestEntityFrameworkJson.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
                
        }

        public DbSet<Campaign> Campaigns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Campaign>(entityBuilder =>
            {
                entityBuilder.Property(x => x.Status).IsRequired();
                entityBuilder.Property(x => x.Timestamp).IsRowVersion();
                entityBuilder.HasIndex(x => x.EntityId).IsUnique();              
                entityBuilder.OwnsOne(c => c.AdditionalDataAsJson).ToJson();
            });
        }
    }
}
