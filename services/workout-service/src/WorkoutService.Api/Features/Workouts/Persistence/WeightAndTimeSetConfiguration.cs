using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Features.Workouts.Domain;

namespace WorkoutService.Features.Workouts.Persistence;

public class WeightAndTimeSetConfiguration : IEntityTypeConfiguration<WeightAndTimeSet>
{
  public void Configure(EntityTypeBuilder<WeightAndTimeSet> builder)
  {
    builder.Property(x => x.Weight).HasPrecision(18, 2).HasColumnName("Weight");
  }
}
