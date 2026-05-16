namespace WorkoutService.Common.Abstractions;

public interface IEndpoint
{
  void MapEndpoint(RouteGroupBuilder builder);
}
