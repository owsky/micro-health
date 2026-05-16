using WorkoutService.Features.ExerciseCatalog.Domain;

namespace WorkoutService.Features.WorkoutTemplates.Domain;

public class WorkoutTemplateExercise
{
  public long WorkoutTemplateId { get; init; }
  public WorkoutTemplate? WorkoutTemplate { get; set; }

  public long ExerciseId { get; init; }

  public Exercise? Exercise { get; set; }

  /// <summary>1-based position of the exercise within the template.</summary>
  public int Order { get; set; }
}
