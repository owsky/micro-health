using Microsoft.EntityFrameworkCore;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Enums;
using WorkoutService.Common.Exceptions;
using WorkoutService.Features.ExerciseCatalog.Domain;
using WorkoutService.Features.WorkoutTemplates.Domain;
using WorkoutService.Features.WorkoutTemplates.Dtos;
using WorkoutService.Features.WorkoutTemplates.Mappings;
using WorkoutService.Infrastructure;

namespace WorkoutService.Features.WorkoutTemplates.Services;

public class WorkoutTemplatesService(WorkoutServiceDbContext dbContext) : IWorkoutTemplatesService
{
  public async Task<WorkoutTemplateResponse?> GetWorkoutTemplateById(long id)
  {
    var template = await dbContext
      .WorkoutTemplates.AsNoTracking()
      .Include(e => e.Exercises)
        .ThenInclude(e => e.Exercise)
          .ThenInclude(e => e!.ExerciseMuscleGroups)
      .FirstOrDefaultAsync(e => e.Id == id);
    if (template is not null)
      return WorkoutTemplateMapper.ToResponse(template);
    return null;
  }

  public async Task<List<WorkoutTemplateResponse>> GetAllWorkoutTemplates(
    int pageSize,
    int pageNumber,
    UserInfo userInfo,
    string? name = null,
    Difficulty? overallDifficulty = null,
    List<long>? exerciseIds = null,
    bool mine = false
  )
  {
    var query = dbContext.WorkoutTemplates.AsNoTracking();

    if (mine)
    {
      query = query.Where(e => e.Creator == userInfo.Username);
    }

    if (!string.IsNullOrWhiteSpace(name))
    {
      query = query.Where(e => e.Name.Contains(name));
    }

    if (overallDifficulty.HasValue)
    {
      query = query.Where(e => e.OverallDifficulty == overallDifficulty.Value);
    }

    if (exerciseIds is { Count: > 0 })
    {
      query = query.Where(e => e.Exercises.Any(ex => exerciseIds.Contains(ex.ExerciseId)));
    }

    var results = await query
      .Include(e => e.Exercises)
        .ThenInclude(ex => ex.Exercise)
          .ThenInclude(emg => emg!.ExerciseMuscleGroups)
      .OrderBy(e => e.Id)
      .Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync();

    return results.Select(WorkoutTemplateMapper.ToResponse).ToList();
  }

  public async Task<WorkoutTemplateResponse> CreateWorkoutTemplate(
    CreateWorkoutTemplateRequest request,
    UserInfo userInfo
  )
  {
    var toBeSaved = new WorkoutTemplate { Creator = userInfo.Username, Name = request.Name };

    var (templateExercises, exercises) = await GetOrderedExercisesById(request.ExerciseIds, toBeSaved);

    toBeSaved.OverallDifficulty = ComputeOverallDifficulty(exercises);

    toBeSaved.Exercises.AddRange(templateExercises);

    dbContext.WorkoutTemplates.Add(toBeSaved);
    await dbContext.SaveChangesAsync();

    return WorkoutTemplateMapper.ToResponse(toBeSaved);
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
    var (templateExercises, exercises) = await GetOrderedExercisesById(request.Exercises, template);
    template.Exercises = templateExercises;
    template.OverallDifficulty = ComputeOverallDifficulty(exercises);

    await dbContext.SaveChangesAsync();
  }

  public async Task DeleteWorkoutTemplateById(long id, UserInfo userInfo)
  {
    var template = await dbContext
      .WorkoutTemplates.Include(workoutTemplate => workoutTemplate.Exercises)
      .FirstOrDefaultAsync(t => t.Id == id);

    if (template == null)
      return;

    if (template.Creator != userInfo.Username)
      throw new ForbiddenException($"User {userInfo.Username} does not own template with ID {template.Id}");

    dbContext.Remove(template);
    await dbContext.SaveChangesAsync();
  }

  private static Difficulty ComputeOverallDifficulty(IEnumerable<Exercise> exercises)
  {
    return exercises.Select(e => e.Difficulty).Aggregate(Difficulty.AggregateDifficulty);
  }

  private async Task<(
    List<WorkoutTemplateExercise> templateExercises,
    List<Exercise> exercises
  )> GetOrderedExercisesById(List<long> exerciseIds, WorkoutTemplate template)
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

    var templateExercises = exercises
      .Select(exercise => new WorkoutTemplateExercise
      {
        ExerciseId = exercise.Id,
        Order = orderMap[exercise.Id] + 1,
        WorkoutTemplate = template,
      })
      .ToList();

    return (templateExercises, exercises);
  }
}
