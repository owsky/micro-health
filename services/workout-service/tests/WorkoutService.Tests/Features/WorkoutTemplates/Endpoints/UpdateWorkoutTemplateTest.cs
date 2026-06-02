using System.Net;
using System.Net.Http.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Exceptions;
using WorkoutService.Features.WorkoutTemplates.Dtos;
using WorkoutService.Tests.Common;

namespace WorkoutService.Tests.Features.WorkoutTemplates.Endpoints;

public class UpdateWorkoutTemplateTest(TestWebApplicationFactory factory) : EndpointTestBase(factory)
{
  private const string BaseUrl = "/api/workout-templates";

  [Fact]
  public async Task UpdateWorkoutTemplate_Returns204_WhenSuccessful()
  {
    // arrange
    Factory
      .WorkoutTemplatesServiceMock.UpdateWorkoutTemplate(Arg.Any<UpdateWorkoutTemplateRequest>(), Arg.Any<UserInfo>())
      .Returns(Task.CompletedTask);

    var client = CreateAuthenticatedClient();
    var request = new UpdateWorkoutTemplateRequest
    {
      Id = 1,
      Name = "Updated Push Day",
      Exercises = [1, 2, 4],
    };

    // act
    var response = await client.PutAsJsonAsync($"{BaseUrl}/1", request);

    // assert
    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
  }

  [Fact]
  public async Task UpdateWorkoutTemplate_Returns403_WhenUserDoesNotOwnTemplate()
  {
    // arrange
    Factory
      .WorkoutTemplatesServiceMock.UpdateWorkoutTemplate(Arg.Any<UpdateWorkoutTemplateRequest>(), Arg.Any<UserInfo>())
      .ThrowsAsync(new ForbiddenException("User does not own the workout template"));

    var client = CreateAuthenticatedClient("bob");
    var request = new UpdateWorkoutTemplateRequest
    {
      Id = 1,
      Name = "Hacked Template",
      Exercises = [1],
    };

    // act
    var response = await client.PutAsJsonAsync($"{BaseUrl}/1", request);

    // assert
    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
  }

  [Fact]
  public async Task UpdateWorkoutTemplate_Returns404_WhenTemplateDoesNotExist()
  {
    // arrange
    Factory
      .WorkoutTemplatesServiceMock.UpdateWorkoutTemplate(Arg.Any<UpdateWorkoutTemplateRequest>(), Arg.Any<UserInfo>())
      .ThrowsAsync(new NotFoundException("Workout template with ID 999 not found"));

    var client = CreateAuthenticatedClient();
    var request = new UpdateWorkoutTemplateRequest
    {
      Id = 999,
      Name = "Non-existent Template",
      Exercises = [1],
    };

    // act
    var response = await client.PutAsJsonAsync($"{BaseUrl}/999", request);

    // assert
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }
}
