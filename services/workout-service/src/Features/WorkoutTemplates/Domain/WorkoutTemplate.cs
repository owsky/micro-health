using WorkoutService.Common.Enums;

namespace WorkoutService.Features.WorkoutTemplates.Domain;

public class WorkoutTemplate
{
  public long Id { get; init; }

  public required string Creator { get; init; }

  public required string Name { get; set; }

  public DifficultyEnum OverallDifficulty { get; set; }

  /// <summary>Ordered list of exercises belonging to this template.</summary>
  public List<WorkoutTemplateExercise> Exercises { get; set; } = new List<WorkoutTemplateExercise>();
}
