namespace WorkoutService.Features.Workouts.Domain;

public class DistanceAndTimeSet : WorkoutSet
{
  public required decimal Distance { get; set; }

  public required int DurationSeconds { get; set; }
}
