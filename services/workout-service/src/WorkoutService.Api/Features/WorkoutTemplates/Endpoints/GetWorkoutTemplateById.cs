using Microsoft.AspNetCore.Http.HttpResults;
using WorkoutService.Features.WorkoutTemplates.Dtos;
using WorkoutService.Features.WorkoutTemplates.Services;

namespace WorkoutService.Features.WorkoutTemplates.Endpoints;

public class GetWorkoutTemplateById : WorkoutTemplatesEndpointBase
{
  protected override void Map(RouteGroupBuilder group)
  {
    group
      .MapGet(
        "{id:long}",
        async Task<Results<Ok<WorkoutTemplateResponse>, NotFound>> (long id, IWorkoutTemplatesService service) =>
        {
          var template = await service.GetWorkoutTemplateById(id);
          if (template == null)
            return TypedResults.NotFound();
          return TypedResults.Ok(template);
        }
      )
      .WithName("GetWorkoutTemplateById")
      .WithSummary("Get the workout template by ID");
  }
}
