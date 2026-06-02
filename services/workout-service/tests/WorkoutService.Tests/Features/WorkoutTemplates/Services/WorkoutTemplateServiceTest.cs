using Microsoft.EntityFrameworkCore;
using WorkoutService.Common.Enums;
using WorkoutService.Common.Exceptions;
using WorkoutService.Features.ExerciseCatalog.Domain;
using WorkoutService.Features.WorkoutTemplates.Dtos;
using WorkoutService.Features.WorkoutTemplates.Services;
using WorkoutService.Tests.Common;
using WorkoutService.Tests.Common.Helpers;

namespace WorkoutService.Tests.Features.WorkoutTemplates.Services;

public class WorkoutTemplateServiceTest : ServiceTestBase
{
  private readonly WorkoutTemplatesService _sut;

  public WorkoutTemplateServiceTest()
  {
    _sut = new WorkoutTemplatesService(Context);
  }

  private async Task<long> SeedExerciseAsync(string creator, Difficulty difficulty = Difficulty.Medium)
  {
    var exercise = new Exercise
    {
      Creator = creator,
      Name = "Bench Press",
      Difficulty = difficulty,
      TrackingType = TrackingType.WeightAndReps,
    };
    exercise.ExerciseMuscleGroups.Add(new ExerciseMuscleGroup { MuscleGroup = MuscleGroup.Chest, Exercise = exercise });
    Context.Exercises.Add(exercise);
    await Context.SaveChangesAsync();
    return exercise.Id;
  }

  /// <summary>
  /// Creates a workout template using a fresh service/context and returns the response DTO.
  /// </summary>
  private async Task<WorkoutTemplateResponse> CreateTemplateAsync(
    string name,
    List<long> exerciseIds,
    string creator = "alice"
  )
  {
    return await _sut.CreateWorkoutTemplate(
      new CreateWorkoutTemplateRequest { Name = name, ExerciseIds = exerciseIds },
      UserInfoHelpers.CreateUserInfo(creator)
    );
  }

  // ═══════════════════════════════════════════════════════════════════════════════
  // GetWorkoutTemplateById
  // ═══════════════════════════════════════════════════════════════════════════════

  [Fact]
  public async Task GetWorkoutTemplateById_ReturnsTemplate_WhenItExists()
  {
    // arrange
    var exerciseId = await SeedExerciseAsync("alice");
    var created = await CreateTemplateAsync("Push Day", [exerciseId]);

    // act
    var sut = new WorkoutTemplatesService(Context);
    var result = await sut.GetWorkoutTemplateById(created.Id);

    // assert
    Assert.NotNull(result);
    Assert.Equal(created.Id, result.Id);
    Assert.Equal("Push Day", result.Name);
  }

  [Fact]
  public async Task GetWorkoutTemplateById_ReturnsNull_WhenTemplateDoesNotExist()
  {
    // act
    var result = await _sut.GetWorkoutTemplateById(9999);

    // assert
    Assert.Null(result);
  }

  // ═══════════════════════════════════════════════════════════════════════════════
  // GetAllWorkoutTemplates
  // ═══════════════════════════════════════════════════════════════════════════════

  [Fact]
  public async Task GetAllWorkoutTemplates_ReturnsAllTemplates()
  {
    // arrange
    var exerciseId = await SeedExerciseAsync("alice");
    await CreateTemplateAsync("Template A", [exerciseId]);
    await CreateTemplateAsync("Template B", [exerciseId]);

    // act
    var result = await _sut.GetAllWorkoutTemplates(pageSize: 15, pageNumber: 1);

    // assert
    Assert.Equal(2, result.Count);
  }

  [Fact]
  public async Task GetAllWorkoutTemplates_RespectsPagination()
  {
    // arrange
    var exerciseId = await SeedExerciseAsync("alice");
    for (var i = 0; i < 5; i++)
      await CreateTemplateAsync($"Template {i}", [exerciseId]);

    // act
    var result = await _sut.GetAllWorkoutTemplates(pageSize: 2, pageNumber: 2);

    // assert
    Assert.Equal(2, result.Count);
  }

  // ═══════════════════════════════════════════════════════════════════════════════
  // CreateWorkoutTemplate
  // ═══════════════════════════════════════════════════════════════════════════════

  [Fact]
  public async Task CreateWorkoutTemplate_PersistsAndReturnsTemplate()
  {
    // arrange
    var exerciseId = await SeedExerciseAsync("alice", Difficulty.High);
    var userInfo = UserInfoHelpers.CreateUserInfo("alice");
    var request = new CreateWorkoutTemplateRequest { Name = "Push Day", ExerciseIds = [exerciseId] };

    // act
    var result = await _sut.CreateWorkoutTemplate(request, userInfo);

    // assert
    Assert.True(result.Id > 0);
    Assert.Equal("Push Day", result.Name);
    Assert.Equal("alice", result.Creator);
    Assert.Single(result.Exercises);
    Assert.Equal(Difficulty.High, result.OverallDifficulty);
    Assert.True(await Context.WorkoutTemplates.AnyAsync(t => t.Id == result.Id));
  }

  [Fact]
  public async Task CreateWorkoutTemplate_PreservesExerciseOrder()
  {
    // arrange
    var ex1Id = await SeedExerciseAsync("alice", Difficulty.Easy);
    var ex2Id = await SeedExerciseAsync("alice", Difficulty.High);
    var userInfo = UserInfoHelpers.CreateUserInfo("alice");
    var request = new CreateWorkoutTemplateRequest { Name = "Full Body", ExerciseIds = [ex1Id, ex2Id] };

    // act
    var result = await _sut.CreateWorkoutTemplate(request, userInfo);

    // assert
    Assert.Equal(2, result.Exercises.Count);
    Assert.Equal(ex1Id, result.Exercises[0].Id);
    Assert.Equal(ex2Id, result.Exercises[1].Id);
  }

  [Fact]
  public async Task CreateWorkoutTemplate_ThrowsConflictException_WhenExerciseNotFound()
  {
    // arrange
    var request = new CreateWorkoutTemplateRequest { Name = "Ghost", ExerciseIds = [9999] };

    // act & assert
    await Assert.ThrowsAsync<ConflictException>(() =>
      _sut.CreateWorkoutTemplate(request, UserInfoHelpers.CreateUserInfo("alice"))
    );
  }

  // ═══════════════════════════════════════════════════════════════════════════════
  // UpdateWorkoutTemplate
  // ═══════════════════════════════════════════════════════════════════════════════

  [Fact]
  public async Task UpdateWorkoutTemplate_UpdatesTemplate_WhenUserIsOwner()
  {
    // arrange
    var ex1Id = await SeedExerciseAsync("alice", Difficulty.Easy);
    var ex2Id = await SeedExerciseAsync("alice", Difficulty.High);
    var created = await CreateTemplateAsync("Push Day", [ex1Id]);

    // act
    await _sut.UpdateWorkoutTemplate(
      new UpdateWorkoutTemplateRequest
      {
        Id = created.Id,
        Name = "Updated Push Day",
        Exercises = [ex1Id, ex2Id],
      },
      UserInfoHelpers.CreateUserInfo("alice")
    );

    // assert
    var updated = await Context
      .WorkoutTemplates.Include(t => t.Exercises)
        .ThenInclude(e => e.Exercise)
      .FirstAsync(t => t.Id == created.Id);
    Assert.Equal("Updated Push Day", updated.Name);
    Assert.Equal(2, updated.Exercises.Count);
  }

  [Fact]
  public async Task UpdateWorkoutTemplate_ThrowsNotFoundException_WhenTemplateDoesNotExist()
  {
    // arrange
    var request = new UpdateWorkoutTemplateRequest
    {
      Id = 9999,
      Name = "Ghost",
      Exercises = [],
    };

    // act & assert
    await Assert.ThrowsAsync<NotFoundException>(() =>
      _sut.UpdateWorkoutTemplate(request, UserInfoHelpers.CreateUserInfo("alice"))
    );
  }

  [Fact]
  public async Task UpdateWorkoutTemplate_ThrowsForbiddenException_WhenUserIsNotOwner()
  {
    // arrange
    var exerciseId = await SeedExerciseAsync("alice");
    var created = await CreateTemplateAsync("Push Day", [exerciseId]);

    var updateRequest = new UpdateWorkoutTemplateRequest
    {
      Id = created.Id,
      Name = "Hacked",
      Exercises = [exerciseId],
    };

    // act & assert
    await Assert.ThrowsAsync<ForbiddenException>(() =>
      _sut.UpdateWorkoutTemplate(updateRequest, UserInfoHelpers.CreateUserInfo("bob"))
    );
  }

  // ═══════════════════════════════════════════════════════════════════════════════
  // DeleteWorkoutTemplateById
  // ═══════════════════════════════════════════════════════════════════════════════

  [Fact]
  public async Task DeleteWorkoutTemplateById_RemovesTemplate_WhenUserIsOwner()
  {
    // arrange
    var exerciseId = await SeedExerciseAsync("alice");
    var created = await CreateTemplateAsync("Push Day", [exerciseId]);

    // act
    await _sut.DeleteWorkoutTemplateById(created.Id, UserInfoHelpers.CreateUserInfo("alice"));

    // assert
    Assert.False(await Context.WorkoutTemplates.AnyAsync(t => t.Id == created.Id));
  }

  [Fact]
  public async Task DeleteWorkoutTemplateById_ThrowsNotFoundException_WhenTemplateDoesNotExist()
  {
    // act & assert
    await Assert.ThrowsAsync<NotFoundException>(() =>
      _sut.DeleteWorkoutTemplateById(9999, UserInfoHelpers.CreateUserInfo("alice"))
    );
  }

  [Fact]
  public async Task DeleteWorkoutTemplateById_ThrowsForbiddenException_WhenUserIsNotOwner()
  {
    // arrange
    var exerciseId = await SeedExerciseAsync("alice");
    var created = await CreateTemplateAsync("Push Day", [exerciseId]);

    // act & assert
    await Assert.ThrowsAsync<ForbiddenException>(() =>
      _sut.DeleteWorkoutTemplateById(created.Id, UserInfoHelpers.CreateUserInfo("bob"))
    );
  }
}
