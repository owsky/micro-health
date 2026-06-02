using System.Net;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Exceptions;
using WorkoutService.Tests.Common;

namespace WorkoutService.Tests.Features.ExerciseCatalog.Endpoints;

public class DeleteExerciseTests(TestWebApplicationFactory factory) : EndpointTestBase(factory)
{
  private const string BaseUrl = "/api/exercise-catalog";

  [Fact]
  public async Task Delete_Returns204_WhenSuccessful()
  {
    // arrange
    Factory.ExerciseServiceMock.DeleteExerciseById(Arg.Any<long>(), Arg.Any<UserInfo>()).Returns(Task.CompletedTask);

    var client = CreateAuthenticatedClient();

    // act
    var response = await client.DeleteAsync($"{BaseUrl}/1");

    // assert
    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
  }

  [Fact]
  public async Task Delete_Returns403_WhenUserDoesNotOwnExercise()
  {
    // arrange
    Factory
      .ExerciseServiceMock.DeleteExerciseById(Arg.Any<long>(), Arg.Any<UserInfo>())
      .ThrowsAsync(new ForbiddenException("User does not own the exercise"));

    var client = CreateAuthenticatedClient("bob");

    // act
    var response = await client.DeleteAsync($"{BaseUrl}/1");

    // assert
    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
  }
}
