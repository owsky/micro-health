namespace WorkoutService.Features.Workouts.Domain;

public class WeightAndRepsSet : WorkoutSet
{
  public required decimal Weight { get; set; }

  public required int Reps { get; set; }
}
