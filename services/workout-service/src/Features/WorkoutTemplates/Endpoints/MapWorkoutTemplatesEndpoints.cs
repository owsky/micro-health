using WorkoutService.Features.WorkoutTemplates.Endpoints;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Routing;

public static class WorkoutTemplatesEndpoints
{
  extension(RouteGroupBuilder api)
  {
    public RouteGroupBuilder MapWorkoutTemplatesEndpoints()
    {
      var group = api.MapGroup("/workout-templates").WithTags("WorkoutTemplates");

      group.MapCreateWorkoutTemplate();
      group.MapDeleteWorkoutTemplate();
      group.MapGetWorkoutTemplateById();
      group.MapUpdateWorkoutTemplate();

      return api;
    }
  }
}
