using System.Net;
using System.Net.Http.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Exceptions;
using WorkoutService.Features.Workouts.Dto;
using WorkoutService.Tests.Common;

namespace WorkoutService.Tests.Features.Workouts.Endpoints;

public class UpdateWorkoutTests(TestWebApplicationFactory factory) : EndpointTestBase(factory)
{
  private const string BaseUrl = "/api/workouts";

  [Fact]
  public async Task UpdateWorkout_Returns204_WhenSuccessful()
  {
    // arrange
    var startedAt = DateTime.UtcNow.AddHours(-1);
    var completedAt = DateTime.UtcNow;
    var request = new UpdateWorkoutRequest
    {
      Name = "Updated Workout",
      StartedAt = startedAt,
      CompletedAt = completedAt,
      Note = "Updated note",
      Sets =
      [
        new WeightAndRepsSetRequest
        {
          ExerciseId = 1,
          SetNumber = 1,
          IsWarmup = false,
          Weight = 100,
          Reps = 10,
        },
        new WeightAndRepsSetRequest
        {
          ExerciseId = 1,
          SetNumber = 2,
          IsWarmup = false,
          Weight = 100,
          Reps = 10,
        },
      ],
    };

    Factory
      .WorkoutsServiceMock.UpdateWorkoutById(Arg.Any<long>(), Arg.Any<UpdateWorkoutRequest>(), Arg.Any<UserInfo>())
      .Returns(Task.CompletedTask);

    var client = CreateAuthenticatedClient();

    // act
    var response = await client.PutAsJsonAsync($"{BaseUrl}/1", request);

    // assert
    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
  }

  [Fact]
  public async Task UpdateWorkout_Returns404_WhenWorkoutNotFound()
  {
    // arrange
    var startedAt = DateTime.UtcNow.AddHours(-1);
    var completedAt = DateTime.UtcNow;
    var request = new UpdateWorkoutRequest
    {
      Name = "Updated Workout",
      StartedAt = startedAt,
      CompletedAt = completedAt,
      Note = "Updated note",
      Sets =
      [
        new WeightAndRepsSetRequest
        {
          ExerciseId = 1,
          SetNumber = 1,
          IsWarmup = false,
          Weight = 100,
          Reps = 10,
        },
      ],
    };

    Factory
      .WorkoutsServiceMock.UpdateWorkoutById(Arg.Any<long>(), Arg.Any<UpdateWorkoutRequest>(), Arg.Any<UserInfo>())
      .ThrowsAsync(new NotFoundException("Workout with ID 999 not found"));

    var client = CreateAuthenticatedClient();

    // act
    var response = await client.PutAsJsonAsync($"{BaseUrl}/999", request);

    // assert
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  [Fact]
  public async Task UpdateWorkout_Returns403_WhenUserDoesNotOwnWorkout()
  {
    // arrange
    var startedAt = DateTime.UtcNow.AddHours(-1);
    var completedAt = DateTime.UtcNow;
    var request = new UpdateWorkoutRequest
    {
      Name = "Updated Workout",
      StartedAt = startedAt,
      CompletedAt = completedAt,
      Note = "Updated note",
      Sets =
      [
        new WeightAndRepsSetRequest
        {
          ExerciseId = 1,
          SetNumber = 1,
          IsWarmup = false,
          Weight = 100,
          Reps = 10,
        },
      ],
    };

    Factory
      .WorkoutsServiceMock.UpdateWorkoutById(Arg.Any<long>(), Arg.Any<UpdateWorkoutRequest>(), Arg.Any<UserInfo>())
      .ThrowsAsync(new ForbiddenException("User alice does not own workout with ID 1"));

    var client = CreateAuthenticatedClient();

    // act
    var response = await client.PutAsJsonAsync($"{BaseUrl}/1", request);

    // assert
    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
  }
}
