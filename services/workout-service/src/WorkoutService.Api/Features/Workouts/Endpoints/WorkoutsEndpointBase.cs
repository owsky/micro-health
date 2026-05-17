using WorkoutService.Common.Abstractions;

namespace WorkoutService.Features.Workouts.Endpoints;

public abstract class WorkoutsEndpointBase : EndpointBase, IEndpoint
{
  public void MapEndpoint(RouteGroupBuilder builder)
  {
    var group = builder.MapGroup("/workouts").WithTags("Workouts");

    Map(group);
  }
}
