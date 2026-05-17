using Microsoft.AspNetCore.Http.HttpResults;
using WorkoutService.Common.Auth;
using WorkoutService.Features.Workouts.Dto;
using WorkoutService.Features.Workouts.Services;

namespace WorkoutService.Features.Workouts.Endpoints;

public class GetWorkoutById : WorkoutsEndpointBase
{
  protected override void Map(RouteGroupBuilder builder)
  {
    builder
      .MapGet(
        "/{id:long}",
        async Task<Ok<WorkoutResponse>> (long id, UserInfo userInfo, IWorkoutsService service) =>
        {
          var workout = await service.GetWorkoutById(id, userInfo);
          return TypedResults.Ok(workout);
        }
      )
      .WithName("GetWorkoutById")
      .WithSummary("Get workout by ID");
  }
}
