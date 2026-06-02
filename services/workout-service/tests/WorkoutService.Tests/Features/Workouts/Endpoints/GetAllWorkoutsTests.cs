using System.Net;
using System.Net.Http.Json;
using NSubstitute;
using WorkoutService.Common.Auth;
using WorkoutService.Features.Workouts.Dto;
using WorkoutService.Tests.Common;
using WorkoutService.Tests.Common.Helpers;

namespace WorkoutService.Tests.Features.Workouts.Endpoints;

public class GetAllWorkoutsTests(TestWebApplicationFactory factory) : EndpointTestBase(factory)
{
  private const string BaseUrl = "/api/workouts";

  [Fact]
  public async Task GetAllWorkouts_Returns200WithList()
  {
    // arrange
    var expected = new List<WorkoutResponse>
    {
      DtoHelpers.MakeWorkoutResponse(1, "alice", DateTime.UtcNow.AddHours(-1), DateTime.UtcNow),
      DtoHelpers.MakeWorkoutResponse(2, "alice", DateTime.UtcNow.AddHours(-1), DateTime.UtcNow),
    };
    Factory.WorkoutsServiceMock.GetAllWorkouts(Arg.Any<UserInfo>()).Returns(expected);

    var client = CreateAuthenticatedClient();

    // act
    var response = await client.GetAsync(BaseUrl);

    // assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var body = await response.Content.ReadFromJsonAsync<List<WorkoutResponse>>(JsonOptions);
    Assert.NotNull(body);
    Assert.Equal(2, body.Count);
  }
}
