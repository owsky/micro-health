using WorkoutService.Common.Enums;

namespace WorkoutService.Features.ExerciseCatalog.Domain;

public class ExerciseMuscleGroup
{
  public long ExerciseId { get; init; }

  public MuscleGroup MuscleGroup { get; init; }

  public Exercise? Exercise { get; init; }
}
