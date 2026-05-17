using Microsoft.AspNetCore.Http.HttpResults;
using WorkoutService.Common.Auth;
using WorkoutService.Features.Workouts.Dto;
using WorkoutService.Features.Workouts.Services;

namespace WorkoutService.Features.Workouts.Endpoints;

public class GetAllWorkouts : WorkoutsEndpointBase
{
  protected override void Map(RouteGroupBuilder builder)
  {
    builder
      .MapGet(
        "",
        async Task<Ok<List<WorkoutResponse>>> (UserInfo userInfo, IWorkoutsService service) =>
        {
          var workouts = await service.GetAllWorkouts(userInfo);
          return TypedResults.Ok(workouts);
        }
      )
      .WithName("GetAllWorkouts")
      .WithSummary("Get all workouts created by the user");
  }
}
