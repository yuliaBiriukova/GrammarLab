using GrammarLab.BLL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GrammarLab.DAL.Database.Configurations;

public class TestResultExerciseConfiguration : IEntityTypeConfiguration<TestResultExercise>
{
    public void Configure(EntityTypeBuilder<TestResultExercise> builder)
    {
        builder.Property(c => c.Task).HasMaxLength(256);
        builder.Property(c => c.Answer).HasMaxLength(256);
        builder.Property(c => c.UserAnswer).HasMaxLength(256);

        builder.HasOne(c => c.TestResult)
            .WithMany(c => c.TestResultExercises)
            .HasForeignKey(c => c.TestResultId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}