using WorkoutService.Common.Enums;

namespace WorkoutService.Features.ExerciseCatalog.Dtos;

public class ExerciseResponse
{
  public long Id { get; init; }

  public required string Name { get; init; }

  public required string Creator { get; init; }

  public required Difficulty Difficulty { get; init; }

  public required IList<MuscleGroup> MuscleGroups { get; init; }
}
