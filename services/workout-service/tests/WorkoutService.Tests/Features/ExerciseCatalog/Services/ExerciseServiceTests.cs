using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Enums;
using WorkoutService.Common.Exceptions;
using WorkoutService.Features.ExerciseCatalog.Domain;
using WorkoutService.Features.ExerciseCatalog.Dtos;
using WorkoutService.Features.ExerciseCatalog.Services;
using WorkoutService.Tests.Common;

namespace WorkoutService.Tests.Features.ExerciseCatalog.Services;

public class ExerciseServiceTests : ServiceTestBase
{
  private readonly ExerciseService _sut;

  public ExerciseServiceTests()
  {
    _sut = new ExerciseService(Context);
  }

  private static UserInfo CreateUserInfo(string username)
  {
    var claims = new[] { new Claim(ClaimTypes.Name, username) };
    var identity = new ClaimsIdentity(claims, authenticationType: "Test");
    var principal = new ClaimsPrincipal(identity);

    var httpContext = new DefaultHttpContext { User = principal };

    var accessor = Substitute.For<IHttpContextAccessor>();
    accessor.HttpContext.Returns(httpContext);

    return new UserInfo(accessor);
  }

  private async Task<Exercise> SeedExerciseAsync(string creator)
  {
    var exercise = new Exercise
    {
      Creator = creator,
      Name = "Bench Press",
      Difficulty = Difficulty.Medium,
      TrackingType = TrackingType.WeightAndReps,
    };
    exercise.ExerciseMuscleGroups.Add(new ExerciseMuscleGroup { MuscleGroup = MuscleGroup.Chest, Exercise = exercise });

    Context.Exercises.Add(exercise);
    await Context.SaveChangesAsync();
    return exercise;
  }

  // ═══════════════════════════════════════════════════════════════════════════════
  // GetAllExercises
  // ═══════════════════════════════════════════════════════════════════════════════

  [Fact]
  public async Task GetAllExercises_ReturnsAllExercises_WhenMineIsFalse()
  {
    // arrange
    await SeedExerciseAsync("alice");
    await SeedExerciseAsync("bob");
    var userInfo = CreateUserInfo("alice");

    // act
    var result = await _sut.GetAllExercises(pageSize: 15, pageNumber: 1, mine: false, userInfo);

    // assert
    Assert.Equal(2, result.Count);
  }

  [Fact]
  public async Task GetAllExercises_ReturnsOnlyOwnExercises_WhenMineIsTrue()
  {
    // arrange
    await SeedExerciseAsync("alice");
    await SeedExerciseAsync("bob");
    var userInfo = CreateUserInfo("alice");

    // act
    var result = await _sut.GetAllExercises(pageSize: 15, pageNumber: 1, mine: true, userInfo);

    // assert
    Assert.Single(result);
    Assert.All(result, r => Assert.Equal("alice", r.Creator));
  }

  [Fact]
  public async Task GetAllExercises_RespectsPagination()
  {
    // arrange
    for (var i = 0; i < 5; i++)
      await SeedExerciseAsync("alice");
    var userInfo = CreateUserInfo("alice");

    // act
    var result = await _sut.GetAllExercises(pageSize: 2, pageNumber: 2, mine: false, userInfo);

    // assert
    Assert.Equal(2, result.Count);
  }

  // ═══════════════════════════════════════════════════════════════════════════════
  // GetExerciseById
  // ═══════════════════════════════════════════════════════════════════════════════

  [Fact]
  public async Task GetExerciseById_ReturnsExercise_WhenItExists()
  {
    // arrange
    var exercise = await SeedExerciseAsync("alice");

    // act
    var result = await _sut.GetExerciseById(exercise.Id);

    // assert
    Assert.NotNull(result);
    Assert.Equal(exercise.Id, result.Id);
    Assert.Equal("Bench Press", result.Name);
    Assert.Contains(MuscleGroup.Chest, result.MuscleGroups);
  }

  [Fact]
  public async Task GetExerciseById_ThrowsNotFoundException_WhenExerciseDoesNotExist()
  {
    // act & assert
    var ex = await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetExerciseById(999));
    Assert.Contains("999", ex.Message);
  }

  // ═══════════════════════════════════════════════════════════════════════════════
  // CreateExercise
  // ═══════════════════════════════════════════════════════════════════════════════

  [Fact]
  public async Task CreateExercise_PersistsAndReturnsExercise()
  {
    // arrange
    var userInfo = CreateUserInfo("alice");
    var request = new CreateExerciseRequest
    {
      Name = "Squat",
      Difficulty = Difficulty.High,
      TrackingType = TrackingType.WeightAndReps,
      MuscleGroups = [MuscleGroup.Quadriceps, MuscleGroup.Glutes],
    };

    // act
    var result = await _sut.CreateExercise(request, userInfo);

    // assert
    Assert.True(result.Id > 0);
    Assert.Equal("Squat", result.Name);
    Assert.Equal("alice", result.Creator);
    Assert.Equal(Difficulty.High, result.Difficulty);
    Assert.Contains(MuscleGroup.Quadriceps, result.MuscleGroups);
    Assert.Contains(MuscleGroup.Glutes, result.MuscleGroups);
    Assert.True(await Context.Exercises.AnyAsync(e => e.Id == result.Id));
  }

  [Fact]
  public async Task CreateExercise_DeduplicatesMuscleGroups()
  {
    // arrange
    var userInfo = CreateUserInfo("alice");
    var request = new CreateExerciseRequest
    {
      Name = "Curl",
      Difficulty = Difficulty.Easy,
      TrackingType = TrackingType.Reps,
      MuscleGroups = [MuscleGroup.Biceps, MuscleGroup.Biceps],
    };

    // act
    var result = await _sut.CreateExercise(request, userInfo);

    // assert
    Assert.Single(result.MuscleGroups);
  }

  // ═══════════════════════════════════════════════════════════════════════════════
  // UpdateExerciseById
  // ═══════════════════════════════════════════════════════════════════════════════

  [Fact]
  public async Task UpdateExerciseById_UpdatesExercise_WhenUserIsOwner()
  {
    // arrange
    var exercise = await SeedExerciseAsync("alice");
    var userInfo = CreateUserInfo("alice");
    var request = new UpdateExerciseRequest
    {
      Name = "Incline Bench Press",
      Difficulty = Difficulty.High,
      TrackingType = TrackingType.WeightAndReps,
      MuscleGroups = [MuscleGroup.Chest, MuscleGroup.Shoulders],
    };

    // act
    await _sut.UpdateExerciseById(exercise.Id, request, userInfo);

    // assert
    var updated = await Context.Exercises.Include(e => e.ExerciseMuscleGroups).FirstAsync(e => e.Id == exercise.Id);
    var muscleGroups = updated.ExerciseMuscleGroups.Select(e => e.MuscleGroup).ToList();
    Assert.Equal("Incline Bench Press", updated.Name);
    Assert.Equal(Difficulty.High, updated.Difficulty);
    Assert.Contains(MuscleGroup.Chest, muscleGroups);
    Assert.Contains(MuscleGroup.Shoulders, muscleGroups);
    Assert.Equal(TrackingType.WeightAndReps, updated.TrackingType);
  }

  [Fact]
  public async Task UpdateExerciseById_ThrowsNotFoundException_WhenExerciseDoesNotExist()
  {
    // arrange
    var userInfo = CreateUserInfo("alice");
    var request = new UpdateExerciseRequest
    {
      Name = "Ghost Exercise",
      Difficulty = Difficulty.Easy,
      TrackingType = TrackingType.Reps,
      MuscleGroups = [MuscleGroup.Biceps],
    };

    // act & Assert
    await Assert.ThrowsAsync<NotFoundException>(() => _sut.UpdateExerciseById(999, request, userInfo));
  }

  [Fact]
  public async Task UpdateExerciseById_ThrowsForbiddenException_WhenUserIsNotOwner()
  {
    // arrange
    var exercise = await SeedExerciseAsync("alice");
    var bobInfo = CreateUserInfo("bob");
    var request = new UpdateExerciseRequest
    {
      Name = "Hacked",
      Difficulty = Difficulty.Easy,
      TrackingType = TrackingType.Reps,
      MuscleGroups = [MuscleGroup.Biceps],
    };

    // act & assert
    await Assert.ThrowsAsync<ForbiddenException>(() => _sut.UpdateExerciseById(exercise.Id, request, bobInfo));
  }

  // ═══════════════════════════════════════════════════════════════════════════════
  // DeleteExerciseById
  // ═══════════════════════════════════════════════════════════════════════════════

  [Fact]
  public async Task DeleteExerciseById_RemovesExercise_WhenUserIsOwner()
  {
    // arrange
    var exercise = await SeedExerciseAsync("alice");
    var userInfo = CreateUserInfo("alice");

    // act
    await _sut.DeleteExerciseById(exercise.Id, userInfo);

    // assert
    Assert.False(await Context.Exercises.AnyAsync(e => e.Id == exercise.Id));
  }

  [Fact]
  public async Task DeleteExerciseById_DoesNothing_WhenExerciseDoesNotExist()
  {
    // arrange
    var userInfo = CreateUserInfo("alice");

    // act & assert
    await _sut.DeleteExerciseById(999, userInfo);
  }

  [Fact]
  public async Task DeleteExerciseById_ThrowsForbiddenException_WhenUserIsNotOwner()
  {
    // arrange
    var exercise = await SeedExerciseAsync("alice");
    var bobInfo = CreateUserInfo("bob");

    // act & assert
    await Assert.ThrowsAsync<ForbiddenException>(() => _sut.DeleteExerciseById(exercise.Id, bobInfo));
  }
}
