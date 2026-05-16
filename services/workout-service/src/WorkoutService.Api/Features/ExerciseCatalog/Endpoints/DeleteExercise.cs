using Microsoft.AspNetCore.Http.HttpResults;
using WorkoutService.Common.Auth;
using WorkoutService.Features.ExerciseCatalog.Services;

namespace WorkoutService.Features.ExerciseCatalog.Endpoints;

public class DeleteExercise : ExerciseCatalogEndpointBase
{
  protected override void Map(RouteGroupBuilder group)
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
  }
}
