using System.ComponentModel.DataAnnotations;
using WorkoutService.Common.Enums;
using WorkoutService.Features.ExerciseCatalog.Enums;

namespace WorkoutService.Features.ExerciseCatalog.Dtos;

public class UpdateExerciseRequest
{
  [Required]
  [MinLength(1)]
  [MaxLength(30)]
  public required string Name { get; init; }

  [Required]
  [EnumDataType(typeof(DifficultyEnum))]
  public required DifficultyEnum Difficulty { get; init; }

  [Required]
  [MinLength(1)]
  public required IList<MuscleGroupEnum> MuscleGroups { get; init; }
}
