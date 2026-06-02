using MassTransit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using WorkoutService.Common.Enums;
using WorkoutService.Common.Messaging.Consumers;
using WorkoutService.Common.Messaging.Events;
using WorkoutService.Features.ExerciseCatalog.Domain;
using WorkoutService.Features.Workouts.Domain;
using WorkoutService.Features.WorkoutTemplates.Domain;
using WorkoutService.Infrastructure;

namespace WorkoutService.Tests.Common.Messaging;

public class UserDeletedConsumerTests : IDisposable
{
  private readonly SqliteConnection _connection;
  private readonly WorkoutServiceDbContext _context;
  private readonly UserDeletedConsumer _sut;

  public UserDeletedConsumerTests()
  {
    // store connection to maintain it available for the duration of the tests
    _connection = new SqliteConnection("DataSource=:memory:");
    _connection.Open();

    var options = new DbContextOptionsBuilder<WorkoutServiceDbContext>().UseSqlite(_connection).Options;

    _context = new WorkoutServiceDbContext(options);
    _context.Database.EnsureCreated();

    _sut = new UserDeletedConsumer(_context);
  }

  // ── Helpers ──────────────────────────────────────────────────────────────────────

  private static ConsumeContext<UserDeletedEvent> MockContext(string username)
  {
    var ctx = Substitute.For<ConsumeContext<UserDeletedEvent>>();
    ctx.Message.Returns(new UserDeletedEvent(username));
    ctx.CancellationToken.Returns(CancellationToken.None);
    return ctx;
  }

  private async Task SeedAliceDataAsync()
  {
    _context.Exercises.Add(
      new Exercise
      {
        Creator = "alice",
        Name = "Alice Exercise",
        Difficulty = Difficulty.Easy,
        TrackingType = TrackingType.Reps,
      }
    );

    _context.WorkoutTemplates.Add(new WorkoutTemplate { Creator = "alice", Name = "Alice Template" });

    _context.Workouts.Add(
      new Workout
      {
        Creator = "alice",
        StartedAt = DateTime.UtcNow,
        CompletedAt = DateTime.UtcNow,
        WorkoutSets = new List<WorkoutSet>(),
      }
    );

    await _context.SaveChangesAsync();
  }

  private async Task SeedBobDataAsync()
  {
    _context.Exercises.Add(
      new Exercise
      {
        Creator = "bob",
        Name = "Bob Exercise",
        Difficulty = Difficulty.Medium,
        TrackingType = TrackingType.WeightAndReps,
      }
    );

    await _context.SaveChangesAsync();
  }

  // ═══════════════════════════════════════════════════════════════════════════════
  // Consume
  // ═══════════════════════════════════════════════════════════════════════════════

  [Fact]
  public async Task Consume_DeletesAllDataForUser_WhenEventReceived()
  {
    // arrange
    await SeedAliceDataAsync();
    await SeedBobDataAsync();

    // act
    await _sut.Consume(MockContext("alice"));

    // assert
    Assert.False(await _context.Exercises.AnyAsync(e => e.Creator == "alice"));
    Assert.False(await _context.WorkoutTemplates.AnyAsync(t => t.Creator == "alice"));
    Assert.False(await _context.Workouts.AnyAsync(w => w.Creator == "alice"));
  }

  [Fact]
  public async Task Consume_LeavesOtherUsersDataIntact()
  {
    // arrange
    await SeedAliceDataAsync();
    await SeedBobDataAsync();

    // act
    await _sut.Consume(MockContext("alice"));

    // assert
    Assert.True(await _context.Exercises.AnyAsync(e => e.Creator == "bob"));
  }

  [Fact]
  public async Task Consume_IsIdempotent_WhenUserHasNoData()
  {
    // act & assert
    await _sut.Consume(MockContext("nonexistent-user"));
  }

  // ── Cleanup ───────────────────────────────────────────────────────────────────

  public void Dispose()
  {
    GC.SuppressFinalize(this);
    _context.Dispose();
    _connection.Dispose();
  }
}
