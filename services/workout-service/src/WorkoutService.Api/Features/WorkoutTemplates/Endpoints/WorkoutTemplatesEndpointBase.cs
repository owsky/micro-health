using WorkoutService.Common.Abstractions;

namespace WorkoutService.Features.WorkoutTemplates.Endpoints;

public abstract class WorkoutTemplatesEndpointBase : EndpointBase, IEndpoint
{
  public void MapEndpoint(RouteGroupBuilder builder)
  {
    var group = builder.MapGroup("/workout-templates").WithTags("WorkoutTemplates");

    Map(group);
  }
}
