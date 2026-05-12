using WorkoutService.Common.Auth;
using WorkoutService.Features.ExerciseCatalog.Dtos;

namespace WorkoutService.Features.ExerciseCatalog.Services;

public interface IExerciseService
{
  public Task<List<ExerciseResponse>> GetAllExercises(int pageSize, int pageNumber);

  public Task<ExerciseResponse?> GetExerciseById(long id);

  public Task<ExerciseResponse> CreateExercise(CreateExerciseRequest request, UserInfo userInfo);

  public Task UpdateExerciseById(long id, UpdateExerciseRequest request, UserInfo userInfo);

  public Task DeleteExerciseById(long id, UserInfo userInfo);
}
