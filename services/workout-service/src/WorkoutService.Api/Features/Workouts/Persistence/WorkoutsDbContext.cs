using Microsoft.EntityFrameworkCore;
using WorkoutService.Features.Workouts.Domain;

// ReSharper disable once CheckNamespace
namespace WorkoutService.Infrastructure;

public sealed partial class WorkoutServiceDbContext
{
  public DbSet<Workout> Workouts => Set<Workout>();
}
