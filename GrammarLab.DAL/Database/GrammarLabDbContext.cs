using GrammarLab.BLL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GrammarLab.DAL.Database;

public class GrammarLabDbContext : IdentityDbContext<User>
{
    public DbSet<Level> Levels { get; set; }

    public DbSet<Topic> Topics { get; set; }

    public DbSet<Exercise> Exercises { get; set; }

    public DbSet<TestResult> TestResults { get; set; }

    public DbSet<TestResultExercise> TestResultExercises { get; set; }

    public GrammarLabDbContext(DbContextOptions options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GrammarLabDbContext).Assembly);
    }
}