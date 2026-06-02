using WorkoutService.Common.Enums;
using WorkoutService.Features.ExerciseCatalog.Dtos;
using WorkoutService.Features.Workouts.Dto;
using WorkoutService.Features.WorkoutTemplates.Dtos;

namespace WorkoutService.Tests.Common.Helpers;

public static class DtoHelpers
{
  public static ExerciseResponse MakeExerciseResponse(long id, string creator) =>
    new()
    {
      Id = id,
      Name = "Bench Press",
      Creator = creator,
      Difficulty = Difficulty.Medium,
      TrackingType = TrackingType.WeightAndReps,
      MuscleGroups = [MuscleGroup.Chest, MuscleGroup.Triceps],
    };

  public static WorkoutResponse MakeWorkoutResponse(
    long id,
    string creator,
    DateTime startedAt,
    DateTime completedAt,
    string? name = null
  ) =>
    new()
    {
      Id = id,
      Creator = creator,
      Name = name ?? "Morning Workout",
      StartedAt = startedAt,
      CompletedAt = completedAt,
      Note = "Great session",
      Sets =
      [
        MakeWeightAndRepsSetResponse(1, 1),
        MakeWeightAndRepsSetResponse(1, 2),
        MakeDistanceAndTimeSetResponse(2, 1),
      ],
    };

  public static WorkoutTemplateResponse MakeWorkoutTemplateResponse(long id, string creator, string name) =>
    new()
    {
      Id = id,
      Creator = creator,
      Name = name,
      OverallDifficulty = Difficulty.Medium,
      Exercises = [MakeExerciseResponse(1, creator), MakeExerciseResponse(2, creator)],
    };

  public static WeightAndRepsSetResponse MakeWeightAndRepsSetResponse(long exerciseId, int setNumber) =>
    new()
    {
      ExerciseId = exerciseId,
      SetNumber = setNumber,
      IsWarmup = false,
      Weight = 100,
      Reps = 10,
    };

  public static DistanceAndTimeSetResponse MakeDistanceAndTimeSetResponse(long exerciseId, int setNumber) =>
    new()
    {
      ExerciseId = exerciseId,
      SetNumber = setNumber,
      IsWarmup = false,
      Distance = 5,
      DurationSeconds = 300,
    };
}
