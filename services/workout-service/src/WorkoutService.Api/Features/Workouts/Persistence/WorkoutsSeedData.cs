using Microsoft.EntityFrameworkCore;
using WorkoutService.Features.Workouts.Domain;
using WorkoutService.Infrastructure;

namespace WorkoutService.Features.Workouts.Persistence;

public static class WorkoutsSeedData
{
  private const string System = "system";

  // (WorkoutName, Note, DaysAgo, DurationMinutes, exercises: (name, sets: (setNumber, isWarmup, ...values)))
  private static readonly IReadOnlyList<WorkoutDefinition> Workouts =
  [
    new WorkoutDefinition(
      "Push Day A",
      "Chest, shoulders, triceps focus",
      14,
      75,
      [
        new(
          "Barbell Bench Press",
          [
            new WeightRepsSetDef(1, true, 40m, 12),
            new WeightRepsSetDef(2, false, 80m, 8),
            new WeightRepsSetDef(3, false, 80m, 7),
            new WeightRepsSetDef(4, false, 82.5m, 6),
          ]
        ),
        new(
          "Incline Dumbbell Press",
          [
            new WeightRepsSetDef(1, false, 30m, 10),
            new WeightRepsSetDef(2, false, 30m, 9),
            new WeightRepsSetDef(3, false, 32m, 8),
          ]
        ),
        new(
          "Overhead Barbell Press",
          [
            new WeightRepsSetDef(1, true, 30m, 10),
            new WeightRepsSetDef(2, false, 55m, 8),
            new WeightRepsSetDef(3, false, 57.5m, 7),
          ]
        ),
        new(
          "Tricep Rope Pushdown",
          [
            new WeightRepsSetDef(1, false, 20m, 15),
            new WeightRepsSetDef(2, false, 22.5m, 12),
            new WeightRepsSetDef(3, false, 22.5m, 11),
          ]
        ),
        new(
          "Cable Fly",
          [
            new WeightRepsSetDef(1, false, 12.5m, 15),
            new WeightRepsSetDef(2, false, 12.5m, 13),
            new WeightRepsSetDef(3, false, 15m, 10),
          ]
        ),
      ]
    ),
    new WorkoutDefinition(
      "Pull Day A",
      "Back and biceps",
      12,
      80,
      [
        new(
          "Deadlift",
          [
            new WeightRepsSetDef(1, true, 60m, 10),
            new WeightRepsSetDef(2, true, 100m, 5),
            new WeightRepsSetDef(3, false, 140m, 5),
            new WeightRepsSetDef(4, false, 150m, 4),
            new WeightRepsSetDef(5, false, 155m, 3),
          ]
        ),
        new(
          "Pull-Up",
          [
            new WeightRepsSetDef(1, false, 0m, 10),
            new WeightRepsSetDef(2, false, 0m, 9),
            new WeightRepsSetDef(3, false, 0m, 8),
          ]
        ),
        new(
          "Seated Cable Row",
          [
            new WeightRepsSetDef(1, false, 60m, 12),
            new WeightRepsSetDef(2, false, 65m, 10),
            new WeightRepsSetDef(3, false, 65m, 9),
          ]
        ),
        new(
          "Barbell Curl",
          [
            new WeightRepsSetDef(1, false, 30m, 12),
            new WeightRepsSetDef(2, false, 35m, 10),
            new WeightRepsSetDef(3, false, 37.5m, 8),
          ]
        ),
        new(
          "Hammer Curl",
          [
            new WeightRepsSetDef(1, false, 16m, 12),
            new WeightRepsSetDef(2, false, 18m, 10),
            new WeightRepsSetDef(3, false, 18m, 9),
          ]
        ),
      ]
    ),
    new WorkoutDefinition(
      "Leg Day A",
      "Quad dominant lower body session",
      10,
      90,
      [
        new(
          "Barbell Back Squat",
          [
            new WeightRepsSetDef(1, true, 60m, 10),
            new WeightRepsSetDef(2, true, 100m, 5),
            new WeightRepsSetDef(3, false, 120m, 5),
            new WeightRepsSetDef(4, false, 130m, 4),
            new WeightRepsSetDef(5, false, 130m, 4),
          ]
        ),
        new(
          "Romanian Deadlift",
          [
            new WeightRepsSetDef(1, false, 80m, 10),
            new WeightRepsSetDef(2, false, 90m, 9),
            new WeightRepsSetDef(3, false, 90m, 8),
          ]
        ),
        new(
          "Leg Press",
          [
            new WeightRepsSetDef(1, false, 150m, 12),
            new WeightRepsSetDef(2, false, 180m, 10),
            new WeightRepsSetDef(3, false, 200m, 8),
          ]
        ),
        new(
          "Leg Curl",
          [
            new WeightRepsSetDef(1, false, 40m, 15),
            new WeightRepsSetDef(2, false, 45m, 12),
            new WeightRepsSetDef(3, false, 50m, 10),
          ]
        ),
        new(
          "Standing Calf Raise",
          [
            new WeightRepsSetDef(1, false, 50m, 20),
            new WeightRepsSetDef(2, false, 60m, 18),
            new WeightRepsSetDef(3, false, 70m, 15),
          ]
        ),
      ]
    ),
    new WorkoutDefinition(
      "Push Day B",
      "Volume push session",
      7,
      70,
      [
        new(
          "Incline Dumbbell Press",
          [
            new WeightRepsSetDef(1, true, 20m, 12),
            new WeightRepsSetDef(2, false, 34m, 9),
            new WeightRepsSetDef(3, false, 34m, 8),
            new WeightRepsSetDef(4, false, 36m, 7),
          ]
        ),
        new(
          "Cable Fly",
          [
            new WeightRepsSetDef(1, false, 14m, 15),
            new WeightRepsSetDef(2, false, 14m, 14),
            new WeightRepsSetDef(3, false, 16m, 12),
          ]
        ),
        new(
          "Arnold Press",
          [
            new WeightRepsSetDef(1, false, 20m, 12),
            new WeightRepsSetDef(2, false, 22m, 10),
            new WeightRepsSetDef(3, false, 22m, 9),
          ]
        ),
        new(
          "Skull Crusher",
          [
            new WeightRepsSetDef(1, false, 30m, 12),
            new WeightRepsSetDef(2, false, 35m, 10),
            new WeightRepsSetDef(3, false, 37.5m, 8),
          ]
        ),
        new(
          "Dumbbell Lateral Raise",
          [
            new WeightRepsSetDef(1, false, 10m, 15),
            new WeightRepsSetDef(2, false, 12m, 12),
            new WeightRepsSetDef(3, false, 12m, 12),
          ]
        ),
      ]
    ),
    new WorkoutDefinition(
      "Pull Day B",
      "Back width focus",
      5,
      65,
      [
        new(
          "Lat Pulldown",
          [
            new WeightRepsSetDef(1, true, 40m, 12),
            new WeightRepsSetDef(2, false, 70m, 10),
            new WeightRepsSetDef(3, false, 75m, 9),
            new WeightRepsSetDef(4, false, 75m, 8),
          ]
        ),
        new(
          "T-Bar Row",
          [
            new WeightRepsSetDef(1, false, 60m, 10),
            new WeightRepsSetDef(2, false, 70m, 9),
            new WeightRepsSetDef(3, false, 70m, 8),
          ]
        ),
        new(
          "Barbell Row",
          [
            new WeightRepsSetDef(1, false, 80m, 10),
            new WeightRepsSetDef(2, false, 85m, 8),
            new WeightRepsSetDef(3, false, 85m, 8),
          ]
        ),
        new(
          "Incline Dumbbell Curl",
          [
            new WeightRepsSetDef(1, false, 14m, 12),
            new WeightRepsSetDef(2, false, 16m, 10),
            new WeightRepsSetDef(3, false, 16m, 9),
          ]
        ),
        new(
          "Face Pull",
          [
            new WeightRepsSetDef(1, false, 20m, 15),
            new WeightRepsSetDef(2, false, 22.5m, 15),
            new WeightRepsSetDef(3, false, 25m, 12),
          ]
        ),
      ]
    ),
    new WorkoutDefinition(
      "Leg Day B",
      "Posterior chain emphasis",
      3,
      85,
      [
        new(
          "Romanian Deadlift",
          [
            new WeightRepsSetDef(1, true, 60m, 10),
            new WeightRepsSetDef(2, false, 100m, 8),
            new WeightRepsSetDef(3, false, 110m, 7),
            new WeightRepsSetDef(4, false, 110m, 6),
          ]
        ),
        new(
          "Bulgarian Split Squat",
          [
            new WeightRepsSetDef(1, false, 20m, 10),
            new WeightRepsSetDef(2, false, 24m, 9),
            new WeightRepsSetDef(3, false, 24m, 8),
          ]
        ),
        new(
          "Hip Thrust",
          [
            new WeightRepsSetDef(1, false, 100m, 12),
            new WeightRepsSetDef(2, false, 110m, 10),
            new WeightRepsSetDef(3, false, 120m, 9),
          ]
        ),
        new(
          "Leg Extension",
          [
            new WeightRepsSetDef(1, false, 40m, 15),
            new WeightRepsSetDef(2, false, 45m, 12),
            new WeightRepsSetDef(3, false, 50m, 10),
          ]
        ),
        new(
          "Seated Calf Raise",
          [
            new WeightRepsSetDef(1, false, 40m, 20),
            new WeightRepsSetDef(2, false, 50m, 18),
            new WeightRepsSetDef(3, false, 50m, 15),
          ]
        ),
      ]
    ),
    new WorkoutDefinition(
      "Cardio & Core",
      "Active recovery — light cardio and core work",
      1,
      45,
      [
        new("Treadmill", [new DistanceTimeSetDef(1, false, 5.0m, 1800)]),
        new("Plank", [new TimeSetDef(1, false, 60), new TimeSetDef(2, false, 60), new TimeSetDef(3, false, 45)]),
        new ExerciseDefinition(
          "Crunch",
          [new RepsSetDef(1, false, 20), new RepsSetDef(2, false, 20), new RepsSetDef(3, false, 15)]
        ),
        new ExerciseDefinition(
          "Hanging Leg Raise",
          [new RepsSetDef(1, false, 15), new RepsSetDef(2, false, 12), new RepsSetDef(3, false, 10)]
        ),
      ]
    ),
  ];

  public static void Seed(WorkoutServiceDbContext context)
  {
    var existingNames = context
      .Workouts.Where(w => w.Creator == System && w.Name != null)
      .Select(w => w.Name!)
      .ToHashSet();

    var exerciseNames = Workouts.SelectMany(w => w.Exercises.Select(e => e.ExerciseName)).Distinct().ToList();

    var exerciseMap = context.Exercises.Where(e => exerciseNames.Contains(e.Name)).ToDictionary(e => e.Name);

    var toAdd = BuildWorkouts(existingNames, exerciseMap);
    if (toAdd.Count == 0)
      return;

    context.Workouts.AddRange(toAdd);
    context.SaveChanges();
  }

  public static async Task SeedAsync(WorkoutServiceDbContext context, CancellationToken ct = default)
  {
    var existingNames = (
      await context.Workouts.Where(w => w.Creator == System && w.Name != null).Select(w => w.Name!).ToListAsync(ct)
    ).ToHashSet();

    var exerciseNames = Workouts.SelectMany(w => w.Exercises.Select(e => e.ExerciseName)).Distinct().ToList();

    var exerciseMap = await context
      .Exercises.Where(e => exerciseNames.Contains(e.Name))
      .ToDictionaryAsync(e => e.Name, ct);

    var toAdd = BuildWorkouts(existingNames, exerciseMap);
    if (toAdd.Count == 0)
      return;

    context.Workouts.AddRange(toAdd);
    await context.SaveChangesAsync(ct);
  }

  private static List<Workout> BuildWorkouts(
    HashSet<string> existingNames,
    Dictionary<string, ExerciseCatalog.Domain.Exercise> exerciseMap
  )
  {
    var now = DateTime.UtcNow;
    var toAdd = new List<Workout>();

    foreach (var def in Workouts)
    {
      if (existingNames.Contains(def.Name))
        continue;

      var started = now.AddDays(-def.DaysAgo).Date.AddHours(7);
      var completed = started.AddMinutes(def.DurationMinutes);

      var sets = new List<WorkoutSet>();
      foreach (var exerciseDef in def.Exercises)
      {
        if (!exerciseMap.TryGetValue(exerciseDef.ExerciseName, out var exercise))
          continue;

        foreach (var setDef in exerciseDef.Sets)
        {
          WorkoutSet set = setDef switch
          {
            WeightRepsSetDef wr => new WeightAndRepsSet
            {
              ExerciseId = exercise.Id,
              WorkoutId = 0,
              SetNumber = wr.SetNumber,
              IsWarmup = wr.IsWarmup,
              Weight = wr.Weight,
              Reps = wr.Reps,
            },
            RepsSetDef r => new RepsSet
            {
              ExerciseId = exercise.Id,
              WorkoutId = 0,
              SetNumber = r.SetNumber,
              IsWarmup = r.IsWarmup,
              Reps = r.Reps,
            },
            TimeSetDef t => new TimeSet
            {
              ExerciseId = exercise.Id,
              WorkoutId = 0,
              SetNumber = t.SetNumber,
              IsWarmup = t.IsWarmup,
              DurationSeconds = t.DurationSeconds,
            },
            DistanceTimeSetDef dt => new DistanceAndTimeSet
            {
              ExerciseId = exercise.Id,
              WorkoutId = 0,
              SetNumber = dt.SetNumber,
              IsWarmup = dt.IsWarmup,
              Distance = dt.Distance,
              DurationSeconds = dt.DurationSeconds,
            },
            WeightTimeSetDef wt => new WeightAndTimeSet
            {
              ExerciseId = exercise.Id,
              WorkoutId = 0,
              SetNumber = wt.SetNumber,
              IsWarmup = wt.IsWarmup,
              Weight = wt.Weight,
              DurationSeconds = wt.DurationSeconds,
            },
            _ => throw new InvalidOperationException($"Unknown set definition type: {setDef.GetType().Name}"),
          };
          sets.Add(set);
        }
      }

      toAdd.Add(
        new Workout
        {
          Creator = System,
          Name = def.Name,
          Note = def.Note,
          StartedAt = started,
          CompletedAt = completed,
          WorkoutSets = sets,
        }
      );
    }

    return toAdd;
  }

  // ── Definition helpers ───────────────────────────────────────────────────────

  private record WorkoutDefinition(
    string Name,
    string? Note,
    int DaysAgo,
    int DurationMinutes,
    IReadOnlyList<ExerciseDefinition> Exercises
  );

  private record ExerciseDefinition(string ExerciseName, IReadOnlyList<SetDefinition> Sets);

  private abstract record SetDefinition(int SetNumber, bool IsWarmup);

  private record WeightRepsSetDef(int SetNumber, bool IsWarmup, decimal Weight, int Reps)
    : SetDefinition(SetNumber, IsWarmup);

  private record RepsSetDef(int SetNumber, bool IsWarmup, int Reps) : SetDefinition(SetNumber, IsWarmup);

  private record TimeSetDef(int SetNumber, bool IsWarmup, int DurationSeconds) : SetDefinition(SetNumber, IsWarmup);

  private record DistanceTimeSetDef(int SetNumber, bool IsWarmup, decimal Distance, int DurationSeconds)
    : SetDefinition(SetNumber, IsWarmup);

  private record WeightTimeSetDef(int SetNumber, bool IsWarmup, decimal Weight, int DurationSeconds)
    : SetDefinition(SetNumber, IsWarmup);
}
