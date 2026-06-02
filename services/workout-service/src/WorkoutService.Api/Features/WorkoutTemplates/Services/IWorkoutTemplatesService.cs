using WorkoutService.Common.Auth;
using WorkoutService.Common.Enums;
using WorkoutService.Features.WorkoutTemplates.Dtos;

namespace WorkoutService.Features.WorkoutTemplates.Services;

public interface IWorkoutTemplatesService
{
  public Task<WorkoutTemplateResponse?> GetWorkoutTemplateById(long id);

  public Task<List<WorkoutTemplateResponse>> GetAllWorkoutTemplates(
    int pageSize,
    int pageNumber,
    UserInfo userInfo,
    string? name = null,
    Difficulty? overallDifficulty = null,
    List<long>? exerciseIds = null,
    bool mine = false
  );

  public Task<WorkoutTemplateResponse> CreateWorkoutTemplate(CreateWorkoutTemplateRequest request, UserInfo userInfo);

  public Task UpdateWorkoutTemplate(UpdateWorkoutTemplateRequest request, UserInfo userInfo);

  public Task DeleteWorkoutTemplateById(long id, UserInfo userInfo);
}
