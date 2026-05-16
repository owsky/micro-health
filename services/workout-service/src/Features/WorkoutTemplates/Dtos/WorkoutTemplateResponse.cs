using WorkoutService.Common.Enums;
using WorkoutService.Features.ExerciseCatalog.Dtos;

namespace WorkoutService.Features.WorkoutTemplates.Dtos;

public class WorkoutTemplateResponse
{
  public long Id { get; init; }

  public required string Creator { get; init; }

  public required string Name { get; init; }

  public required DifficultyEnum OverallDifficulty { get; init; }

  public required IList<ExerciseResponse> Exercises { get; init; }
}
