using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace OutOfSchool.Services.Models.Configurations;
internal class MinistryConfiguration : IEntityTypeConfiguration<Ministry>
{
    public void Configure(EntityTypeBuilder<Ministry> builder)
    {
        builder
            .HasMany(x => x.MinistryAdmins)
            .WithOne(x => x.Ministry)
            .HasForeignKey(x => x.MinistryId)
            .HasPrincipalKey(x => x.Id);
    }
}
