using Riok.Mapperly.Abstractions;
using WorkoutService.Common.Enums;
using WorkoutService.Features.ExerciseCatalog.Domain;
using WorkoutService.Features.ExerciseCatalog.Dtos;

namespace WorkoutService.Features.ExerciseCatalog.Mappings;

[Mapper]
public static partial class ExerciseMapper
{
  [MapProperty(nameof(Exercise.ExerciseMuscleGroups), nameof(ExerciseResponse.MuscleGroups))]
  public static partial ExerciseResponse ToResponse(Exercise exercise);

  private static MuscleGroup MapMuscleGroup(ExerciseMuscleGroup emg) => emg.MuscleGroup;
}
