using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OutOfSchool.Services.Models.Configurations;
internal class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
{
    public void Configure(EntityTypeBuilder<Favourite> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
