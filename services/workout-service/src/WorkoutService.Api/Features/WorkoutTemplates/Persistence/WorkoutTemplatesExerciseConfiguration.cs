using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Features.WorkoutTemplates.Domain;

namespace WorkoutService.Features.WorkoutTemplates.Persistence;

public sealed class WorkoutTemplatesExerciseConfiguration : IEntityTypeConfiguration<WorkoutTemplateExercise>
{
  public void Configure(EntityTypeBuilder<WorkoutTemplateExercise> builder)
  {
    builder.ToTable("WorkoutTemplateExercises");
    builder.HasKey(x => new { x.WorkoutTemplateId, x.ExerciseId });

    builder.Property(x => x.Order).IsRequired();

    builder.HasIndex(x => new { x.WorkoutTemplateId, x.Order }).IsUnique();

    builder
      .HasOne(x => x.WorkoutTemplate)
      .WithMany(t => t.Exercises)
      .HasForeignKey(x => x.WorkoutTemplateId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(x => x.Exercise).WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
  }
}
