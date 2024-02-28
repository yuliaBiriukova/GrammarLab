using GrammarLab.BLL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GrammarLab.DAL.Database.Configurations;

public class CompletedTestExerciseConfiguration : IEntityTypeConfiguration<CompletedTestExercise>
{
    public void Configure(EntityTypeBuilder<CompletedTestExercise> builder)
    {
        builder.Property(c => c.Task).HasMaxLength(256);
        builder.Property(c => c.Answer).HasMaxLength(256);
        builder.Property(c => c.UserAnswer).HasMaxLength(256);

        builder.HasOne(c => c.CompletedTest)
            .WithMany(c => c.CompletedTestExercises)
            .HasForeignKey(c => c.CompletedTestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}