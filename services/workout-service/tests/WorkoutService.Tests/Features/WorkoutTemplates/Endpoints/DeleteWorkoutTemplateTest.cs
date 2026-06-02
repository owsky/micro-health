using System.Net;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Exceptions;
using WorkoutService.Tests.Common;

namespace WorkoutService.Tests.Features.WorkoutTemplates.Endpoints;

public class DeleteWorkoutTemplateTest(TestWebApplicationFactory factory) : EndpointTestBase(factory)
{
  private const string BaseUrl = "/api/workout-templates";

  [Fact]
  public async Task DeleteWorkoutTemplate_Returns204_WhenSuccessful()
  {
    // arrange
    Factory
      .WorkoutTemplatesServiceMock.DeleteWorkoutTemplateById(Arg.Any<long>(), Arg.Any<UserInfo>())
      .Returns(Task.CompletedTask);

    var client = CreateAuthenticatedClient();

    // act
    var response = await client.DeleteAsync($"{BaseUrl}/1");

    // assert
    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
  }

  [Fact]
  public async Task DeleteWorkoutTemplate_Returns403_WhenUserDoesNotOwnTemplate()
  {
    // arrange
    Factory
      .WorkoutTemplatesServiceMock.DeleteWorkoutTemplateById(Arg.Any<long>(), Arg.Any<UserInfo>())
      .ThrowsAsync(new ForbiddenException("User does not own the workout template"));

    var client = CreateAuthenticatedClient("bob");

    // act
    var response = await client.DeleteAsync($"{BaseUrl}/1");

    // assert
    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
  }
}
