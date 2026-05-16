using Microsoft.AspNetCore.Http.HttpResults;
using WorkoutService.Common.Auth;
using WorkoutService.Features.WorkoutTemplates.Services;

namespace WorkoutService.Features.WorkoutTemplates.Endpoints;

public static class DeleteWorkoutTemplate
{
  extension(RouteGroupBuilder group)
  {
    public RouteGroupBuilder MapDeleteWorkoutTemplate()
    {
      group
        .MapDelete(
          "{id:long}",
          async Task<NoContent> (long id, UserInfo userInfo, IWorkoutTemplatesService service) =>
          {
            await service.DeleteWorkoutTemplateById(id, userInfo);
            return TypedResults.NoContent();
          }
        )
        .WithName("DeleteWorkoutTemplate")
        .WithSummary("Delete the workout template by ID");

      return group;
    }
  }
}
