using WorkoutService.Common.Auth;
using WorkoutService.Features.WorkoutTemplates.Dtos;

namespace WorkoutService.Features.WorkoutTemplates.Services;

public interface IWorkoutTemplateService
{
  public Task<WorkoutTemplateResponse?> GetWorkoutTemplateById(long id);

  public Task<List<WorkoutTemplateResponse>> GetAllWorkoutTemplates(int pageSize, int pageNumber);

  public Task<WorkoutTemplateResponse> CreateWorkoutTemplate(CreateWorkoutTemplateRequest request, UserInfo userInfo);

  public Task UpdateWorkoutTemplate(UpdateWorkoutTemplateRequest request, UserInfo userInfo);

  public Task DeleteWorkoutTemplateById(long id, UserInfo userInfo);
}
