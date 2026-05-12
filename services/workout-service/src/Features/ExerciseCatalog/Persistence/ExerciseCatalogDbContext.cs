using Microsoft.EntityFrameworkCore;
using WorkoutService.Features.ExerciseCatalog.Domain;

namespace WorkoutService.Features.ExerciseCatalog.Persistence;

public sealed class ExerciseCatalogDbContext(DbContextOptions<ExerciseCatalogDbContext> options) : DbContext(options)
{
  public DbSet<Exercise> Exercises => Set<Exercise>();
  public DbSet<ExerciseMuscleGroup> ExercisesMuscleGroups => Set<ExerciseMuscleGroup>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExerciseCatalogDbContext).Assembly);
    base.OnModelCreating(modelBuilder);
  }
}
