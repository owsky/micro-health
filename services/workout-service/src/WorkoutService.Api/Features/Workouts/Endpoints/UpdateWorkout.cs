using Microsoft.AspNetCore.Http.HttpResults;
using WorkoutService.Common.Auth;
using WorkoutService.Features.Workouts.Dto;
using WorkoutService.Features.Workouts.Services;

namespace WorkoutService.Features.Workouts.Endpoints;

public class UpdateWorkout : WorkoutsEndpointBase
{
  protected override void Map(RouteGroupBuilder builder)
  {
    builder
      .MapPut(
        "/{id:long}",
        async Task<NoContent> (UpdateWorkoutRequest request, long id, UserInfo userInfo, IWorkoutsService service) =>
        {
          await service.UpdateWorkoutById(id, request, userInfo);
          return TypedResults.NoContent();
        }
      )
      .WithName("UpdateWorkoutById")
      .WithSummary("Update the workout by ID");
  }
}
