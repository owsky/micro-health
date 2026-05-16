using WorkoutService.Common.Enums;
using WorkoutService.Features.ExerciseCatalog.Enums;

namespace WorkoutService.Features.ExerciseCatalog.Dtos;

public class ExerciseResponse
{
  public long Id { get; init; }

  public required string Name { get; init; }

  public required string Creator { get; init; }

  public required DifficultyEnum Difficulty { get; init; }

  public required IList<MuscleGroupEnum> MuscleGroups { get; init; }
}
