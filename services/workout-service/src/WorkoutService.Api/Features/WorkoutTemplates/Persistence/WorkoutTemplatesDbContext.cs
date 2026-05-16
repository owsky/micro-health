using Microsoft.EntityFrameworkCore;
using WorkoutService.Features.WorkoutTemplates.Domain;

// ReSharper disable once CheckNamespace
namespace WorkoutService.Infrastructure;

public sealed partial class WorkoutServiceDbContext
{
  public DbSet<WorkoutTemplate> WorkoutTemplates => Set<WorkoutTemplate>();
  public DbSet<WorkoutTemplateExercise> WorkoutTemplateExercises => Set<WorkoutTemplateExercise>();
}
