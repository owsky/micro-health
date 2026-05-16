using Microsoft.AspNetCore.Http.HttpResults;
using WorkoutService.Common.Auth;
using WorkoutService.Features.ExerciseCatalog.Dtos;
using WorkoutService.Features.ExerciseCatalog.Services;

namespace WorkoutService.Features.ExerciseCatalog.Endpoints;

public static class GetAllExercises
{
  extension(RouteGroupBuilder group)
  {
    public RouteGroupBuilder MapGetAllExercises()
    {
      group
        .MapGet(
          "",
          async Task<Ok<List<ExerciseResponse>>> (
            IExerciseService exerciseService,
            UserInfo userInfo,
            int pageSize = 15,
            int pageNumber = 1,
            bool mine = false
          ) =>
          {
            var exercises = await exerciseService.GetAllExercises(pageSize, pageNumber, mine, userInfo);
            return TypedResults.Ok(exercises);
          }
        )
        .WithName("GetAllExercises")
        .WithSummary("Get all exercises, paginated");

      return group;
    }
  }
}
