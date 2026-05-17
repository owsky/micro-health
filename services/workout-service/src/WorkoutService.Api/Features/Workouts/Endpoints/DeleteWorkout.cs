using Microsoft.AspNetCore.Http.HttpResults;
using WorkoutService.Common.Auth;
using WorkoutService.Features.Workouts.Services;

namespace WorkoutService.Features.Workouts.Endpoints;

public class DeleteWorkout : WorkoutsEndpointBase
{
  protected override void Map(RouteGroupBuilder builder)
  {
    builder
      .MapDelete(
        "/{id:long}",
        async Task<NoContent> (long id, UserInfo userInfo, IWorkoutsService service) =>
        {
          await service.DeleteWorkoutById(id, userInfo);
          return TypedResults.NoContent();
        }
      )
      .WithName("DeleteWorkoutById")
      .WithSummary("Delete the workout by ID");
  }
}
