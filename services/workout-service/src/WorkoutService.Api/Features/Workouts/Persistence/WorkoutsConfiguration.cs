using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Features.Workouts.Domain;

namespace WorkoutService.Features.Workouts.Persistence;

public class WorkoutsConfiguration : IEntityTypeConfiguration<Workout>
{
  public void Configure(EntityTypeBuilder<Workout> builder)
  {
    builder.ToTable("Workouts");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Creator).IsRequired();

    builder.Property(x => x.Name).HasMaxLength(30);

    builder.Property(x => x.StartedAt).IsRequired();

    builder.Property(x => x.CompletedAt).IsRequired();

    builder.Property(x => x.Note).HasMaxLength(50);
  }
}
