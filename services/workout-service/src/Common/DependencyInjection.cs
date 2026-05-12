using Microsoft.EntityFrameworkCore;
using WorkoutService.Common.Auth;
using WorkoutService.Features.ExerciseCatalog.Persistence;
using WorkoutService.Features.ExerciseCatalog.Services;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class WebApplicationExtensions
{
  extension(WebApplication app)
  {
    /// <summary>
    /// Discovers every concrete <see cref="DbContext"/> subclass in the entry assembly
    /// and runs pending migrations on each one, so <c>Program.cs</c>
    /// never needs updating when new feature contexts are added.
    /// </summary>
    public async Task MigrateAllDbContextsAsync()
    {
      var dbContextTypes = typeof(Program)
        .Assembly.GetTypes()
        .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsAssignableTo(typeof(DbContext)));

      await using var scope = app.Services.CreateAsyncScope();
      foreach (var contextType in dbContextTypes)
      {
        if (scope.ServiceProvider.GetService(contextType) is DbContext ctx)
          await ctx.Database.MigrateAsync();
      }
    }
  }
}

/// <summary>
/// Extension methods for registering all Workout service dependencies
/// into the ASP.NET Core dependency injection container.
/// </summary>
public static class WorkoutServices
{
  extension(WebApplicationBuilder builder)
  {
    /// <summary>
    /// Registers infrastructure concerns: the EF Core <see cref="ExerciseCatalogDbContext"/> backed by SQL Server,
    /// including synchronous and asynchronous seed data for the exercise catalog.
    /// Throws <see cref="InvalidOperationException"/> if the <c>SqlServer</c> connection string is missing.
    /// </summary>
    /// <returns>The same <see cref="WebApplicationBuilder"/> for chaining.</returns>
    public WebApplicationBuilder AddWorkoutInfrastructure()
    {
      var connectionString =
        builder.Configuration.GetConnectionString("SqlServer")
        ?? throw new InvalidOperationException("Connection string 'SqlServer' was not found.");
      builder.Services.AddDbContext<ExerciseCatalogDbContext>(options =>
        options
          .UseSqlServer(connectionString)
          .UseSeeding((ctx, _) => ExerciseSeedData.Seed((ExerciseCatalogDbContext)ctx))
          .UseAsyncSeeding(async (ctx, _, ct) => await ExerciseSeedData.SeedAsync((ExerciseCatalogDbContext)ctx, ct))
      );
      return builder;
    }
  }

  extension(IServiceCollection services)
  {
    /// <summary>
    /// Registers AutoMapper and scans the entry assembly for mapping profiles.
    /// </summary>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public IServiceCollection AddWorkoutMappings()
    {
      services.AddAutoMapper(_ => { }, typeof(Program).Assembly);
      return services;
    }

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
      return services;
    }
  }
}
