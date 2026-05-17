using WorkoutService.Common.Enums;

namespace WorkoutService.Features.Workouts.Dto;

public static class CompletedSetRequestExtensions
{
  public static TrackingType GetTrackingType(this CompletedSetRequest set) =>
    set switch
    {
      WeightAndRepsSetRequest => TrackingType.WeightAndReps,
      RepsSetRequest => TrackingType.Reps,
      DistanceAndTimeSetRequest => TrackingType.DistanceAndTime,
      TimeSetRequest => TrackingType.Time,
      WeightAndTimeSetRequest => TrackingType.WeightAndTime,
      _ => throw new ArgumentOutOfRangeException(nameof(set), $"Unknown set type: {set.GetType().Name}"),
    };
}
