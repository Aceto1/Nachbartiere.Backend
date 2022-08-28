using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nachbartiere.Backend.Database.Entities.Maps
{
    internal class SightingMap : IEntityTypeConfiguration<Sighting>
    {
        public void Configure(EntityTypeBuilder<Sighting> builder)
        {
            builder.ToTable("Sightings");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).HasColumnName("Id");
            builder.Property(m => m.CreatedAt).HasColumnName("CreatedAt");
            builder.Property(m => m.CreatedBy).HasColumnName("CreatedBy");
            builder.Property(m => m.Count).HasColumnName("Count");
            builder.Property(m => m.AnimalKind).HasColumnName("AnimalKind").HasConversion(
                v => (int)v,
                v => (Enum.AnimalKind)v); ;
            builder.Property(m => m.Description).HasColumnName("Description");
            builder.Property(m => m.Location).HasColumnName("Location");
        }
    }
}
