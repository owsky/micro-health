using System.Net;
using System.Net.Http.Json;
using NSubstitute;
using WorkoutService.Common.Enums;
using WorkoutService.Features.WorkoutTemplates.Dtos;
using WorkoutService.Tests.Common;
using WorkoutService.Tests.Common.Helpers;

namespace WorkoutService.Tests.Features.WorkoutTemplates.Endpoints;

public class GetAllWorkoutTemplatesTest(TestWebApplicationFactory factory) : EndpointTestBase(factory)
{
  private const string BaseUrl = "/api/workout-templates";

  [Fact]
  public async Task GetAllWorkoutTemplates_Returns200WithTemplates_NoFilters()
  {
    // arrange
    var mockResponse = new List<WorkoutTemplateResponse>
    {
      DtoHelpers.MakeWorkoutTemplateResponse(1, "alice", "Push Day"),
      DtoHelpers.MakeWorkoutTemplateResponse(2, "bob", "Pull Day"),
    };

    Factory
      .WorkoutTemplatesServiceMock.GetAllWorkoutTemplates(
        pageSize: Arg.Any<int>(),
        pageNumber: Arg.Any<int>(),
        userInfo: Arg.Any<WorkoutService.Common.Auth.UserInfo>(),
        name: Arg.Any<string?>(),
        overallDifficulty: Arg.Any<Difficulty?>(),
        exerciseIds: Arg.Any<List<long>?>(),
        mine: Arg.Any<bool>()
      )
      .Returns(mockResponse);

    var client = CreateAuthenticatedClient();

    // act
    var response = await client.GetAsync(BaseUrl);

    // assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var body = await response.Content.ReadFromJsonAsync<List<WorkoutTemplateResponse>>(JsonOptions);
    Assert.NotNull(body);
    Assert.Equal(2, body.Count);
    Assert.Equal("Push Day", body[0].Name);
    Assert.Equal("Pull Day", body[1].Name);
  }

  [Fact]
  public async Task GetAllWorkoutTemplates_PassesParametersToService()
  {
    // arrange
    Factory
      .WorkoutTemplatesServiceMock.GetAllWorkoutTemplates(
        pageSize: 5,
        pageNumber: 2,
        userInfo: Arg.Any<WorkoutService.Common.Auth.UserInfo>(),
        name: "Legs",
        overallDifficulty: Difficulty.High,
        exerciseIds: Arg.Is<List<long>>(list => list != null && list.Count == 2 && list[0] == 10 && list[1] == 20),
        mine: true
      )
      .Returns([]);

    var client = CreateAuthenticatedClient();

    // act
    const string url =
      $"{BaseUrl}?pageSize=5&pageNumber=2&name=Legs&overallDifficulty=High&exercises=10&exercises=20&mine=true";
    var response = await client.GetAsync(url);

    // assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    // Verify service call received expected arguments
    await Factory
      .WorkoutTemplatesServiceMock.Received(1)
      .GetAllWorkoutTemplates(
        pageSize: 5,
        pageNumber: 2,
        userInfo: Arg.Any<WorkoutService.Common.Auth.UserInfo>(),
        name: "Legs",
        overallDifficulty: Difficulty.High,
        exerciseIds: Arg.Is<List<long>>(list => list != null && list.Count == 2 && list[0] == 10 && list[1] == 20),
        mine: true
      );
  }
}
