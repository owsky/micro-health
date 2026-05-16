using WorkoutService.Features.ExerciseCatalog.Domain;

namespace WorkoutService.Features.Workouts.Domain;

public abstract class WorkoutSet
{
  public required long WorkoutId { get; init; }

  public Workout? Workout { get; init; }

  public required long ExerciseId { get; init; }

  public Exercise? Exercise { get; init; }

  public required int SetNumber { get; set; }

  public required bool IsWarmup { get; set; }
}
