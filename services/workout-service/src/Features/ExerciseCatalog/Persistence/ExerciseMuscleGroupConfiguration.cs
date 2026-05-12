using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Features.ExerciseCatalog.Domain;

namespace WorkoutService.Features.ExerciseCatalog.Persistence;

public sealed class ExerciseMuscleGroupConfiguration : IEntityTypeConfiguration<ExerciseMuscleGroup>
{
  public void Configure(EntityTypeBuilder<ExerciseMuscleGroup> builder)
  {
    builder.ToTable("ExercisesMuscleGroup");
    builder.HasKey(x => new { x.ExerciseId, x.MuscleGroup });
    builder.Property(x => x.MuscleGroup).HasConversion<string>();
    builder.HasIndex(x => x.MuscleGroup);
  }
}
