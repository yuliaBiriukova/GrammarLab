using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using GrammarLab.BLL.Entities;

namespace GrammarLab.DAL.Database.Configurations;

public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new()
            {
                Id = "65d65432-f32b-48be-8359-4cd5a8b57199",
                Name = UserRole.Admin.ToString(),
                NormalizedName = UserRole.Admin.ToString().ToUpper(CultureInfo.InvariantCulture)
            },
            new()
            {
                Id = "a019f80a-ae40-4644-8abb-f092305c0a7d",
                Name = UserRole.Teacher.ToString(),
                NormalizedName = UserRole.Teacher.ToString().ToUpper(CultureInfo.InvariantCulture)
            },
            new()
            {
                Id = "00d616ff-c79a-431d-b801-792492ac8efc",
                Name = UserRole.Student.ToString(),
                NormalizedName = UserRole.Student.ToString().ToUpper(CultureInfo.InvariantCulture)
            }
        );
    }
}