using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Features.ExerciseCatalog.Domain;
using WorkoutService.Features.Workouts.Domain;

namespace WorkoutService.Features.Workouts.Persistence;

public class WorkoutSetsConfiguration : IEntityTypeConfiguration<WorkoutSet>
{
  public void Configure(EntityTypeBuilder<WorkoutSet> builder)
  {
    builder.ToTable("WorkoutSets");

    builder.HasKey(x => new
    {
      x.WorkoutId,
      x.ExerciseId,
      x.SetNumber,
    });
    builder.Property(x => x.SetNumber).IsRequired();
    builder.Property(x => x.IsWarmup).IsRequired();

    builder
      .HasDiscriminator<string>("SetType")
      .HasValue<WeightAndRepsSet>("WeightReps")
      .HasValue<RepsSet>("Reps")
      .HasValue<DistanceAndTimeSet>("DistanceTime")
      .HasValue<TimeSet>("Time")
      .HasValue<WeightAndTimeSet>("WeightTime");

    builder
      .HasOne<Workout>(x => x.Workout)
      .WithMany(x => x.WorkoutSets)
      .HasForeignKey(x => x.WorkoutId)
      .OnDelete(DeleteBehavior.Cascade);

    builder
      .HasOne<Exercise>(x => x.Exercise)
      .WithMany()
      .HasForeignKey(x => x.ExerciseId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
