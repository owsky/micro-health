using System.ComponentModel.DataAnnotations;

namespace WorkoutService.Features.Workouts.Dto;

public class RepsSetRequest : CompletedSetRequest
{
  [Range(1, 9999)]
  public required int Reps { get; init; }
}
