// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Routing;

public static class FeatureRegistration
{
  extension(IEndpointRouteBuilder routes)
  {
    public IEndpointRouteBuilder MapFeatureEndpoints()
    {
      var api = routes.MapGroup("/api");

      api.MapExerciseCatalogEndpoints();

      return routes;
    }
  }
}
