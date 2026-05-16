using WorkoutService.Common.Enums;

namespace WorkoutService.Features.ExerciseCatalog.Domain;

public class Exercise
{
  public long Id { get; init; }

  public required string Creator { get; init; }

  public required string Name { get; set; }

  public required Difficulty Difficulty { get; set; }

  public required TrackingType TrackingType { get; set; }

  public ICollection<ExerciseMuscleGroup> ExerciseMuscleGroups { get; set; } = new List<ExerciseMuscleGroup>();
}
