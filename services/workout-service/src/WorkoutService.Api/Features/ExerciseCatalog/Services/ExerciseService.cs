using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Exceptions;
using WorkoutService.Features.ExerciseCatalog.Domain;
using WorkoutService.Features.ExerciseCatalog.Dtos;
using WorkoutService.Infrastructure;

namespace WorkoutService.Features.ExerciseCatalog.Services;

public class ExerciseService(WorkoutServiceDbContext context, IMapper mapper) : IExerciseService
{
  public Task<List<ExerciseResponse>> GetAllExercises(int pageSize, int pageNumber, bool mine, UserInfo userInfo)
  {
    return context
      .Exercises.AsNoTracking()
      .Include(e => e.ExerciseMuscleGroups)
      .Where(e => !mine || e.Creator == userInfo.Username)
      .OrderBy(e => e.Id)
      .Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .Select(e => mapper.Map<ExerciseResponse>(e))
      .ToListAsync();
  }

  public async Task<ExerciseResponse?> GetExerciseById(long id)
  {
    var entity = await context
      .Exercises.AsNoTracking()
      .Include(e => e.ExerciseMuscleGroups)
      .FirstOrDefaultAsync(e => e.Id == id);
    if (entity is not null)
      return mapper.Map<ExerciseResponse>(entity);
    return null;
  }

  public async Task<ExerciseResponse> CreateExercise(CreateExerciseRequest request, UserInfo userInfo)
  {
    var toBeSaved = new Exercise()
    {
      Creator = userInfo.Username,
      Name = request.Name,
      Difficulty = request.Difficulty,
    };
    foreach (var group in request.MuscleGroups.Distinct())
      toBeSaved.ExerciseMuscleGroups.Add(new ExerciseMuscleGroup { MuscleGroup = group, Exercise = toBeSaved });
    var saved = context.Exercises.Add(toBeSaved).Entity;
    await context.SaveChangesAsync();
    return mapper.Map<ExerciseResponse>(saved);
  }

  public async Task UpdateExerciseById(long id, UpdateExerciseRequest request, UserInfo userInfo)
  {
    var entity = await context.Exercises.Include(e => e.ExerciseMuscleGroups).FirstOrDefaultAsync(e => e.Id == id);
    if (entity is null)
      throw new NotFoundException($"Exercise with ID {id} not found");
    if (entity.Creator != userInfo.Username)
      throw new ForbiddenException("User does not own the exercise");
    entity.Name = request.Name;
    entity.Difficulty = request.Difficulty;
    entity.ExerciseMuscleGroups.Clear();
    foreach (var group in request.MuscleGroups.Distinct())
      entity.ExerciseMuscleGroups.Add(new ExerciseMuscleGroup { MuscleGroup = group, Exercise = entity });
    await context.SaveChangesAsync();
  }

  public async Task DeleteExerciseById(long id, UserInfo userInfo)
  {
    var entity = await context.Exercises.FindAsync(id);
    if (entity is not null)
    {
      if (entity.Creator != userInfo.Username)
        throw new ForbiddenException("User does not own the exercise");
      context.Exercises.Remove(entity);
      await context.SaveChangesAsync();
    }
  }
}
