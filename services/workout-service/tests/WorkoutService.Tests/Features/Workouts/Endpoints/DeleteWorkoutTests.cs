using System.Net;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Exceptions;
using WorkoutService.Tests.Common;

namespace WorkoutService.Tests.Features.Workouts.Endpoints;

public class DeleteWorkoutTests(TestWebApplicationFactory factory) : EndpointTestBase(factory)
{
  private const string BaseUrl = "/api/workouts";

  [Fact]
  public async Task DeleteWorkout_Returns204_WhenSuccessful()
  {
    // arrange
    Factory.WorkoutsServiceMock.DeleteWorkoutById(Arg.Any<long>(), Arg.Any<UserInfo>()).Returns(Task.CompletedTask);

    var client = CreateAuthenticatedClient();

    // act
    var response = await client.DeleteAsync($"{BaseUrl}/1");

    // assert
    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
  }

  [Fact]
  public async Task DeleteWorkout_Returns403_WhenUserDoesNotOwnExercise()
  {
    // arrange
    Factory
      .WorkoutsServiceMock.DeleteWorkoutById(Arg.Any<long>(), Arg.Any<UserInfo>())
      .ThrowsAsync(new ForbiddenException("User does not own the workout"));

    var client = CreateAuthenticatedClient();

    // act
    var response = await client.DeleteAsync($"{BaseUrl}/1");

    // assert
    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
  }
}
