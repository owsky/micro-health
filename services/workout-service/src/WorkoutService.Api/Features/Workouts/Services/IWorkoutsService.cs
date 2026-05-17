using WorkoutService.Common.Auth;
using WorkoutService.Features.Workouts.Dto;

namespace WorkoutService.Features.Workouts.Services;

public interface IWorkoutsService
{
  public Task<WorkoutResponse> CreateWorkout(CreateWorkoutRequest request, UserInfo userInfo);

  public Task<WorkoutResponse> GetWorkoutById(long id, UserInfo userInfo);

  public Task<List<WorkoutResponse>> GetAllWorkouts(UserInfo userInfo);

  public Task UpdateWorkoutById(long id, UpdateWorkoutRequest request, UserInfo userInfo);

  public Task DeleteWorkoutById(long id, UserInfo userInfo);
}
