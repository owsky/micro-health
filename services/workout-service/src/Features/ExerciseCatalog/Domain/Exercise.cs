using WorkoutService.Features.ExerciseCatalog.Enums;

namespace WorkoutService.Features.ExerciseCatalog.Domain;

public class Exercise
{
  public long Id { get; init; }

  public required string Creator { get; init; }

  public required string Name { get; set; }

  public required DifficultyEnum Difficulty { get; set; }

  // EF persists this navigation to the side table.
  public ICollection<ExerciseMuscleGroup> MuscleGroupLinks { get; } = new List<ExerciseMuscleGroup>();

  public ISet<MuscleGroupEnum> MuscleGroups
  {
    get => MuscleGroupLinks.Select(x => x.MuscleGroup).ToHashSet();
    set
    {
      MuscleGroupLinks.Clear();

      foreach (var group in value.Distinct())
      {
        MuscleGroupLinks.Add(new ExerciseMuscleGroup { MuscleGroup = group, Exercise = this });
      }
    }
  }
}
