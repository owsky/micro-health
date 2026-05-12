using Microsoft.AspNetCore.Http.HttpResults;
using WorkoutService.Common.Auth;
using WorkoutService.Features.ExerciseCatalog.Services;

namespace WorkoutService.Features.ExerciseCatalog.Endpoints;

public static class DeleteExercise
{
  extension(RouteGroupBuilder group)
  {
    public RouteGroupBuilder MapDeleteExercise()
    {
      group
        .MapDelete(
          "{id:long}",
          async Task<NoContent> (long id, IExerciseService service, UserInfo userInfo) =>
          {
            await service.DeleteExerciseById(id, userInfo);
            return TypedResults.NoContent();
          }
        )
        .WithName("DeleteExercise")
        .WithSummary("Delete an exercise by ID");

      return group;
    }
  }
}
