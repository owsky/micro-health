using Microsoft.EntityFrameworkCore;
using WorkoutService.Features.ExerciseCatalog.Domain;
using WorkoutService.Features.ExerciseCatalog.Enums;

namespace WorkoutService.Features.ExerciseCatalog.Persistence;

public static class ExerciseSeedData
{
  private const string System = "system";

  public static readonly IReadOnlyList<(
    string Name,
    DifficultyEnum Difficulty,
    MuscleGroupEnum[] MuscleGroups
  )> Exercises =
  [
    // ── Chest ────────────────────────────────────────────────────────────────
    (
      "Barbell Bench Press",
      DifficultyEnum.Medium,
      [MuscleGroupEnum.Chest, MuscleGroupEnum.Triceps, MuscleGroupEnum.Shoulders]
    ),
    (
      "Incline Dumbbell Press",
      DifficultyEnum.Medium,
      [MuscleGroupEnum.Chest, MuscleGroupEnum.Shoulders, MuscleGroupEnum.Triceps]
    ),
    ("Cable Fly", DifficultyEnum.Easy, [MuscleGroupEnum.Chest]),
    ("Dips", DifficultyEnum.Medium, [MuscleGroupEnum.Chest, MuscleGroupEnum.Triceps]),
    ("Push-Up", DifficultyEnum.Easy, [MuscleGroupEnum.Chest, MuscleGroupEnum.Triceps, MuscleGroupEnum.Shoulders]),
    // ── Back ─────────────────────────────────────────────────────────────────
    (
      "Deadlift",
      DifficultyEnum.High,
      [MuscleGroupEnum.LowerBack, MuscleGroupEnum.Hamstrings, MuscleGroupEnum.Glutes, MuscleGroupEnum.Trapezius]
    ),
    ("Pull-Up", DifficultyEnum.Medium, [MuscleGroupEnum.UpperBack, MuscleGroupEnum.Biceps]),
    (
      "Barbell Row",
      DifficultyEnum.Medium,
      [MuscleGroupEnum.UpperBack, MuscleGroupEnum.Biceps, MuscleGroupEnum.LowerBack]
    ),
    ("Lat Pulldown", DifficultyEnum.Easy, [MuscleGroupEnum.UpperBack, MuscleGroupEnum.Biceps]),
    ("Seated Cable Row", DifficultyEnum.Easy, [MuscleGroupEnum.UpperBack, MuscleGroupEnum.Biceps]),
    (
      "T-Bar Row",
      DifficultyEnum.Medium,
      [MuscleGroupEnum.UpperBack, MuscleGroupEnum.LowerBack, MuscleGroupEnum.Biceps]
    ),
    // ── Shoulders ────────────────────────────────────────────────────────────
    ("Overhead Barbell Press", DifficultyEnum.Medium, [MuscleGroupEnum.Shoulders, MuscleGroupEnum.Triceps]),
    ("Dumbbell Lateral Raise", DifficultyEnum.Easy, [MuscleGroupEnum.Shoulders]),
    ("Face Pull", DifficultyEnum.Easy, [MuscleGroupEnum.Shoulders, MuscleGroupEnum.Trapezius]),
    ("Arnold Press", DifficultyEnum.Medium, [MuscleGroupEnum.Shoulders, MuscleGroupEnum.Triceps]),
    // ── Triceps ──────────────────────────────────────────────────────────────
    ("Skull Crusher", DifficultyEnum.Medium, [MuscleGroupEnum.Triceps]),
    ("Tricep Rope Pushdown", DifficultyEnum.Easy, [MuscleGroupEnum.Triceps]),
    ("Close-Grip Bench Press", DifficultyEnum.Medium, [MuscleGroupEnum.Triceps, MuscleGroupEnum.Chest]),
    ("Overhead Tricep Extension", DifficultyEnum.Easy, [MuscleGroupEnum.Triceps]),
    // ── Biceps ───────────────────────────────────────────────────────────────
    ("Barbell Curl", DifficultyEnum.Easy, [MuscleGroupEnum.Biceps]),
    ("Hammer Curl", DifficultyEnum.Easy, [MuscleGroupEnum.Biceps, MuscleGroupEnum.Forearms]),
    ("Incline Dumbbell Curl", DifficultyEnum.Easy, [MuscleGroupEnum.Biceps]),
    ("Preacher Curl", DifficultyEnum.Easy, [MuscleGroupEnum.Biceps]),
    // ── Forearms ─────────────────────────────────────────────────────────────
    ("Wrist Curl", DifficultyEnum.Easy, [MuscleGroupEnum.Forearms]),
    ("Reverse Curl", DifficultyEnum.Easy, [MuscleGroupEnum.Forearms, MuscleGroupEnum.Biceps]),
    // ── Core / Abs ───────────────────────────────────────────────────────────
    ("Plank", DifficultyEnum.Easy, [MuscleGroupEnum.Core, MuscleGroupEnum.Abs]),
    ("Crunch", DifficultyEnum.VeryEasy, [MuscleGroupEnum.Abs]),
    ("Hanging Leg Raise", DifficultyEnum.Medium, [MuscleGroupEnum.Abs, MuscleGroupEnum.Core]),
    ("Cable Crunch", DifficultyEnum.Easy, [MuscleGroupEnum.Abs]),
    ("Ab Wheel Rollout", DifficultyEnum.High, [MuscleGroupEnum.Core, MuscleGroupEnum.Abs]),
    ("Russian Twist", DifficultyEnum.Easy, [MuscleGroupEnum.Abs, MuscleGroupEnum.Core]),
    // ── Legs ─────────────────────────────────────────────────────────────────
    (
      "Barbell Back Squat",
      DifficultyEnum.High,
      [MuscleGroupEnum.Quadriceps, MuscleGroupEnum.Glutes, MuscleGroupEnum.Hamstrings, MuscleGroupEnum.Core]
    ),
    ("Front Squat", DifficultyEnum.High, [MuscleGroupEnum.Quadriceps, MuscleGroupEnum.Core, MuscleGroupEnum.Glutes]),
    (
      "Romanian Deadlift",
      DifficultyEnum.Medium,
      [MuscleGroupEnum.Hamstrings, MuscleGroupEnum.Glutes, MuscleGroupEnum.LowerBack]
    ),
    ("Leg Press", DifficultyEnum.Medium, [MuscleGroupEnum.Quadriceps, MuscleGroupEnum.Glutes]),
    ("Leg Curl", DifficultyEnum.Easy, [MuscleGroupEnum.Hamstrings]),
    ("Leg Extension", DifficultyEnum.Easy, [MuscleGroupEnum.Quadriceps]),
    (
      "Bulgarian Split Squat",
      DifficultyEnum.High,
      [MuscleGroupEnum.Quadriceps, MuscleGroupEnum.Glutes, MuscleGroupEnum.Hamstrings]
    ),
    ("Hip Thrust", DifficultyEnum.Medium, [MuscleGroupEnum.Glutes, MuscleGroupEnum.Hamstrings]),
    (
      "Walking Lunge",
      DifficultyEnum.Medium,
      [MuscleGroupEnum.Quadriceps, MuscleGroupEnum.Glutes, MuscleGroupEnum.Hamstrings]
    ),
    ("Standing Calf Raise", DifficultyEnum.Easy, [MuscleGroupEnum.Calves]),
    ("Seated Calf Raise", DifficultyEnum.Easy, [MuscleGroupEnum.Calves]),
    // ── Trapezius ────────────────────────────────────────────────────────────
    ("Barbell Shrug", DifficultyEnum.Easy, [MuscleGroupEnum.Trapezius]),
    ("Dumbbell Shrug", DifficultyEnum.Easy, [MuscleGroupEnum.Trapezius]),
    // ── Full Body / Compound ─────────────────────────────────────────────────
    (
      "Power Clean",
      DifficultyEnum.VeryHigh,
      [MuscleGroupEnum.Trapezius, MuscleGroupEnum.Glutes, MuscleGroupEnum.Hamstrings, MuscleGroupEnum.Shoulders]
    ),
    (
      "Kettlebell Swing",
      DifficultyEnum.Medium,
      [MuscleGroupEnum.Glutes, MuscleGroupEnum.Hamstrings, MuscleGroupEnum.Core, MuscleGroupEnum.LowerBack]
    ),
    (
      "Farmer's Walk",
      DifficultyEnum.Medium,
      [MuscleGroupEnum.Forearms, MuscleGroupEnum.Trapezius, MuscleGroupEnum.Core]
    ),
  ];

  public static void Seed(ExerciseCatalogDbContext context)
  {
    var existingNames = context.Exercises.Where(e => e.Creator == System).Select(e => e.Name).ToHashSet();
    var toAdd = BuildExercises(existingNames);
    if (toAdd.Count == 0)
      return;
    context.Exercises.AddRange(toAdd);
    context.SaveChanges();
  }

  public static async Task SeedAsync(ExerciseCatalogDbContext context, CancellationToken ct = default)
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
        exercise.MuscleGroups = e.MuscleGroups.ToHashSet();
        return exercise;
      })
      .ToList();
}
