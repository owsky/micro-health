using System.ComponentModel.DataAnnotations;

namespace WorkoutService.Features.Workouts.Dto;

public class DistanceAndTimeSetRequest : CompletedSetRequest
{
  [Range(0.01, 100000)]
  public required decimal Distance { get; init; }

  [Range(1, int.MaxValue)]
  public required int DurationSeconds { get; init; }
}
