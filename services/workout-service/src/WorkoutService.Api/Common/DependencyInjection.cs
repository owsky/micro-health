using Microsoft.EntityFrameworkCore;
using WorkoutService.Common.Abstractions;
using WorkoutService.Common.Auth;
using WorkoutService.Features.ExerciseCatalog.Persistence;
using WorkoutService.Features.ExerciseCatalog.Services;
using WorkoutService.Features.Workouts.Services;
using WorkoutService.Features.WorkoutTemplates.Persistence;
using WorkoutService.Features.WorkoutTemplates.Services;
using WorkoutService.Infrastructure;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for registering all Workout service dependencies
/// into the ASP.NET Core dependency injection container.
/// </summary>
public static class WorkoutServices
{
  extension(WebApplicationBuilder builder)
  {
    /// <summary>
    /// Registers infrastructure concerns: the EF Core <see cref="WorkoutServiceDbContext"/> backed by SQL Server,
    /// including synchronous and asynchronous seed data for the exercise catalog.
    /// Throws <see cref="InvalidOperationException"/> if the <c>SqlServer</c> connection string is missing.
    /// </summary>
    /// <returns>The same <see cref="WebApplicationBuilder"/> for chaining.</returns>
    public WebApplicationBuilder AddWorkoutInfrastructure()
    {
      var connectionString =
        builder.Configuration.GetConnectionString("SqlServer")
        ?? throw new InvalidOperationException("Connection string 'SqlServer' was not found.");
      builder.Services.AddDbContext<WorkoutServiceDbContext>(options =>
        options
          .UseSqlServer(connectionString)
          .UseSeeding(
            (ctx, _) =>
            {
              var db = (WorkoutServiceDbContext)ctx;
              ExerciseSeedData.Seed(db);
              WorkoutTemplatesSeedData.Seed(db);
            }
          )
          .UseAsyncSeeding(
            async (ctx, _, ct) =>
            {
              var db = (WorkoutServiceDbContext)ctx;
              await ExerciseSeedData.SeedAsync(db, ct);
              await WorkoutTemplatesSeedData.SeedAsync(db, ct);
            }
          )
      );
      return builder;
    }
  }

  extension(WebApplication app)
  {
    public WebApplication MapEndpoints()
    {
      var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

      var builder = app.MapGroup("/api");

      foreach (var endpoint in endpoints)
        endpoint.MapEndpoint(builder);

      return app;
    }
  }

  extension(IServiceCollection services)
  {
    /// <summary>
    /// Registers authentication-related services: <see cref="IHttpContextAccessor"/>,
    /// <see cref="UserInfo"/> for reading the current user's claims,
    /// and <see cref="UserInfoMiddleware"/> for populating it per request.
    /// </summary>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public IServiceCollection AddWorkoutAuth()
    {
      services.AddHttpContextAccessor();
      services.AddScoped<UserInfo>();
      services.AddScoped<UserInfoMiddleware>();
      return services;
    }

    /// <summary>
    /// Registers application-layer services, such as <see cref="IExerciseService"/>.
    /// </summary>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public IServiceCollection AddWorkoutServices()
    {
      services.AddScoped<IExerciseService, ExerciseService>();
      services.AddScoped<IWorkoutsService, WorkoutsService>();
      services.AddScoped<IWorkoutTemplatesService, WorkoutTemplatesService>();
      return services;
    }

    public IServiceCollection AddEndpoints()
    {
      var endpointTypes = typeof(Program)
        .Assembly.GetExportedTypes()
        .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(IEndpoint).IsAssignableFrom(t));

      foreach (var type in endpointTypes)
        services.AddTransient(typeof(IEndpoint), type);

      return services;
    }
  }
}
