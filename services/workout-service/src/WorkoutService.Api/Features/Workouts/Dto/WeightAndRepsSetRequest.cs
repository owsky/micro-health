using System.ComponentModel.DataAnnotations;

namespace WorkoutService.Features.Workouts.Dto;

public class WeightAndRepsSetRequest : CompletedSetRequest
{
  [Range(0.01, 2000)]
  public required decimal Weight { get; init; }

  [Range(1, 9999)]
  public required int Reps { get; init; }
}
