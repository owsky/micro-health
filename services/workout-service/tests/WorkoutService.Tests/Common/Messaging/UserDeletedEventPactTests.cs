using DotNetEnv;
using MassTransit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using PactNet;
using PactNet.Matchers;
using WorkoutService.Common.Messaging.Consumers;
using WorkoutService.Common.Messaging.Events;
using WorkoutService.Infrastructure;

namespace WorkoutService.Tests.Common.Messaging;

public class UserDeletedEventPactTests : IDisposable
{
  static UserDeletedEventPactTests() => Env.TraversePath().Load();

  private static string PactDir => Environment.GetEnvironmentVariable("CONTRACTS_PATH")!;

  private readonly SqliteConnection _connection;
  private readonly WorkoutServiceDbContext _context;
  private readonly UserDeletedConsumer _consumer;

  public UserDeletedEventPactTests()
  {
    _connection = new SqliteConnection("DataSource=:memory:");
    _connection.Open();

    var options = new DbContextOptionsBuilder<WorkoutServiceDbContext>().UseSqlite(_connection).Options;

    _context = new WorkoutServiceDbContext(options);
    _context.Database.EnsureCreated();

    _consumer = new UserDeletedConsumer(_context);
  }

  [Fact]
  public async Task UserDeletedConsumer_MatchesContractWith_UserService()
  {
    Directory.CreateDirectory(PactDir);

    var config = new PactConfig { PactDir = PactDir, LogLevel = PactLogLevel.Information };

    var pact = Pact.V4("workout-service", "user-service", config);

    await pact.WithMessageInteractions()
      .ExpectsToReceive("a user deleted event when a user account is removed")
      .WithMetadata("contentType", "application/json")
      .WithJsonContent(new { Username = Match.Type("alice") })
      .VerifyAsync<UserDeletedEvent>(async message =>
      {
        var ctx = Substitute.For<ConsumeContext<UserDeletedEvent>>();
        ctx.Message.Returns(message);
        ctx.CancellationToken.Returns(CancellationToken.None);
        await _consumer.Consume(ctx);
      });
  }

  public void Dispose()
  {
    GC.SuppressFinalize(this);
    _context.Dispose();
    _connection.Dispose();
  }
}
