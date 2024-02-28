using GrammarLab.BLL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GrammarLab.DAL.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(256);
        builder.Property(u => u.LastName).HasMaxLength(256);

        builder.HasOne(u => u.Level)
            .WithMany()
            .HasForeignKey(u => u.LevelId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}