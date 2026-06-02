using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using WorkoutService.Features.ExerciseCatalog.Services;
using WorkoutService.Features.Workouts.Services;
using WorkoutService.Features.WorkoutTemplates.Services;
using WorkoutService.Infrastructure;

namespace WorkoutService.Tests.Common;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
  public IExerciseService ExerciseServiceMock { get; } = Substitute.For<IExerciseService>();
  public IWorkoutsService WorkoutsServiceMock { get; } = Substitute.For<IWorkoutsService>();
  public IWorkoutTemplatesService WorkoutTemplatesServiceMock { get; } = Substitute.For<IWorkoutTemplatesService>();

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.UseEnvironment("Testing");

    builder.ConfigureAppConfiguration(
      (_, config) =>
        config.AddInMemoryCollection(
          new Dictionary<string, string?>
          {
            ["ConnectionStrings:SqlServer"] = "Server=.;Database=TestDb;Trusted_Connection=True;",
            ["RABBITMQ_HOST"] = "localhost",
            ["RABBITMQ_PORT"] = "5672",
            ["RABBITMQ_DEFAULT_USER"] = "guest",
            ["RABBITMQ_DEFAULT_PASS"] = "guest",
            ["RabbitMQ:Exchange"] = "test-exchange",
            ["RabbitMQ:UserDeletedRoutingKey"] = "test.user.deleted",
          }
        )
    );

    builder.ConfigureServices(services =>
    {
      services.RemoveAll<WorkoutServiceDbContext>();
      services.AddDbContext<WorkoutServiceDbContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

      services.RemoveAll<IHostedService>();

      services.RemoveAll<IExerciseService>();
      services.AddScoped(_ => ExerciseServiceMock);

      services.RemoveAll<IWorkoutsService>();
      services.AddScoped(_ => WorkoutsServiceMock);

      services.RemoveAll<IWorkoutTemplatesService>();
      services.AddScoped(_ => WorkoutTemplatesServiceMock);
    });
  }
}
