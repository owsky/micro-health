using System.ComponentModel.DataAnnotations;

namespace WorkoutService.Features.Workouts.Dto;

public class UpdateWorkoutRequest
{
  [MaxLength(50)]
  public string? Name { get; init; }

  public required DateTime StartedAt { get; init; }

  public required DateTime CompletedAt { get; init; }

  [MaxLength(500)]
  public string? Note { get; init; }

  public required IReadOnlyList<CompletedSetRequest> Sets { get; init; }
}
