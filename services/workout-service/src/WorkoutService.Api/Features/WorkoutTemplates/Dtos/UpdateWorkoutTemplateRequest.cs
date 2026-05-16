using System.ComponentModel.DataAnnotations;

namespace WorkoutService.Features.WorkoutTemplates.Dtos;

public class UpdateWorkoutTemplateRequest
{
  [Required]
  public required long Id { get; init; }

  [Required]
  [MinLength(1)]
  [MaxLength(50)]
  public required string Name { get; init; }

  [Required]
  [MinLength(1)]
  [MaxLength(50)]
  public required List<long> Exercises { get; init; }
}
