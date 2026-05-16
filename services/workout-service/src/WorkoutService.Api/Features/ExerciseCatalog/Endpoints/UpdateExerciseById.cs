using Microsoft.AspNetCore.Http.HttpResults;
using WorkoutService.Common.Auth;
using WorkoutService.Features.ExerciseCatalog.Dtos;
using WorkoutService.Features.ExerciseCatalog.Services;

namespace WorkoutService.Features.ExerciseCatalog.Endpoints;

public static class UpdateExerciseById
{
  extension(RouteGroupBuilder group)
  {
    public RouteGroupBuilder MapUpdateExerciseById()
    {
      group
        .MapPut(
          "{id:long}",
          async Task<NoContent> (long id, UpdateExerciseRequest request, IExerciseService service, UserInfo userInfo) =>
          {
            await service.UpdateExerciseById(id, request, userInfo);
            return TypedResults.NoContent();
          }
        )
        .WithName("UpdateExerciseById")
        .WithSummary("Update the exercise by ID");

      return group;
    }
  }
}
