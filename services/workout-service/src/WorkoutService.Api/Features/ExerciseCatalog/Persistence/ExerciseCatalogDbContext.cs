using Microsoft.EntityFrameworkCore;
using WorkoutService.Features.ExerciseCatalog.Domain;

// ReSharper disable once CheckNamespace
namespace WorkoutService.Infrastructure;

public sealed partial class WorkoutServiceDbContext
{
  public DbSet<Exercise> Exercises => Set<Exercise>();
  public DbSet<ExerciseMuscleGroup> ExercisesMuscleGroups => Set<ExerciseMuscleGroup>();
}
