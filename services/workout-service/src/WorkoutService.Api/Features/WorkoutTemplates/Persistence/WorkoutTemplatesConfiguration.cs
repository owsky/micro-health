using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Features.WorkoutTemplates.Domain;

namespace WorkoutService.Features.WorkoutTemplates.Persistence;

public sealed class WorkoutTemplatesConfiguration : IEntityTypeConfiguration<WorkoutTemplate>
{
  public void Configure(EntityTypeBuilder<WorkoutTemplate> builder)
  {
    builder.ToTable("WorkoutTemplates");
    builder.HasKey(x => x.Id);
    builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
    builder.Property(x => x.Creator).IsRequired();
    builder.Property(x => x.OverallDifficulty).HasConversion<string>().IsRequired();

    builder
      .HasMany(x => x.Exercises)
      .WithOne(x => x.WorkoutTemplate)
      .HasForeignKey(x => x.WorkoutTemplateId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
