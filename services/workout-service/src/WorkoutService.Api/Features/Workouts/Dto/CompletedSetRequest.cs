using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WorkoutService.Features.Workouts.Dto;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "trackingType")]
[JsonDerivedType(typeof(WeightAndRepsSetRequest), "WeightAndReps")]
[JsonDerivedType(typeof(RepsSetRequest), "Reps")]
[JsonDerivedType(typeof(DistanceAndTimeSetRequest), "DistanceAndTime")]
[JsonDerivedType(typeof(TimeSetRequest), "Time")]
[JsonDerivedType(typeof(WeightAndTimeSetRequest), "WeightAndTime")]
public abstract class CompletedSetRequest
{
  public required long ExerciseId { get; init; }

  [Range(1, 100)]
  public required int SetNumber { get; init; }

  public required bool IsWarmup { get; init; }
}
