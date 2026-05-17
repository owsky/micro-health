namespace WorkoutService.Features.Workouts.Domain;

public class Workout
{
  public long Id { get; init; }

  public required string Creator { get; set; }

  public string? Name { get; set; }

  public required DateTime StartedAt { get; set; }

  public required DateTime CompletedAt { get; set; }

  public string? Note { get; set; }

  public required ICollection<WorkoutSet> WorkoutSets { get; set; }
}
