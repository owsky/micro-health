using Microsoft.EntityFrameworkCore;
using WorkoutService.Common.Enums;
using WorkoutService.Features.ExerciseCatalog.Domain;
using WorkoutService.Infrastructure;

namespace WorkoutService.Features.ExerciseCatalog.Persistence;

public static class ExerciseSeedData
{
  private const string System = "system";

  private static readonly IReadOnlyList<(
    string Name,
    Difficulty Difficulty,
    MuscleGroup[] MuscleGroups,
    TrackingType TrackingType
  )> Exercises =
  [
    // ── Chest ────────────────────────────────────────────────────────────────
    (
      "Barbell Bench Press",
      Difficulty.Medium,
      [MuscleGroup.Chest, MuscleGroup.Triceps, MuscleGroup.Shoulders],
      TrackingType.WeightAndReps
    ),
    (
      "Incline Dumbbell Press",
      Difficulty.Medium,
      [MuscleGroup.Chest, MuscleGroup.Shoulders, MuscleGroup.Triceps],
      TrackingType.WeightAndReps
    ),
    ("Cable Fly", Difficulty.Easy, [MuscleGroup.Chest], TrackingType.WeightAndReps),
    ("Dips", Difficulty.Medium, [MuscleGroup.Chest, MuscleGroup.Triceps], TrackingType.WeightAndReps),
    ("Push-Up", Difficulty.Easy, [MuscleGroup.Chest, MuscleGroup.Triceps, MuscleGroup.Shoulders], TrackingType.Reps),
    // ── Back ─────────────────────────────────────────────────────────────────
    (
      "Deadlift",
      Difficulty.High,
      [MuscleGroup.LowerBack, MuscleGroup.Hamstrings, MuscleGroup.Glutes, MuscleGroup.Trapezius],
      TrackingType.WeightAndReps
    ),
    ("Pull-Up", Difficulty.Medium, [MuscleGroup.UpperBack, MuscleGroup.Biceps], TrackingType.WeightAndReps),
    (
      "Barbell Row",
      Difficulty.Medium,
      [MuscleGroup.UpperBack, MuscleGroup.Biceps, MuscleGroup.LowerBack],
      TrackingType.WeightAndReps
    ),
    ("Lat Pulldown", Difficulty.Easy, [MuscleGroup.UpperBack, MuscleGroup.Biceps], TrackingType.WeightAndReps),
    ("Seated Cable Row", Difficulty.Easy, [MuscleGroup.UpperBack, MuscleGroup.Biceps], TrackingType.WeightAndReps),
    (
      "T-Bar Row",
      Difficulty.Medium,
      [MuscleGroup.UpperBack, MuscleGroup.LowerBack, MuscleGroup.Biceps],
      TrackingType.WeightAndReps
    ),
    // ── Shoulders ────────────────────────────────────────────────────────────
    (
      "Overhead Barbell Press",
      Difficulty.Medium,
      [MuscleGroup.Shoulders, MuscleGroup.Triceps],
      TrackingType.WeightAndReps
    ),
    ("Dumbbell Lateral Raise", Difficulty.Easy, [MuscleGroup.Shoulders], TrackingType.WeightAndReps),
    ("Face Pull", Difficulty.Easy, [MuscleGroup.Shoulders, MuscleGroup.Trapezius], TrackingType.WeightAndReps),
    ("Arnold Press", Difficulty.Medium, [MuscleGroup.Shoulders, MuscleGroup.Triceps], TrackingType.WeightAndReps),
    // ── Triceps ──────────────────────────────────────────────────────────────
    ("Skull Crusher", Difficulty.Medium, [MuscleGroup.Triceps], TrackingType.WeightAndReps),
    ("Tricep Rope Pushdown", Difficulty.Easy, [MuscleGroup.Triceps], TrackingType.WeightAndReps),
    ("Close-Grip Bench Press", Difficulty.Medium, [MuscleGroup.Triceps, MuscleGroup.Chest], TrackingType.WeightAndReps),
    ("Overhead Tricep Extension", Difficulty.Easy, [MuscleGroup.Triceps], TrackingType.WeightAndReps),
    // ── Biceps ───────────────────────────────────────────────────────────────
    ("Barbell Curl", Difficulty.Easy, [MuscleGroup.Biceps], TrackingType.WeightAndReps),
    ("Hammer Curl", Difficulty.Easy, [MuscleGroup.Biceps, MuscleGroup.Forearms], TrackingType.WeightAndReps),
    ("Incline Dumbbell Curl", Difficulty.Easy, [MuscleGroup.Biceps], TrackingType.WeightAndReps),
    ("Preacher Curl", Difficulty.Easy, [MuscleGroup.Biceps], TrackingType.WeightAndReps),
    // ── Forearms ─────────────────────────────────────────────────────────────
    ("Wrist Curl", Difficulty.Easy, [MuscleGroup.Forearms], TrackingType.WeightAndReps),
    ("Reverse Curl", Difficulty.Easy, [MuscleGroup.Forearms, MuscleGroup.Biceps], TrackingType.WeightAndReps),
    // ── Core / Abs ───────────────────────────────────────────────────────────
    ("Plank", Difficulty.Easy, [MuscleGroup.Core, MuscleGroup.Abs], TrackingType.Time),
    ("Crunch", Difficulty.VeryEasy, [MuscleGroup.Abs], TrackingType.Reps),
    ("Hanging Leg Raise", Difficulty.Medium, [MuscleGroup.Abs, MuscleGroup.Core], TrackingType.Reps),
    ("Cable Crunch", Difficulty.Easy, [MuscleGroup.Abs], TrackingType.WeightAndReps),
    ("Ab Wheel Rollout", Difficulty.High, [MuscleGroup.Core, MuscleGroup.Abs], TrackingType.Reps),
    ("Russian Twist", Difficulty.Easy, [MuscleGroup.Abs, MuscleGroup.Core], TrackingType.WeightAndReps),
    // ── Legs ─────────────────────────────────────────────────────────────────
    (
      "Barbell Back Squat",
      Difficulty.High,
      [MuscleGroup.Quadriceps, MuscleGroup.Glutes, MuscleGroup.Hamstrings, MuscleGroup.Core],
      TrackingType.WeightAndReps
    ),
    (
      "Front Squat",
      Difficulty.High,
      [MuscleGroup.Quadriceps, MuscleGroup.Core, MuscleGroup.Glutes],
      TrackingType.WeightAndReps
    ),
    (
      "Romanian Deadlift",
      Difficulty.Medium,
      [MuscleGroup.Hamstrings, MuscleGroup.Glutes, MuscleGroup.LowerBack],
      TrackingType.WeightAndReps
    ),
    ("Leg Press", Difficulty.Medium, [MuscleGroup.Quadriceps, MuscleGroup.Glutes], TrackingType.WeightAndReps),
    ("Leg Curl", Difficulty.Easy, [MuscleGroup.Hamstrings], TrackingType.WeightAndReps),
    ("Leg Extension", Difficulty.Easy, [MuscleGroup.Quadriceps], TrackingType.WeightAndReps),
    (
      "Bulgarian Split Squat",
      Difficulty.High,
      [MuscleGroup.Quadriceps, MuscleGroup.Glutes, MuscleGroup.Hamstrings],
      TrackingType.WeightAndReps
    ),
    ("Hip Thrust", Difficulty.Medium, [MuscleGroup.Glutes, MuscleGroup.Hamstrings], TrackingType.WeightAndReps),
    (
      "Walking Lunge",
      Difficulty.Medium,
      [MuscleGroup.Quadriceps, MuscleGroup.Glutes, MuscleGroup.Hamstrings],
      TrackingType.WeightAndReps
    ),
    ("Standing Calf Raise", Difficulty.Easy, [MuscleGroup.Calves], TrackingType.WeightAndReps),
    ("Seated Calf Raise", Difficulty.Easy, [MuscleGroup.Calves], TrackingType.WeightAndReps),
    // ── Trapezius ────────────────────────────────────────────────────────────
    ("Barbell Shrug", Difficulty.Easy, [MuscleGroup.Trapezius], TrackingType.WeightAndReps),
    ("Dumbbell Shrug", Difficulty.Easy, [MuscleGroup.Trapezius], TrackingType.WeightAndReps),
    // ── Full Body / Compound ─────────────────────────────────────────────────
    (
      "Power Clean",
      Difficulty.VeryHigh,
      [MuscleGroup.Trapezius, MuscleGroup.Glutes, MuscleGroup.Hamstrings, MuscleGroup.Shoulders],
      TrackingType.WeightAndReps
    ),
    (
      "Kettlebell Swing",
      Difficulty.Medium,
      [MuscleGroup.Glutes, MuscleGroup.Hamstrings, MuscleGroup.Core, MuscleGroup.LowerBack],
      TrackingType.WeightAndReps
    ),
    (
      "Farmer's Walk",
      Difficulty.Medium,
      [MuscleGroup.Forearms, MuscleGroup.Trapezius, MuscleGroup.Core],
      TrackingType.WeightAndTime
    ),
    // ── Cardio ───────────────────────────────────────────────────────────────
    (
      "Running",
      Difficulty.Medium,
      [MuscleGroup.Quadriceps, MuscleGroup.Hamstrings, MuscleGroup.Calves],
      TrackingType.DistanceAndTime
    ),
    (
      "Cycling",
      Difficulty.Medium,
      [MuscleGroup.Quadriceps, MuscleGroup.Hamstrings, MuscleGroup.Calves, MuscleGroup.Glutes],
      TrackingType.DistanceAndTime
    ),
    (
      "Rowing Machine",
      Difficulty.Medium,
      [MuscleGroup.UpperBack, MuscleGroup.Core, MuscleGroup.Hamstrings, MuscleGroup.Glutes],
      TrackingType.DistanceAndTime
    ),
    ("Jump Rope", Difficulty.Medium, [MuscleGroup.Calves, MuscleGroup.Core, MuscleGroup.Shoulders], TrackingType.Time),
    (
      "Treadmill",
      Difficulty.Easy,
      [MuscleGroup.Quadriceps, MuscleGroup.Hamstrings, MuscleGroup.Calves],
      TrackingType.DistanceAndTime
    ),
    (
      "Stair Climber",
      Difficulty.Medium,
      [MuscleGroup.Quadriceps, MuscleGroup.Glutes, MuscleGroup.Calves],
      TrackingType.Time
    ),
    (
      "Elliptical",
      Difficulty.Easy,
      [MuscleGroup.Quadriceps, MuscleGroup.Hamstrings, MuscleGroup.Glutes],
      TrackingType.DistanceAndTime
    ),
    (
      "Burpees",
      Difficulty.High,
      [MuscleGroup.Core, MuscleGroup.Chest, MuscleGroup.Shoulders, MuscleGroup.Quadriceps, MuscleGroup.Glutes],
      TrackingType.Reps
    ),
    (
      "Box Jump",
      Difficulty.High,
      [MuscleGroup.Quadriceps, MuscleGroup.Glutes, MuscleGroup.Hamstrings, MuscleGroup.Calves],
      TrackingType.Reps
    ),
    (
      "Battle Ropes",
      Difficulty.High,
      [MuscleGroup.Shoulders, MuscleGroup.Core, MuscleGroup.Forearms],
      TrackingType.Time
    ),
    (
      "Swimming",
      Difficulty.Medium,
      [MuscleGroup.Shoulders, MuscleGroup.UpperBack, MuscleGroup.Core],
      TrackingType.DistanceAndTime
    ),
  ];

  public static void Seed(WorkoutServiceDbContext context)
  {
    var existingNames = context.Exercises.Where(e => e.Creator == System).Select(e => e.Name).ToHashSet();
    var toAdd = BuildExercises(existingNames);
    if (toAdd.Count == 0)
      return;
    context.Exercises.AddRange(toAdd);
    context.SaveChanges();
  }

  public static async Task SeedAsync(WorkoutServiceDbContext context, CancellationToken ct = default)
  {
    var existingNames = (
      await context.Exercises.Where(e => e.Creator == System).Select(e => e.Name).ToListAsync(ct)
    ).ToHashSet();
    var toAdd = BuildExercises(existingNames);
    if (toAdd.Count == 0)
      return;
    context.Exercises.AddRange(toAdd);
    await context.SaveChangesAsync(ct);
  }

  private static List<Exercise> BuildExercises(HashSet<string> existingNames) =>
    Exercises
      .Where(e => !existingNames.Contains(e.Name))
      .Select(e =>
      {
        var exercise = new Exercise
        {
          Creator = System,
          Name = e.Name,
          Difficulty = e.Difficulty,
          TrackingType = e.TrackingType,
        };
        foreach (var group in e.MuscleGroups.Distinct())
          exercise.ExerciseMuscleGroups.Add(new ExerciseMuscleGroup { MuscleGroup = group, Exercise = exercise });
        return exercise;
      })
      .ToList();
}
