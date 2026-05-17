namespace WorkoutService.Features.Workouts.Dto;

public class WorkoutResponse
{
  public long Id { get; init; }

  public required string Creator { get; init; }

  public string? Name { get; init; }

  public required DateTime StartedAt { get; init; }

  public required DateTime CompletedAt { get; init; }

  public string? Note { get; init; }

  public required IReadOnlyList<CompletedSetResponse> Sets { get; init; }
}
