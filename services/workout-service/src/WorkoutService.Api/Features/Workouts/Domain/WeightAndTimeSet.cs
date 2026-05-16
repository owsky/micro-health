namespace WorkoutService.Features.Workouts.Domain;

public class WeightAndTimeSet : WorkoutSet
{
  public required decimal Weight { get; set; }

  public required int DurationSeconds { get; set; }
}
