using Microsoft.AspNetCore.Http.HttpResults;
using WorkoutService.Common.Auth;
using WorkoutService.Features.ExerciseCatalog.Dtos;
using WorkoutService.Features.ExerciseCatalog.Services;

namespace WorkoutService.Features.ExerciseCatalog.Endpoints;

public class CreateExercise : ExerciseCatalogEndpointBase
{
  protected override void Map(RouteGroupBuilder group)
  {
    group
      .MapPost(
        "",
        async Task<Created<ExerciseResponse>> (
          CreateExerciseRequest request,
          UserInfo userInfo,
          IExerciseService service,
          LinkGenerator linkGenerator
        ) =>
        {
          var created = await service.CreateExercise(request, userInfo);
          var uri = linkGenerator.GetPathByName("GetExerciseById", new { id = created.Id });
          return TypedResults.Created(uri, created);
        }
      )
      .WithName("CreateExercise")
      .WithSummary("Create a new exercise");
  }
}
