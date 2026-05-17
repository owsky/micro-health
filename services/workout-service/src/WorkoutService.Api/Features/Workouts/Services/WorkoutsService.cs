using Microsoft.EntityFrameworkCore;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Exceptions;
using WorkoutService.Features.Workouts.Dto;
using WorkoutService.Features.Workouts.Mappings;
using WorkoutService.Infrastructure;

namespace WorkoutService.Features.Workouts.Services;

public class WorkoutsService(WorkoutServiceDbContext dbContext) : IWorkoutsService
{
  public async Task<WorkoutResponse> CreateWorkout(CreateWorkoutRequest request, UserInfo userInfo)
  {
    await ValidateExercisesAndTrackingTypes(request.Sets);

    var toBeSaved = WorkoutMapper.ToDomain(request, userInfo.Username);
    toBeSaved.Creator = userInfo.Username;
    var saved = await dbContext.Workouts.AddAsync(toBeSaved);
    await dbContext.SaveChangesAsync();
    return WorkoutMapper.ToResponse(saved.Entity);
  }

  public async Task<WorkoutResponse> GetWorkoutById(long id, UserInfo userInfo)
  {
    var workout = await dbContext
      .Workouts.AsNoTracking()
      .Include(w => w.WorkoutSets)
      .FirstOrDefaultAsync(w => w.Id == id);
    if (workout is null)
      throw new NotFoundException($"Workout with ID ${id} not found");

    if (workout.Creator != userInfo.Username)
      throw new ForbiddenException($"User {userInfo.Username} does not own workout with ID ${id}");

    return WorkoutMapper.ToResponse(workout);
  }

  public async Task<List<WorkoutResponse>> GetAllWorkouts(UserInfo userInfo)
  {
    var workouts = await dbContext
      .Workouts.AsNoTracking()
      .Include(w => w.WorkoutSets)
      .Where(w => w.Creator == userInfo.Username)
      .ToListAsync();
    return workouts.Select(WorkoutMapper.ToResponse).ToList();
  }

  public async Task UpdateWorkoutById(long id, UpdateWorkoutRequest request, UserInfo userInfo)
  {
    var workout = await dbContext.Workouts.Include(w => w.WorkoutSets).FirstOrDefaultAsync(w => w.Id == id);
    if (workout is null)
      throw new NotFoundException($"Workout with ID ${id} not found");

    if (workout.Creator != userInfo.Username)
      throw new ForbiddenException($"User {userInfo.Username} does not own workout with ID ${id}");

    await ValidateExercisesAndTrackingTypes(request.Sets);

    workout.Name = request.Name;
    workout.StartedAt = request.StartedAt;
    workout.CompletedAt = request.CompletedAt;
    workout.Note = request.Note;
    workout.WorkoutSets = request.Sets.Select(WorkoutMapper.ToDomain).ToList();

    await dbContext.SaveChangesAsync();
  }

  public async Task DeleteWorkoutById(long id, UserInfo userInfo)
  {
    var workout = await dbContext.Workouts.FirstOrDefaultAsync(w => w.Id == id);
    if (workout is null)
      return;

    if (workout.Creator != userInfo.Username)
      throw new ForbiddenException($"User {userInfo.Username} does not own workout with ID ${id}");
    dbContext.Workouts.Remove(workout);
    await dbContext.SaveChangesAsync();
  }

  private async Task ValidateExercisesAndTrackingTypes(IEnumerable<CompletedSetRequest> sets)
  {
    var setsList = sets.ToList();
    var exerciseIds = setsList.Select(s => s.ExerciseId).Distinct().ToList();
    var exercises = await dbContext.Exercises.Where(e => exerciseIds.Contains(e.Id)).ToDictionaryAsync(e => e.Id);

    // Check all exercises exist
    var missingIds = exerciseIds.Except(exercises.Keys).ToList();
    if (missingIds.Count != 0)
      throw new ConflictException($"Exercises with IDs {string.Join(", ", missingIds)} not found");

    // Validate each set's tracking type matches the exercise's tracking type
    foreach (var set in setsList)
    {
      var exercise = exercises[set.ExerciseId];
      var expectedTrackingType = set.GetTrackingType();

      if (exercise.TrackingType != expectedTrackingType)
        throw new ConflictException(
          $"Exercise {exercise.Name} (ID: {exercise.Id}) has tracking type {exercise.TrackingType}, "
            + $"but set requires {expectedTrackingType}"
        );
    }
  }
}
