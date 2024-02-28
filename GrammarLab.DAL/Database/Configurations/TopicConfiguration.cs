using GrammarLab.BLL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GrammarLab.DAL.Database.Configurations;

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.Property(e => e.Name).HasMaxLength(256);

        builder.HasOne(t => t.Level)
            .WithMany(l => l.Topics)
            .HasForeignKey(t => t.LevelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}