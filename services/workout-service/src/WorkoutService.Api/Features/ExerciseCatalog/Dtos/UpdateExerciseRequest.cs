using System.ComponentModel.DataAnnotations;
using WorkoutService.Common.Enums;

namespace WorkoutService.Features.ExerciseCatalog.Dtos;

public class UpdateExerciseRequest
{
  [MinLength(1)]
  [MaxLength(30)]
  public required string Name { get; init; }

  public required Difficulty Difficulty { get; init; }

  public required TrackingType TrackingType { get; init; }

  [MinLength(1)]
  public required IList<MuscleGroup> MuscleGroups { get; init; }
}
