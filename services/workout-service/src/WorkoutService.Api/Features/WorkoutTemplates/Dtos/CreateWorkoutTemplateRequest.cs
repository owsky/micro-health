using System.ComponentModel.DataAnnotations;

namespace WorkoutService.Features.WorkoutTemplates.Dtos;

public class CreateWorkoutTemplateRequest
{
  [Required]
  [MinLength(1)]
  [MaxLength(50)]
  public required string Name { get; init; }

  [Required]
  [MinLength(1)]
  [MaxLength(50)]
  public required List<long> ExerciseIds { get; init; }
}
