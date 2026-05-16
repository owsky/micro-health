using Riok.Mapperly.Abstractions;
using WorkoutService.Common.Enums;
using WorkoutService.Features.ExerciseCatalog.Domain;
using WorkoutService.Features.ExerciseCatalog.Dtos;
using WorkoutService.Features.WorkoutTemplates.Domain;
using WorkoutService.Features.WorkoutTemplates.Dtos;

namespace WorkoutService.Features.WorkoutTemplates.Mappings;

[Mapper]
public partial class WorkoutTemplateMapper
{
  public partial WorkoutTemplateResponse ToResponse(WorkoutTemplate template);

  [MapProperty(nameof(Exercise.ExerciseMuscleGroups), nameof(ExerciseResponse.MuscleGroups))]
  private partial ExerciseResponse ExerciseToResponse(Exercise exercise);

  private static MuscleGroup MapMuscleGroup(ExerciseMuscleGroup emg) => emg.MuscleGroup;

  private ExerciseResponse WorkoutTemplateExerciseToResponse(WorkoutTemplateExercise wte) =>
    ExerciseToResponse(wte.Exercise!);
}
