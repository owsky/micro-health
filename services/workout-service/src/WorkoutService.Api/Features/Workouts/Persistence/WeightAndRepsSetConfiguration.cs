using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Features.Workouts.Domain;

namespace WorkoutService.Features.Workouts.Persistence;

public class WeightAndRepsSetConfiguration : IEntityTypeConfiguration<WeightAndRepsSet>
{
  public void Configure(EntityTypeBuilder<WeightAndRepsSet> builder)
  {
    builder.Property(x => x.Weight).HasPrecision(18, 2).HasColumnName("Weight");
  }
}
