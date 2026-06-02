using System.Net;
using System.Net.Http.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Enums;
using WorkoutService.Common.Exceptions;
using WorkoutService.Features.ExerciseCatalog.Dtos;
using WorkoutService.Tests.Common;

namespace WorkoutService.Tests.Features.ExerciseCatalog.Endpoints;

public class UpdateExerciseByIdTests(TestWebApplicationFactory factory) : EndpointTestBase(factory)
{
  private const string BaseUrl = "/api/exercise-catalog";

  [Fact]
  public async Task Update_Returns204_WhenSuccessful()
  {
    // arrange
    Factory
      .ExerciseServiceMock.UpdateExerciseById(Arg.Any<long>(), Arg.Any<UpdateExerciseRequest>(), Arg.Any<UserInfo>())
      .Returns(Task.CompletedTask);

    var client = CreateAuthenticatedClient();
    var request = new UpdateExerciseRequest
    {
      Name = "Incline Bench Press",
      Difficulty = Difficulty.High,
      TrackingType = TrackingType.WeightAndReps,
      MuscleGroups = [MuscleGroup.Chest, MuscleGroup.Shoulders],
    };

    // act
    var response = await client.PutAsJsonAsync($"{BaseUrl}/1", request);

    // assert
    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
  }

  [Fact]
  public async Task Update_Returns403_WhenUserDoesNotOwnExercise()
  {
    // arrange
    Factory
      .ExerciseServiceMock.UpdateExerciseById(Arg.Any<long>(), Arg.Any<UpdateExerciseRequest>(), Arg.Any<UserInfo>())
      .ThrowsAsync(new ForbiddenException("User does not own the exercise"));

    var client = CreateAuthenticatedClient("bob");
    var request = new UpdateExerciseRequest
    {
      Name = "Hacked",
      Difficulty = Difficulty.Easy,
      TrackingType = TrackingType.Reps,
      MuscleGroups = [MuscleGroup.Biceps],
    };

    // act
    var response = await client.PutAsJsonAsync($"{BaseUrl}/1", request);

    // assert
    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
  }
}
