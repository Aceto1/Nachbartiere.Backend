using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nachbartiere.Backend.Database.Entities.Maps
{
    internal class UserToTestGroupMap : IEntityTypeConfiguration<UserToTestGroup>
    {
        public void Configure(EntityTypeBuilder<UserToTestGroup> builder)
        {
            builder.ToTable("UsersToTestGroups");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.UserId).HasColumnName("UserId");
            builder.Property(x => x.TestGroup).HasColumnName("TestGroup");
        }
    }
}
