using System.Diagnostics;
using Riok.Mapperly.Abstractions;
using WorkoutService.Features.Workouts.Domain;
using WorkoutService.Features.Workouts.Dto;

namespace WorkoutService.Features.Workouts.Mappings;

[Mapper]
public static partial class WorkoutMapper
{
  [MapProperty(nameof(Workout.WorkoutSets), nameof(WorkoutResponse.Sets))]
  public static partial WorkoutResponse ToResponse(Workout workout);

  private static CompletedSetResponse ToResponse(WorkoutSet workoutSet)
  {
    ArgumentNullException.ThrowIfNull(workoutSet);

    return workoutSet switch
    {
      WeightAndRepsSet s => new WeightAndRepsSetResponse
      {
        ExerciseId = s.ExerciseId,
        SetNumber = s.SetNumber,
        IsWarmup = s.IsWarmup,
        Weight = s.Weight,
        Reps = s.Reps,
      },
      RepsSet s => new RepsSetResponse
      {
        ExerciseId = s.ExerciseId,
        SetNumber = s.SetNumber,
        IsWarmup = s.IsWarmup,
        Reps = s.Reps,
      },
      DistanceAndTimeSet s => new DistanceAndTimeSetResponse
      {
        ExerciseId = s.ExerciseId,
        SetNumber = s.SetNumber,
        IsWarmup = s.IsWarmup,
        Distance = s.Distance,
        DurationSeconds = s.DurationSeconds,
      },
      TimeSet s => new TimeSetResponse
      {
        ExerciseId = s.ExerciseId,
        SetNumber = s.SetNumber,
        IsWarmup = s.IsWarmup,
        DurationSeconds = s.DurationSeconds,
      },
      WeightAndTimeSet s => new WeightAndTimeSetResponse
      {
        ExerciseId = s.ExerciseId,
        SetNumber = s.SetNumber,
        IsWarmup = s.IsWarmup,
        Weight = s.Weight,
        DurationSeconds = s.DurationSeconds,
      },
      _ => throw new UnreachableException($"Unknown workout set type: {workoutSet.GetType().Name}"),
    };
  }

  [MapProperty(nameof(CreateWorkoutRequest.Sets), nameof(Workout.WorkoutSets))]
  [MapperIgnoreTarget(nameof(Workout.Id))]
  public static partial Workout ToDomain(CreateWorkoutRequest request, string creator);

  public static WorkoutSet ToDomain(CompletedSetRequest request) =>
    request switch
    {
      WeightAndRepsSetRequest r => new WeightAndRepsSet
      {
        ExerciseId = r.ExerciseId,
        SetNumber = r.SetNumber,
        IsWarmup = r.IsWarmup,
        Weight = r.Weight,
        Reps = r.Reps,
        WorkoutId = 0,
      },
      RepsSetRequest r => new RepsSet
      {
        ExerciseId = r.ExerciseId,
        SetNumber = r.SetNumber,
        IsWarmup = r.IsWarmup,
        Reps = r.Reps,
        WorkoutId = 0,
      },
      DistanceAndTimeSetRequest r => new DistanceAndTimeSet
      {
        ExerciseId = r.ExerciseId,
        SetNumber = r.SetNumber,
        IsWarmup = r.IsWarmup,
        Distance = r.Distance,
        DurationSeconds = r.DurationSeconds,
        WorkoutId = 0,
      },
      TimeSetRequest r => new TimeSet
      {
        ExerciseId = r.ExerciseId,
        SetNumber = r.SetNumber,
        IsWarmup = r.IsWarmup,
        DurationSeconds = r.DurationSeconds,
        WorkoutId = 0,
      },
      WeightAndTimeSetRequest r => new WeightAndTimeSet
      {
        ExerciseId = r.ExerciseId,
        SetNumber = r.SetNumber,
        IsWarmup = r.IsWarmup,
        Weight = r.Weight,
        DurationSeconds = r.DurationSeconds,
        WorkoutId = 0,
      },
      _ => throw new ArgumentOutOfRangeException(nameof(request)),
    };
}
