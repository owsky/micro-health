using Microsoft.AspNetCore.Http.HttpResults;
using WorkoutService.Common.Auth;
using WorkoutService.Features.WorkoutTemplates.Dtos;
using WorkoutService.Features.WorkoutTemplates.Services;

namespace WorkoutService.Features.WorkoutTemplates.Endpoints;

public static class UpdateWorkoutTemplate
{
  extension(RouteGroupBuilder group)
  {
    public RouteGroupBuilder MapUpdateWorkoutTemplate()
    {
      group
        .MapPut(
          "{id:long}",
          async Task<NoContent> (
            long id,
            UpdateWorkoutTemplateRequest request,
            UserInfo userInfo,
            IWorkoutTemplatesService service
          ) =>
          {
            await service.UpdateWorkoutTemplate(request, userInfo);
            return TypedResults.NoContent();
          }
        )
        .WithName("UpdateWorkoutTemplate")
        .WithSummary("Update the new workout template by ID");

      return group;
    }
  }
}
