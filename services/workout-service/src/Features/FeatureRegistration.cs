namespace WorkoutService.Features;

public static class FeatureRegistration
{
  extension(IEndpointRouteBuilder routes)
  {
    public IEndpointRouteBuilder MapFeatureEndpoints()
    {
      var api = routes.MapGroup("/api");

      api.MapExerciseCatalogEndpoints();
      api.MapWorkoutTemplatesEndpoints();

      return routes;
    }
  }
}
