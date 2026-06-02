using Microsoft.EntityFrameworkCore;
using WorkoutService.Common.Enums;
using WorkoutService.Common.Exceptions;
using WorkoutService.Features.ExerciseCatalog.Domain;
using WorkoutService.Features.Workouts.Domain;
using WorkoutService.Features.Workouts.Dto;
using WorkoutService.Features.Workouts.Services;
using WorkoutService.Tests.Common;
using WorkoutService.Tests.Common.Helpers;

namespace WorkoutService.Tests.Features.Workouts.Services;

public class WorkoutsServiceTests : ServiceTestBase
{
  private readonly WorkoutsService _sut;

  public WorkoutsServiceTests()
  {
    _sut = new WorkoutsService(Context);
  }

  private async Task<Exercise> SeedExerciseAsync(string creator, TrackingType trackingType = TrackingType.WeightAndReps)
  {
    var exercise = new Exercise
    {
      Creator = creator,
      Name = "Bench Press",
      Difficulty = Difficulty.Medium,
      TrackingType = trackingType,
    };
    exercise.ExerciseMuscleGroups.Add(new ExerciseMuscleGroup { MuscleGroup = MuscleGroup.Chest, Exercise = exercise });

    Context.Exercises.Add(exercise);
    await Context.SaveChangesAsync();
    return exercise;
  }

  private async Task<Workout> SeedWorkoutAsync(string creator, long exerciseId)
  {
    var startedAt = DateTime.UtcNow.AddHours(-1);
    var completedAt = DateTime.UtcNow;

    var workout = new Workout
    {
      Creator = creator,
      Name = "Morning Workout",
      StartedAt = startedAt,
      CompletedAt = completedAt,
      Note = "Great session",
      WorkoutSets = [],
    };

    Context.Workouts.Add(workout);
    await Context.SaveChangesAsync();

    // Now add the sets with the correct WorkoutId
    workout.WorkoutSets =
    [
      new WeightAndRepsSet
      {
        WorkoutId = workout.Id,
        ExerciseId = exerciseId,
        SetNumber = 1,
        IsWarmup = false,
        Weight = 100,
        Reps = 10,
      },
    ];

    await Context.SaveChangesAsync();
    return workout;
  }

  // ═══════════════════════════════════════════════════════════════════════════════
  // CreateWorkout
  // ═══════════════════════════════════════════════════════════════════════════════

  [Fact]
  public async Task CreateWorkout_PersistsAndReturnsWorkout()
  {
    // arrange
    var exercise = await SeedExerciseAsync("alice");
    var userInfo = UserInfoHelpers.CreateUserInfo("alice");
    var startedAt = DateTime.UtcNow.AddHours(-1);
    var completedAt = DateTime.UtcNow;

    var request = new CreateWorkoutRequest
    {
      Name = "Evening Workout",
      StartedAt = startedAt,
      CompletedAt = completedAt,
      Note = "Good session",
      Sets =
      [
        new WeightAndRepsSetRequest
        {
          ExerciseId = exercise.Id,
          SetNumber = 1,
          IsWarmup = false,
          Weight = 100,
          Reps = 12,
        },
      ],
    };

    // act
    var result = await _sut.CreateWorkout(request, userInfo);

    // assert
    Assert.True(result.Id > 0);
    Assert.Equal("Evening Workout", result.Name);
    Assert.Equal("alice", result.Creator);
    Assert.Equal(startedAt, result.StartedAt);
    Assert.Equal(completedAt, result.CompletedAt);
    Assert.Equal("Good session", result.Note);
    Assert.Single(result.Sets);
  }

  [Fact]
  public async Task CreateWorkout_ThrowsConflictException_WhenExerciseDoesNotExist()
  {
    // arrange
    var userInfo = UserInfoHelpers.CreateUserInfo("alice");
    var startedAt = DateTime.UtcNow.AddHours(-1);
    var completedAt = DateTime.UtcNow;

    var request = new CreateWorkoutRequest
    {
      Name = "Workout",
      StartedAt = startedAt,
      CompletedAt = completedAt,
      Sets =
      [
        new WeightAndRepsSetRequest
        {
          ExerciseId = 999,
          SetNumber = 1,
          IsWarmup = false,
          Weight = 100,
          Reps = 10,
        },
      ],
    };

    // act & assert
    var ex = await Assert.ThrowsAsync<ConflictException>(() => _sut.CreateWorkout(request, userInfo));
    Assert.Contains("999", ex.Message);
  }

  [Fact]
  public async Task CreateWorkout_ThrowsConflictException_WhenTrackingTypeDoesNotMatch()
  {
    // arrange
    var exercise = await SeedExerciseAsync("alice", TrackingType.Reps);
    var userInfo = UserInfoHelpers.CreateUserInfo("alice");
    var startedAt = DateTime.UtcNow.AddHours(-1);
    var completedAt = DateTime.UtcNow;

    var request = new CreateWorkoutRequest
    {
      Name = "Workout",
      StartedAt = startedAt,
      CompletedAt = completedAt,
      Sets =
      [
        new WeightAndRepsSetRequest
        {
          ExerciseId = exercise.Id,
          SetNumber = 1,
          IsWarmup = false,
          Weight = 100,
          Reps = 10,
        },
      ],
    };

    // act & assert
    var ex = await Assert.ThrowsAsync<ConflictException>(() => _sut.CreateWorkout(request, userInfo));
    Assert.Contains("tracking type", ex.Message);
  }

  // ═══════════════════════════════════════════════════════════════════════════════
  // GetWorkoutById
  // ═══════════════════════════════════════════════════════════════════════════════

  [Fact]
  public async Task GetWorkoutById_ReturnsWorkout_WhenItExists()
  {
    // arrange
    var exercise = await SeedExerciseAsync("alice");
    var workout = await SeedWorkoutAsync("alice", exercise.Id);
    var userInfo = UserInfoHelpers.CreateUserInfo("alice");

    // act
    var result = await _sut.GetWorkoutById(workout.Id, userInfo);

    // assert
    Assert.NotNull(result);
    Assert.Equal(workout.Id, result.Id);
    Assert.Equal("alice", result.Creator);
    Assert.Equal("Morning Workout", result.Name);
  }

  [Fact]
  public async Task GetWorkoutById_ThrowsNotFoundException_WhenWorkoutDoesNotExist()
  {
    // arrange
    var userInfo = UserInfoHelpers.CreateUserInfo("alice");

    // act & assert
    var ex = await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetWorkoutById(999, userInfo));
    Assert.Contains("999", ex.Message);
  }

  [Fact]
  public async Task GetWorkoutById_ThrowsForbiddenException_WhenUserIsNotOwner()
  {
    // arrange
    var exercise = await SeedExerciseAsync("alice");
    var workout = await SeedWorkoutAsync("alice", exercise.Id);
    var bobInfo = UserInfoHelpers.CreateUserInfo("bob");

    // act & assert
    await Assert.ThrowsAsync<ForbiddenException>(() => _sut.GetWorkoutById(workout.Id, bobInfo));
  }

  // ═══════════════════════════════════════════════════════════════════════════════
  // GetAllWorkouts
  // ═══════════════════════════════════════════════════════════════════════════════

  [Fact]
  public async Task GetAllWorkouts_ReturnsAllUserWorkouts()
  {
    // arrange
    var exercise = await SeedExerciseAsync("alice");
    await SeedWorkoutAsync("alice", exercise.Id);
    await SeedWorkoutAsync("alice", exercise.Id);
    await SeedWorkoutAsync("bob", exercise.Id);
    var userInfo = UserInfoHelpers.CreateUserInfo("alice");

    // act
    var result = await _sut.GetAllWorkouts(userInfo);

    // assert
    Assert.Equal(2, result.Count);
    Assert.All(result, w => Assert.Equal("alice", w.Creator));
  }

  [Fact]
  public async Task GetAllWorkouts_ReturnsEmptyList_WhenUserHasNoWorkouts()
  {
    // arrange
    var userInfo = UserInfoHelpers.CreateUserInfo("alice");

    // act
    var result = await _sut.GetAllWorkouts(userInfo);

    // assert
    Assert.Empty(result);
  }

  // ═══════════════════════════════════════════════════════════════════════════════
  // UpdateWorkoutById
  // ═══════════════════════════════════════════════════════════════════════════════

  [Fact]
  public async Task UpdateWorkoutById_UpdatesWorkout_WhenUserIsOwner()
  {
    // arrange
    var exercise = await SeedExerciseAsync("alice");
    var workout = await SeedWorkoutAsync("alice", exercise.Id);
    var userInfo = UserInfoHelpers.CreateUserInfo("alice");
    var newStartedAt = DateTime.UtcNow.AddHours(-2);
    var newCompletedAt = DateTime.UtcNow.AddHours(-1);

    var request = new UpdateWorkoutRequest
    {
      Name = "Updated Workout",
      StartedAt = newStartedAt,
      CompletedAt = newCompletedAt,
      Note = "Updated note",
      Sets =
      [
        new WeightAndRepsSetRequest
        {
          ExerciseId = exercise.Id,
          SetNumber = 1,
          IsWarmup = true,
          Weight = 50,
          Reps = 15,
        },
      ],
    };

    // act
    await _sut.UpdateWorkoutById(workout.Id, request, userInfo);

    // assert
    var updated = await Context.Workouts.FirstAsync(w => w.Id == workout.Id);
    Assert.Equal("Updated Workout", updated.Name);
    Assert.Equal("Updated note", updated.Note);
    Assert.Equal(newStartedAt, updated.StartedAt);
    Assert.Equal(newCompletedAt, updated.CompletedAt);
  }

  [Fact]
  public async Task UpdateWorkoutById_ThrowsNotFoundException_WhenWorkoutDoesNotExist()
  {
    // arrange
    var exercise = await SeedExerciseAsync("alice");
    var userInfo = UserInfoHelpers.CreateUserInfo("alice");

    var request = new UpdateWorkoutRequest
    {
      Name = "Workout",
      StartedAt = DateTime.UtcNow.AddHours(-1),
      CompletedAt = DateTime.UtcNow,
      Sets =
      [
        new WeightAndRepsSetRequest
        {
          ExerciseId = exercise.Id,
          SetNumber = 1,
          IsWarmup = false,
          Weight = 100,
          Reps = 10,
        },
      ],
    };

    // act & assert
    await Assert.ThrowsAsync<NotFoundException>(() => _sut.UpdateWorkoutById(999, request, userInfo));
  }

  [Fact]
  public async Task UpdateWorkoutById_ThrowsForbiddenException_WhenUserIsNotOwner()
  {
    // arrange
    var exercise = await SeedExerciseAsync("alice");
    var workout = await SeedWorkoutAsync("alice", exercise.Id);
    var bobInfo = UserInfoHelpers.CreateUserInfo("bob");

    var request = new UpdateWorkoutRequest
    {
      Name = "Hacked Workout",
      StartedAt = DateTime.UtcNow.AddHours(-1),
      CompletedAt = DateTime.UtcNow,
      Sets =
      [
        new WeightAndRepsSetRequest
        {
          ExerciseId = exercise.Id,
          SetNumber = 1,
          IsWarmup = false,
          Weight = 100,
          Reps = 10,
        },
      ],
    };

    // act & assert
    await Assert.ThrowsAsync<ForbiddenException>(() => _sut.UpdateWorkoutById(workout.Id, request, bobInfo));
  }

  [Fact]
  public async Task UpdateWorkoutById_ThrowsConflictException_WhenExerciseDoesNotExist()
  {
    // arrange
    var exercise = await SeedExerciseAsync("alice");
    var workout = await SeedWorkoutAsync("alice", exercise.Id);
    var userInfo = UserInfoHelpers.CreateUserInfo("alice");

    var request = new UpdateWorkoutRequest
    {
      Name = "Workout",
      StartedAt = DateTime.UtcNow.AddHours(-1),
      CompletedAt = DateTime.UtcNow,
      Sets =
      [
        new WeightAndRepsSetRequest
        {
          ExerciseId = 999,
          SetNumber = 1,
          IsWarmup = false,
          Weight = 100,
          Reps = 10,
        },
      ],
    };

    // act & assert
    var ex = await Assert.ThrowsAsync<ConflictException>(() => _sut.UpdateWorkoutById(workout.Id, request, userInfo));
    Assert.Contains("999", ex.Message);
  }

  // ═══════════════════════════════════════════════════════════════════════════════
  // DeleteWorkoutById
  // ═══════════════════════════════════════════════════════════════════════════════

  [Fact]
  public async Task DeleteWorkoutById_RemovesWorkout_WhenUserIsOwner()
  {
    // arrange
    var exercise = await SeedExerciseAsync("alice");
    var workout = await SeedWorkoutAsync("alice", exercise.Id);
    var userInfo = UserInfoHelpers.CreateUserInfo("alice");

    // act
    await _sut.DeleteWorkoutById(workout.Id, userInfo);

    // assert
    Assert.False(await Context.Workouts.AnyAsync(w => w.Id == workout.Id));
  }

  [Fact]
  public async Task DeleteWorkoutById_DoesNothing_WhenWorkoutDoesNotExist()
  {
    // arrange
    var userInfo = UserInfoHelpers.CreateUserInfo("alice");

    // act & assert
    await _sut.DeleteWorkoutById(999, userInfo);
  }

  [Fact]
  public async Task DeleteWorkoutById_ThrowsForbiddenException_WhenUserIsNotOwner()
  {
    // arrange
    var exercise = await SeedExerciseAsync("alice");
    var workout = await SeedWorkoutAsync("alice", exercise.Id);
    var bobInfo = UserInfoHelpers.CreateUserInfo("bob");

    // act & assert
    await Assert.ThrowsAsync<ForbiddenException>(() => _sut.DeleteWorkoutById(workout.Id, bobInfo));
  }
}
