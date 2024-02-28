using GrammarLab.BLL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GrammarLab.DAL.Database.Configurations;

public class LevelConfiguration : IEntityTypeConfiguration<Level>
{
    public void Configure(EntityTypeBuilder<Level> builder)
    {
        builder.Property(e => e.Code).HasMaxLength(16);
        builder.Property(e => e.Name).HasMaxLength(128);

        builder.HasData(
           new { Id = 1, Code = "A1", Name = "Beginner" },
           new { Id = 2, Code = "A2", Name = "Pre-Intermediate" },
           new { Id = 3, Code = "B1", Name = "Intermediate" },
           new { Id = 4, Code = "B2", Name = "Upper-Intermediate" },
           new { Id = 5, Code = "C1", Name = "Advanced" },
           new { Id = 6, Code = "C2", Name = "Proficient" });
    }
}