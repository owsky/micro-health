using Microsoft.AspNetCore.Http.HttpResults;
using WorkoutService.Features.ExerciseCatalog.Dtos;
using WorkoutService.Features.ExerciseCatalog.Services;

namespace WorkoutService.Features.ExerciseCatalog.Endpoints;

public class GetExerciseById : ExerciseCatalogEndpointBase
{
  protected override void Map(RouteGroupBuilder group)
  {
    group
      .MapGet(
        "{id:long}",
        async Task<Results<Ok<ExerciseResponse>, NotFound>> (long id, IExerciseService exerciseService) =>
        {
          var exercise = await exerciseService.GetExerciseById(id);
          if (exercise is null)
            return TypedResults.NotFound();
          return TypedResults.Ok(exercise);
        }
      )
      .WithName("GetExerciseById")
      .WithSummary("Get exercise by ID");
  }
}
