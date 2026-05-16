using Microsoft.AspNetCore.Http.HttpResults;
using WorkoutService.Common.Auth;
using WorkoutService.Features.WorkoutTemplates.Dtos;
using WorkoutService.Features.WorkoutTemplates.Services;

namespace WorkoutService.Features.WorkoutTemplates.Endpoints;

public static class CreateWorkoutTemplate
{
  extension(RouteGroupBuilder group)
  {
    public RouteGroupBuilder MapCreateWorkoutTemplate()
    {
      group
        .MapPost(
          "",
          async Task<Created<WorkoutTemplateResponse>> (
            CreateWorkoutTemplateRequest request,
            UserInfo userInfo,
            IWorkoutTemplatesService service,
            LinkGenerator linkGenerator
          ) =>
          {
            var created = await service.CreateWorkoutTemplate(request, userInfo);
            var uri = linkGenerator.GetPathByName("GetWorkoutTemplateById", new { id = created.Id });
            return TypedResults.Created(uri, created);
          }
        )
        .WithName("CreateWorkoutTemplate")
        .WithSummary("Create a new workout template");

      return group;
    }
  }
}
