using Microsoft.EntityFrameworkCore;
using WorkoutService.Common.Enums;
using WorkoutService.Features.WorkoutTemplates.Domain;
using WorkoutService.Infrastructure;

namespace WorkoutService.Features.WorkoutTemplates.Persistence;

public static class WorkoutTemplatesSeedData
{
  private const string System = "system";

  private static readonly IReadOnlyList<(string Name, Difficulty Difficulty, string[] Exercises)> Templates =
  [
    // ── Push / Pull / Legs ───────────────────────────────────────────────────
    (
      "PPL — Push",
      Difficulty.Medium,
      [
        "Barbell Bench Press",
        "Incline Dumbbell Press",
        "Cable Fly",
        "Overhead Barbell Press",
        "Dumbbell Lateral Raise",
        "Tricep Rope Pushdown",
        "Skull Crusher",
      ]
    ),
    (
      "PPL — Pull",
      Difficulty.Medium,
      [
        "Deadlift",
        "Pull-Up",
        "Barbell Row",
        "Lat Pulldown",
        "Seated Cable Row",
        "Face Pull",
        "Barbell Curl",
        "Hammer Curl",
      ]
    ),
    (
      "PPL — Legs",
      Difficulty.High,
      [
        "Barbell Back Squat",
        "Romanian Deadlift",
        "Leg Press",
        "Bulgarian Split Squat",
        "Leg Curl",
        "Leg Extension",
        "Standing Calf Raise",
      ]
    ),
    // ── Upper / Lower ────────────────────────────────────────────────────────
    (
      "Upper Body",
      Difficulty.Medium,
      [
        "Barbell Bench Press",
        "Barbell Row",
        "Overhead Barbell Press",
        "Pull-Up",
        "Dumbbell Lateral Raise",
        "Barbell Curl",
        "Tricep Rope Pushdown",
      ]
    ),
    (
      "Lower Body",
      Difficulty.High,
      [
        "Barbell Back Squat",
        "Romanian Deadlift",
        "Leg Press",
        "Hip Thrust",
        "Leg Curl",
        "Standing Calf Raise",
        "Seated Calf Raise",
      ]
    ),
    // ── Full Body ────────────────────────────────────────────────────────────
    (
      "Full Body Strength",
      Difficulty.High,
      [
        "Barbell Back Squat",
        "Deadlift",
        "Barbell Bench Press",
        "Pull-Up",
        "Overhead Barbell Press",
        "Barbell Curl",
        "Tricep Rope Pushdown",
      ]
    ),
    (
      "Full Body Hypertrophy",
      Difficulty.Medium,
      [
        "Leg Press",
        "Romanian Deadlift",
        "Incline Dumbbell Press",
        "Seated Cable Row",
        "Arnold Press",
        "Hammer Curl",
        "Overhead Tricep Extension",
        "Standing Calf Raise",
      ]
    ),
    // ── Specialised ─────────────────────────────────────────────────────────
    (
      "Chest & Triceps",
      Difficulty.Medium,
      [
        "Barbell Bench Press",
        "Incline Dumbbell Press",
        "Cable Fly",
        "Dips",
        "Close-Grip Bench Press",
        "Skull Crusher",
        "Tricep Rope Pushdown",
        "Overhead Tricep Extension",
      ]
    ),
    (
      "Back & Biceps",
      Difficulty.Medium,
      [
        "Deadlift",
        "Pull-Up",
        "Barbell Row",
        "T-Bar Row",
        "Lat Pulldown",
        "Seated Cable Row",
        "Barbell Curl",
        "Incline Dumbbell Curl",
        "Preacher Curl",
      ]
    ),
    (
      "Shoulders & Arms",
      Difficulty.Easy,
      [
        "Overhead Barbell Press",
        "Dumbbell Lateral Raise",
        "Arnold Press",
        "Face Pull",
        "Barbell Curl",
        "Hammer Curl",
        "Skull Crusher",
        "Tricep Rope Pushdown",
      ]
    ),
    (
      "Posterior Chain",
      Difficulty.High,
      ["Deadlift", "Romanian Deadlift", "Hip Thrust", "Bulgarian Split Squat", "Leg Curl", "Barbell Row", "Face Pull"]
    ),
    (
      "Cardio & Core",
      Difficulty.Easy,
      ["Treadmill", "Plank", "Crunch", "Hanging Leg Raise", "Ab Wheel Rollout", "Russian Twist", "Jump Rope"]
    ),
  ];

  public static void Seed(WorkoutServiceDbContext context)
  {
    var existingNames = context.WorkoutTemplates.Where(t => t.Creator == System).Select(t => t.Name).ToHashSet();

    var exerciseNames = Templates.SelectMany(t => t.Exercises).Distinct().ToList();
    var exerciseMap = context.Exercises.Where(e => exerciseNames.Contains(e.Name)).ToDictionary(e => e.Name);

    var toAdd = BuildTemplates(existingNames, exerciseMap);
    if (toAdd.Count == 0)
      return;

    context.WorkoutTemplates.AddRange(toAdd);
    context.SaveChanges();
  }

  public static async Task SeedAsync(WorkoutServiceDbContext context, CancellationToken ct = default)
  {
    var existingNames = (
      await context.WorkoutTemplates.Where(t => t.Creator == System).Select(t => t.Name).ToListAsync(ct)
    ).ToHashSet();

    var exerciseNames = Templates.SelectMany(t => t.Exercises).Distinct().ToList();
    var exerciseMap = await context
      .Exercises.Where(e => exerciseNames.Contains(e.Name))
      .ToDictionaryAsync(e => e.Name, ct);

    var toAdd = BuildTemplates(existingNames, exerciseMap);
    if (toAdd.Count == 0)
      return;

    context.WorkoutTemplates.AddRange(toAdd);
    await context.SaveChangesAsync(ct);
  }

  private static List<WorkoutTemplate> BuildTemplates(
    HashSet<string> existingNames,
    Dictionary<string, ExerciseCatalog.Domain.Exercise> exerciseMap
  )
  {
    var toAdd = new List<WorkoutTemplate>();

    foreach (var (name, difficulty, exercises) in Templates)
    {
      if (existingNames.Contains(name))
        continue;

      var template = new WorkoutTemplate
      {
        Creator = System,
        Name = name,
        OverallDifficulty = difficulty,
      };

      var order = 1;
      foreach (var exerciseName in exercises)
      {
        if (!exerciseMap.TryGetValue(exerciseName, out var exercise))
          continue;

        template.Exercises.Add(
          new WorkoutTemplateExercise
          {
            WorkoutTemplateId = 0,
            ExerciseId = exercise.Id,
            Order = order++,
          }
        );
      }

      toAdd.Add(template);
    }

    return toAdd;
  }
}
