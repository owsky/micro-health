using System.ComponentModel.DataAnnotations;

namespace WorkoutService.Features.Workouts.Dto;

public class TimeSetRequest : CompletedSetRequest
{
  [Range(1, int.MaxValue)]
  public required int DurationSeconds { get; init; }
}
