using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Enums;
using WorkoutService.Common.Exceptions;
using WorkoutService.Features.WorkoutTemplates.Domain;
using WorkoutService.Features.WorkoutTemplates.Dtos;
using WorkoutService.Infrastructure;

namespace WorkoutService.Features.WorkoutTemplates.Services;

public class WorkoutTemplatesService(WorkoutServiceDbContext dbContext, IMapper mapper) : IWorkoutTemplatesService
{
  public async Task<WorkoutTemplateResponse?> GetWorkoutTemplateById(long id)
  {
    var template = await dbContext.WorkoutTemplates.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    if (template is not null)
      return mapper.Map<WorkoutTemplateResponse>(template);
    return null;
  }

  public Task<List<WorkoutTemplateResponse>> GetAllWorkoutTemplates(int pageSize, int pageNumber)
  {
    return dbContext
      .WorkoutTemplates.AsNoTracking()
      .OrderBy(e => e.Id)
      .Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .Select(e => mapper.Map<WorkoutTemplateResponse>(e))
      .ToListAsync();
  }

  public async Task<WorkoutTemplateResponse> CreateWorkoutTemplate(
    CreateWorkoutTemplateRequest request,
    UserInfo userInfo
  )
  {
    var toBeSaved = new WorkoutTemplate { Creator = userInfo.Username, Name = request.Name };

    var templateExercises = await GetOrderedExercisesById(request.ExerciseIds, toBeSaved);

    toBeSaved.OverallDifficulty = ComputeOverallDifficulty(templateExercises);

    toBeSaved.Exercises.AddRange(templateExercises);

    dbContext.WorkoutTemplates.Add(toBeSaved);
    await dbContext.SaveChangesAsync();

    return mapper.Map<WorkoutTemplateResponse>(toBeSaved);
  }

  public async Task UpdateWorkoutTemplate(UpdateWorkoutTemplateRequest request, UserInfo userInfo)
  {
    var template = await dbContext
      .WorkoutTemplates.Include(workoutTemplate => workoutTemplate.Exercises)
      .FirstOrDefaultAsync(t => t.Id == request.Id);

    if (template == null)
      throw new NotFoundException($"Template with ID {request.Id} not found");

    if (template.Creator != userInfo.Username)
      throw new ForbiddenException($"User {userInfo.Username} does not own template with ID {template.Id}");

    template.Name = request.Name;
    template.Exercises = await GetOrderedExercisesById(request.Exercises, template);
    template.OverallDifficulty = ComputeOverallDifficulty(template.Exercises);

    await dbContext.SaveChangesAsync();
  }

  public async Task DeleteWorkoutTemplateById(long id, UserInfo userInfo)
  {
    var template = await dbContext
      .WorkoutTemplates.Include(workoutTemplate => workoutTemplate.Exercises)
      .FirstOrDefaultAsync(t => t.Id == id);

    if (template == null)
      throw new NotFoundException($"Template with ID {id} not found");

    if (template.Creator != userInfo.Username)
      throw new ForbiddenException($"User {userInfo.Username} does not own template with ID {template.Id}");

    dbContext.Remove(template);
    await dbContext.SaveChangesAsync();
  }

  private static Difficulty ComputeOverallDifficulty(List<WorkoutTemplateExercise> workoutTemplateExercises)
  {
    return workoutTemplateExercises
      .Select(e =>
      {
        if (e.Exercise is null)
          throw new InvalidOperationException(
            $"WorkoutTemplateExercise with exercise ID '{e.ExerciseId}' has a null back reference"
          );
        return e.Exercise.Difficulty;
      })
      .Aggregate(Difficulty.AggregateDifficulty);
  }

  private async Task<List<WorkoutTemplateExercise>> GetOrderedExercisesById(
    List<long> exerciseIds,
    WorkoutTemplate template
  )
  {
    var orderMap = exerciseIds.Select((id, index) => new { id, index }).ToDictionary(x => x.id, x => x.index);

    var exercises = await dbContext.Exercises.AsNoTracking().Where(e => exerciseIds.Contains(e.Id)).ToListAsync();

    if (exercises.Count < exerciseIds.Count)
    {
      var requestedIds = orderMap.Keys.ToHashSet();
      var foundIds = exercises.Select(e => e.Id).ToHashSet();
      var missingExercises = requestedIds
        .Except(foundIds)
        .ToList()
        .Select(a => a.ToString())
        .Aggregate((a, b) => $"{a}, {b}");
      throw new ConflictException($"Requested IDs '{missingExercises}' not found");
    }

    return exercises
      .Select(exercise => new WorkoutTemplateExercise()
      {
        Exercise = exercise,
        Order = orderMap[exercise.Id] + 1,
        WorkoutTemplate = template,
      })
      .ToList();
  }
}
