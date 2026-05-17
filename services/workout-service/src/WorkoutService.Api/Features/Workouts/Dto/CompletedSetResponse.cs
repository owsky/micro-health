using System.Text.Json.Serialization;

namespace WorkoutService.Features.Workouts.Dto;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "trackingType")]
[JsonDerivedType(typeof(WeightAndRepsSetResponse), "WeightAndReps")]
[JsonDerivedType(typeof(RepsSetResponse), "Reps")]
[JsonDerivedType(typeof(DistanceAndTimeSetResponse), "DistanceAndTime")]
[JsonDerivedType(typeof(TimeSetResponse), "Time")]
[JsonDerivedType(typeof(WeightAndTimeSetResponse), "WeightAndTime")]
public abstract class CompletedSetResponse
{
  public required long ExerciseId { get; init; }

  public required int SetNumber { get; init; }

  public required bool IsWarmup { get; init; }
}

public class WeightAndRepsSetResponse : CompletedSetResponse
{
  public required decimal Weight { get; init; }

  public required int Reps { get; init; }
}

public class RepsSetResponse : CompletedSetResponse
{
  public required int Reps { get; init; }
}

public class DistanceAndTimeSetResponse : CompletedSetResponse
{
  public required decimal Distance { get; init; }

  public required int DurationSeconds { get; init; }
}

public class TimeSetResponse : CompletedSetResponse
{
  public required int DurationSeconds { get; init; }
}

public class WeightAndTimeSetResponse : CompletedSetResponse
{
  public required decimal Weight { get; init; }

  public required int DurationSeconds { get; init; }
}
