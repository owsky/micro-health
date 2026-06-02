using System.Net;
using System.Net.Http.Json;
using NSubstitute;
using WorkoutService.Common.Auth;
using WorkoutService.Features.WorkoutTemplates.Dtos;
using WorkoutService.Tests.Common;
using WorkoutService.Tests.Common.Helpers;

namespace WorkoutService.Tests.Features.WorkoutTemplates.Endpoints;

public class CreateWorkoutTemplateTest(TestWebApplicationFactory factory) : EndpointTestBase(factory)
{
  private const string BaseUrl = "/api/workout-templates";

  [Fact]
  public async Task CreateWorkoutTemplate_Returns201_WithLocationHeader()
  {
    // arrange
    var created = DtoHelpers.MakeWorkoutTemplateResponse(42, "alice", "Push Day");
    Factory
      .WorkoutTemplatesServiceMock.CreateWorkoutTemplate(Arg.Any<CreateWorkoutTemplateRequest>(), Arg.Any<UserInfo>())
      .Returns(created);

    var client = CreateAuthenticatedClient();
    var request = new CreateWorkoutTemplateRequest { Name = "Push Day", ExerciseIds = [1, 2, 3] };

    // act
    var response = await client.PostAsJsonAsync(BaseUrl, request);

    // assert
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    Assert.Contains("/42", response.Headers.Location?.ToString());

    var body = await response.Content.ReadFromJsonAsync<WorkoutTemplateResponse>(JsonOptions);
    Assert.NotNull(body);
    Assert.Equal(42, body.Id);
    Assert.Equal("Push Day", body.Name);
    Assert.Equal("alice", body.Creator);
  }
}
