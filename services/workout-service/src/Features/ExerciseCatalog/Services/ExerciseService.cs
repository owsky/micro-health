using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorkoutService.Common.Auth;
using WorkoutService.Features.ExerciseCatalog.Domain;
using WorkoutService.Features.ExerciseCatalog.Dtos;
using WorkoutService.Features.ExerciseCatalog.Persistence;

namespace WorkoutService.Features.ExerciseCatalog.Services;

public class ExerciseService(ExerciseCatalogDbContext context, IMapper mapper) : IExerciseService
{
  public Task<List<ExerciseResponse>> GetAllExercises(int pageSize, int pageNumber)
  {
    return context
      .Exercises.AsNoTracking()
      .Include(e => e.MuscleGroupLinks)
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
      .Include(e => e.MuscleGroupLinks)
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
      MuscleGroups = request.MuscleGroups,
    };
    var saved = context.Exercises.Add(toBeSaved).Entity;
    await context.SaveChangesAsync();
    return mapper.Map<ExerciseResponse>(saved);
  }

  public async Task UpdateExerciseById(long id, UpdateExerciseRequest request, UserInfo userInfo)
  {
    var entity = await context.Exercises.Include(e => e.MuscleGroupLinks).FirstOrDefaultAsync(e => e.Id == id);
    if (entity is null)
      throw new Exception($"Entity with ID {id} not found");
    if (entity.Creator != userInfo.Username)
      throw new Exception("User does not own the exercise");
    entity.Name = request.Name;
    entity.Difficulty = request.Difficulty;
    entity.MuscleGroups = request.MuscleGroups;
    await context.SaveChangesAsync();
  }

  public async Task DeleteExerciseById(long id, UserInfo userInfo)
  {
    var entity = await context.Exercises.FindAsync(id);
    if (entity is not null)
    {
      if (entity.Creator != userInfo.Username)
        throw new Exception("User does not own the exercise");
      context.Exercises.Remove(entity);
      await context.SaveChangesAsync();
    }
  }
}
