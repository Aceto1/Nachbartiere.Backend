using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nachbartiere.Backend.Database.Entities.Maps
{
    internal class InviteTokenMap : IEntityTypeConfiguration<InviteToken>
    {
        public void Configure(EntityTypeBuilder<InviteToken> builder)
        {
            builder.ToTable("InviteTokens");

            builder.HasKey(x => x.Id);

            builder.Property(m => m.Id).HasColumnName("Id");
            builder.Property(m => m.Token).HasColumnName("Token").HasMaxLength(6);
        }
    }
}
