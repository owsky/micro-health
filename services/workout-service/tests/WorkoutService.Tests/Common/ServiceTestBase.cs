using Microsoft.EntityFrameworkCore;
using WorkoutService.Infrastructure;

namespace WorkoutService.Tests.Common;

public abstract class ServiceTestBase : IDisposable
{
  protected readonly WorkoutServiceDbContext Context;

  protected ServiceTestBase()
  {
    var options = new DbContextOptionsBuilder<WorkoutServiceDbContext>()
      .UseInMemoryDatabase(Guid.NewGuid().ToString())
      .Options;
    Context = new WorkoutServiceDbContext(options);
  }

  public void Dispose()
  {
    Context.Database.EnsureDeleted();
    Context.Dispose();
    GC.SuppressFinalize(this);
  }
}
