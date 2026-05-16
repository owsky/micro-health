using Microsoft.EntityFrameworkCore;

namespace WorkoutService.Infrastructure;

public sealed partial class WorkoutServiceDbContext(DbContextOptions<WorkoutServiceDbContext> options)
  : DbContext(options)
{
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorkoutServiceDbContext).Assembly);
    base.OnModelCreating(modelBuilder);
  }
}
