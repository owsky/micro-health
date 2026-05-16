using Riok.Mapperly.Abstractions;
using WorkoutService.Common.Enums;
using WorkoutService.Features.ExerciseCatalog.Domain;
using WorkoutService.Features.ExerciseCatalog.Dtos;

namespace WorkoutService.Features.ExerciseCatalog.Mappings;

[Mapper]
public partial class ExerciseMapper
{
  [MapProperty(nameof(Exercise.ExerciseMuscleGroups), nameof(ExerciseResponse.MuscleGroups))]
  public partial ExerciseResponse ToResponse(Exercise exercise);

  private static MuscleGroup MapMuscleGroup(ExerciseMuscleGroup emg) => emg.MuscleGroup;
}
