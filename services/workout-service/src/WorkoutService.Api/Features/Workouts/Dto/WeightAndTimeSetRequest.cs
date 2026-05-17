using System.ComponentModel.DataAnnotations;

namespace WorkoutService.Features.Workouts.Dto;

public class WeightAndTimeSetRequest : CompletedSetRequest
{
  [Range(0.01, 2000)]
  public required decimal Weight { get; init; }

  [Range(1, int.MaxValue)]
  public required int DurationSeconds { get; init; }
}
