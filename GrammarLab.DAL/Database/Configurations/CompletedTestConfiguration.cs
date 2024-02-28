using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GrammarLab.BLL.Entities;

namespace GrammarLab.DAL.Database.Configurations;

public class CompletedTestConfiguration : IEntityTypeConfiguration<CompletedTest>
{
    public void Configure(EntityTypeBuilder<CompletedTest> builder)
    {
        builder.Property(c => c.UserId).HasMaxLength(450);

        builder.HasOne(c => c.Topic)
            .WithMany()
            .HasForeignKey(c => c.TopicId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}