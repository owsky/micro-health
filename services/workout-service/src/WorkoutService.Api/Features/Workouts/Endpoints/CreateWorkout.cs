using Microsoft.AspNetCore.Http.HttpResults;
using WorkoutService.Common.Auth;
using WorkoutService.Features.Workouts.Dto;
using WorkoutService.Features.Workouts.Services;

namespace WorkoutService.Features.Workouts.Endpoints;

public class CreateWorkout : WorkoutsEndpointBase
{
  protected override void Map(RouteGroupBuilder builder)
  {
    builder
      .MapPost(
        "",
        async Task<Created<WorkoutResponse>> (
          CreateWorkoutRequest request,
          UserInfo userInfo,
          IWorkoutsService service,
          LinkGenerator linkGenerator
        ) =>
        {
          var created = await service.CreateWorkout(request, userInfo);
          var uri = linkGenerator.GetPathByName("GetWorkoutById", new { id = created.Id });
          return TypedResults.Created(uri, created);
        }
      )
      .WithName("CreateWorkout")
      .WithSummary("Create a new workout");
  }
}
