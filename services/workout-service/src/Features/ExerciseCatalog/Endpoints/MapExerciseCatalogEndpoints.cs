using WorkoutService.Features.ExerciseCatalog.Endpoints;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Routing;

public static class ExerciseCatalogEndpoints
{
  extension(RouteGroupBuilder api)
  {
    public RouteGroupBuilder MapExerciseCatalogEndpoints()
    {
      var group = api.MapGroup("/exercise-catalog").WithTags("ExerciseCatalog");

      group.MapGetExerciseById();
      group.MapGetAllExercises();
      group.MapUpdateExerciseById();
      group.MapCreateExercise();
      group.MapDeleteExercise();

      return api;
    }
  }
}
