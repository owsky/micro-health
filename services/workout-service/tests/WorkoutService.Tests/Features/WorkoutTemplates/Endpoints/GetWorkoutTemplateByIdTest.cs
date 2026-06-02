using System.Net;
using System.Net.Http.Json;
using NSubstitute;
using WorkoutService.Common.Enums;
using WorkoutService.Features.WorkoutTemplates.Dtos;
using WorkoutService.Tests.Common;
using WorkoutService.Tests.Common.Helpers;

namespace WorkoutService.Tests.Features.WorkoutTemplates.Endpoints;

public class GetWorkoutTemplateByIdTest(TestWebApplicationFactory factory) : EndpointTestBase(factory)
{
  private const string BaseUrl = "/api/workout-templates";

  [Fact]
  public async Task GetWorkoutTemplateById_Returns200_WhenTemplateExists()
  {
    // arrange
    Factory
      .WorkoutTemplatesServiceMock.GetWorkoutTemplateById(1)
      .Returns(DtoHelpers.MakeWorkoutTemplateResponse(1, "alice", "Push Day"));

    var client = CreateAuthenticatedClient();

    // act
    var response = await client.GetAsync($"{BaseUrl}/1");

    // assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var body = await response.Content.ReadFromJsonAsync<WorkoutTemplateResponse>(JsonOptions);
    Assert.NotNull(body);
    Assert.Equal(1, body.Id);
    Assert.Equal("Push Day", body.Name);
    Assert.Equal("alice", body.Creator);
    Assert.Equal(Difficulty.Medium, body.OverallDifficulty);
    Assert.Equal(2, body.Exercises.Count);
  }

  [Fact]
  public async Task GetWorkoutTemplateById_Returns404_WhenTemplateDoesNotExist()
  {
    // arrange
    Factory.WorkoutTemplatesServiceMock.GetWorkoutTemplateById(999).Returns((WorkoutTemplateResponse?)null);

    var client = CreateAuthenticatedClient();

    // act
    var response = await client.GetAsync($"{BaseUrl}/999");

    // assert
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }
}
