namespace WorkoutService.Common.Abstractions;

public abstract class EndpointBase
{
  protected abstract void Map(RouteGroupBuilder builder);
}
