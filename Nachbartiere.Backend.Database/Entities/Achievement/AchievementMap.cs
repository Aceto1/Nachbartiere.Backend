using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nachbartiere.Backend.Database.Entities.Maps
{
    internal class AchievementMap : IEntityTypeConfiguration<Achievement>
    {
        public void Configure(EntityTypeBuilder<Achievement> builder)
        {
            builder.ToTable("Achievements");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).HasColumnName("Id");
            builder.Property(m => m.AchievementKind).HasColumnName("AchievementKind").HasConversion(
                v => (int)v,
                v => (Enum.Achievement)v);
            builder.Property(m => m.UnlockedAt).HasColumnName("UnlockedAt");
            builder.Property(m => m.UserId).HasColumnName("UserId");
        }
    }
}
