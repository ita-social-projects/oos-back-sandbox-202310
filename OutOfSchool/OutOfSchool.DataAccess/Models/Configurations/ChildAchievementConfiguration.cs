using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfSchool.Services.Models.Configurations;
internal class ChildAchievementConfiguration : IEntityTypeConfiguration<ChildAchievement>
{
    public void Configure(EntityTypeBuilder<ChildAchievement> builder) {
        builder.
            HasOne(t => t.ChildAchievementType)
            .WithOne(t => t.ChildAchievement)
            .HasForeignKey<ChildAchievementType>(t => t.ChildAchievementId);
    }
}
