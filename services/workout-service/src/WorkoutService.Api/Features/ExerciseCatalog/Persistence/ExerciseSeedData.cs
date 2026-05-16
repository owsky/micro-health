using Microsoft.EntityFrameworkCore;
using WorkoutService.Common.Enums;
using WorkoutService.Features.ExerciseCatalog.Domain;
using WorkoutService.Infrastructure;

namespace WorkoutService.Features.ExerciseCatalog.Persistence;

public static class ExerciseSeedData
{
  private const string System = "system";

  private static readonly IReadOnlyList<(string Name, Difficulty Difficulty, MuscleGroup[] MuscleGroups)> Exercises =
  [
    // ── Chest ────────────────────────────────────────────────────────────────
    ("Barbell Bench Press", Difficulty.Medium, [MuscleGroup.Chest, MuscleGroup.Triceps, MuscleGroup.Shoulders]),
    ("Incline Dumbbell Press", Difficulty.Medium, [MuscleGroup.Chest, MuscleGroup.Shoulders, MuscleGroup.Triceps]),
    ("Cable Fly", Difficulty.Easy, [MuscleGroup.Chest]),
    ("Dips", Difficulty.Medium, [MuscleGroup.Chest, MuscleGroup.Triceps]),
    ("Push-Up", Difficulty.Easy, [MuscleGroup.Chest, MuscleGroup.Triceps, MuscleGroup.Shoulders]),
    // ── Back ─────────────────────────────────────────────────────────────────
    (
      "Deadlift",
      Difficulty.High,
      [MuscleGroup.LowerBack, MuscleGroup.Hamstrings, MuscleGroup.Glutes, MuscleGroup.Trapezius]
    ),
    ("Pull-Up", Difficulty.Medium, [MuscleGroup.UpperBack, MuscleGroup.Biceps]),
    ("Barbell Row", Difficulty.Medium, [MuscleGroup.UpperBack, MuscleGroup.Biceps, MuscleGroup.LowerBack]),
    ("Lat Pulldown", Difficulty.Easy, [MuscleGroup.UpperBack, MuscleGroup.Biceps]),
    ("Seated Cable Row", Difficulty.Easy, [MuscleGroup.UpperBack, MuscleGroup.Biceps]),
    ("T-Bar Row", Difficulty.Medium, [MuscleGroup.UpperBack, MuscleGroup.LowerBack, MuscleGroup.Biceps]),
    // ── Shoulders ────────────────────────────────────────────────────────────
    ("Overhead Barbell Press", Difficulty.Medium, [MuscleGroup.Shoulders, MuscleGroup.Triceps]),
    ("Dumbbell Lateral Raise", Difficulty.Easy, [MuscleGroup.Shoulders]),
    ("Face Pull", Difficulty.Easy, [MuscleGroup.Shoulders, MuscleGroup.Trapezius]),
    ("Arnold Press", Difficulty.Medium, [MuscleGroup.Shoulders, MuscleGroup.Triceps]),
    // ── Triceps ──────────────────────────────────────────────────────────────
    ("Skull Crusher", Difficulty.Medium, [MuscleGroup.Triceps]),
    ("Tricep Rope Pushdown", Difficulty.Easy, [MuscleGroup.Triceps]),
    ("Close-Grip Bench Press", Difficulty.Medium, [MuscleGroup.Triceps, MuscleGroup.Chest]),
    ("Overhead Tricep Extension", Difficulty.Easy, [MuscleGroup.Triceps]),
    // ── Biceps ───────────────────────────────────────────────────────────────
    ("Barbell Curl", Difficulty.Easy, [MuscleGroup.Biceps]),
    ("Hammer Curl", Difficulty.Easy, [MuscleGroup.Biceps, MuscleGroup.Forearms]),
    ("Incline Dumbbell Curl", Difficulty.Easy, [MuscleGroup.Biceps]),
    ("Preacher Curl", Difficulty.Easy, [MuscleGroup.Biceps]),
    // ── Forearms ─────────────────────────────────────────────────────────────
    ("Wrist Curl", Difficulty.Easy, [MuscleGroup.Forearms]),
    ("Reverse Curl", Difficulty.Easy, [MuscleGroup.Forearms, MuscleGroup.Biceps]),
    // ── Core / Abs ───────────────────────────────────────────────────────────
    ("Plank", Difficulty.Easy, [MuscleGroup.Core, MuscleGroup.Abs]),
    ("Crunch", Difficulty.VeryEasy, [MuscleGroup.Abs]),
    ("Hanging Leg Raise", Difficulty.Medium, [MuscleGroup.Abs, MuscleGroup.Core]),
    ("Cable Crunch", Difficulty.Easy, [MuscleGroup.Abs]),
    ("Ab Wheel Rollout", Difficulty.High, [MuscleGroup.Core, MuscleGroup.Abs]),
    ("Russian Twist", Difficulty.Easy, [MuscleGroup.Abs, MuscleGroup.Core]),
    // ── Legs ─────────────────────────────────────────────────────────────────
    (
      "Barbell Back Squat",
      Difficulty.High,
      [MuscleGroup.Quadriceps, MuscleGroup.Glutes, MuscleGroup.Hamstrings, MuscleGroup.Core]
    ),
    ("Front Squat", Difficulty.High, [MuscleGroup.Quadriceps, MuscleGroup.Core, MuscleGroup.Glutes]),
    ("Romanian Deadlift", Difficulty.Medium, [MuscleGroup.Hamstrings, MuscleGroup.Glutes, MuscleGroup.LowerBack]),
    ("Leg Press", Difficulty.Medium, [MuscleGroup.Quadriceps, MuscleGroup.Glutes]),
    ("Leg Curl", Difficulty.Easy, [MuscleGroup.Hamstrings]),
    ("Leg Extension", Difficulty.Easy, [MuscleGroup.Quadriceps]),
    ("Bulgarian Split Squat", Difficulty.High, [MuscleGroup.Quadriceps, MuscleGroup.Glutes, MuscleGroup.Hamstrings]),
    ("Hip Thrust", Difficulty.Medium, [MuscleGroup.Glutes, MuscleGroup.Hamstrings]),
    ("Walking Lunge", Difficulty.Medium, [MuscleGroup.Quadriceps, MuscleGroup.Glutes, MuscleGroup.Hamstrings]),
    ("Standing Calf Raise", Difficulty.Easy, [MuscleGroup.Calves]),
    ("Seated Calf Raise", Difficulty.Easy, [MuscleGroup.Calves]),
    // ── Trapezius ────────────────────────────────────────────────────────────
    ("Barbell Shrug", Difficulty.Easy, [MuscleGroup.Trapezius]),
    ("Dumbbell Shrug", Difficulty.Easy, [MuscleGroup.Trapezius]),
    // ── Full Body / Compound ─────────────────────────────────────────────────
    (
      "Power Clean",
      Difficulty.VeryHigh,
      [MuscleGroup.Trapezius, MuscleGroup.Glutes, MuscleGroup.Hamstrings, MuscleGroup.Shoulders]
    ),
    (
      "Kettlebell Swing",
      Difficulty.Medium,
      [MuscleGroup.Glutes, MuscleGroup.Hamstrings, MuscleGroup.Core, MuscleGroup.LowerBack]
    ),
    ("Farmer's Walk", Difficulty.Medium, [MuscleGroup.Forearms, MuscleGroup.Trapezius, MuscleGroup.Core]),
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
        };
        foreach (var group in e.MuscleGroups.Distinct())
          exercise.ExerciseMuscleGroups.Add(new ExerciseMuscleGroup { MuscleGroup = group, Exercise = exercise });
        return exercise;
      })
      .ToList();
}
