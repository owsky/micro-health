using System.ComponentModel.DataAnnotations;
using WorkoutService.Common.Enums;

namespace WorkoutService.Features.ExerciseCatalog.Dtos;

public class UpdateExerciseRequest
{
  [Required]
  [MinLength(1)]
  [MaxLength(30)]
  public required string Name { get; init; }

  [Required]
  [EnumDataType(typeof(Difficulty))]
  public required Difficulty Difficulty { get; init; }

  [Required]
  [MinLength(1)]
  public required IList<MuscleGroup> MuscleGroups { get; init; }
}
