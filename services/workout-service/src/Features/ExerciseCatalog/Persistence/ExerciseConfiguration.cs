using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Features.ExerciseCatalog.Domain;

namespace WorkoutService.Features.ExerciseCatalog.Persistence;

public sealed class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
  public void Configure(EntityTypeBuilder<Exercise> builder)
  {
    builder.ToTable("Exercises");
    builder.HasKey(x => x.Id);

    builder.Property(x => x.Name).HasMaxLength(30).IsRequired();
    builder.Property(x => x.Difficulty).HasConversion<string>().IsRequired();
    builder.Property(x => x.Creator).IsRequired();

    builder
      .HasMany(x => x.ExerciseMuscleGroups)
      .WithOne(x => x.Exercise)
      .HasForeignKey(x => x.ExerciseId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
