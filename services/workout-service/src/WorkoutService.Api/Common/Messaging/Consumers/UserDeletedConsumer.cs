using MassTransit;
using Microsoft.EntityFrameworkCore;
using WorkoutService.Common.Messaging.Events;
using WorkoutService.Infrastructure;

namespace WorkoutService.Common.Messaging.Consumers;

public class UserDeletedConsumer(WorkoutServiceDbContext dbContext) : IConsumer<UserDeletedEvent>
{
  public async Task Consume(ConsumeContext<UserDeletedEvent> context)
  {
    var username = context.Message.Username;

    await dbContext.Workouts.Where(w => w.Creator == username).ExecuteDeleteAsync(context.CancellationToken);

    await dbContext.WorkoutTemplates.Where(t => t.Creator == username).ExecuteDeleteAsync(context.CancellationToken);

    await dbContext.Exercises.Where(e => e.Creator == username).ExecuteDeleteAsync(context.CancellationToken);
  }
}
