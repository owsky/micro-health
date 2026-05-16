using WorkoutService.Features.ExerciseCatalog.Enums;

namespace WorkoutService.Features.ExerciseCatalog.Domain;

public class ExerciseMuscleGroup
{
  public long ExerciseId { get; init; }

  public MuscleGroupEnum MuscleGroup { get; init; }

  public Exercise? Exercise { get; init; }
}
