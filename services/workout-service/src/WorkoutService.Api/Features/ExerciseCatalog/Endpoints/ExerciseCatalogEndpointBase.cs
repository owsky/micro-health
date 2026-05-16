using WorkoutService.Common.Interfaces;
using WorkoutService.Common.Utils;

namespace WorkoutService.Features.ExerciseCatalog.Endpoints;

public abstract class ExerciseCatalogEndpointBase : EndpointBase, IEndpoint
{
  public void MapEndpoint(RouteGroupBuilder app)
  {
    var group = app.MapGroup("/exercise-catalog").WithTags("ExerciseCatalog");

    Map(group);
  }
}
