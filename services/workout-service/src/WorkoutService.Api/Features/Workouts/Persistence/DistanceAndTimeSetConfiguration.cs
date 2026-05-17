using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Features.Workouts.Domain;

namespace WorkoutService.Features.Workouts.Persistence;

public class DistanceAndTimeSetConfiguration : IEntityTypeConfiguration<DistanceAndTimeSet>
{
  public void Configure(EntityTypeBuilder<DistanceAndTimeSet> builder)
  {
    builder.Property(x => x.Distance).HasPrecision(18, 2).HasColumnName("Distance");
  }
}
