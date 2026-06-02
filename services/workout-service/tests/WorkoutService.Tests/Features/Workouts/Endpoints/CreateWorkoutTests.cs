using System.Net;
using System.Net.Http.Json;
using NSubstitute;
using WorkoutService.Common.Auth;
using WorkoutService.Features.Workouts.Dto;
using WorkoutService.Tests.Common;
using WorkoutService.Tests.Common.Helpers;

namespace WorkoutService.Tests.Features.Workouts.Endpoints;

public class CreateWorkoutTests(TestWebApplicationFactory factory) : EndpointTestBase(factory)
{
  private const string BaseUrl = "/api/workouts";

  [Fact]
  public async Task CreateWorkout_Returns201_WithLocationHeader()
  {
    // arrange
    var startedAt = DateTime.UtcNow.AddHours(-1);
    var completedAt = DateTime.UtcNow;
    var created = DtoHelpers.MakeWorkoutResponse(1, "alice", startedAt, completedAt);
    Factory.WorkoutsServiceMock.CreateWorkout(Arg.Any<CreateWorkoutRequest>(), Arg.Any<UserInfo>()).Returns(created);

    var client = CreateAuthenticatedClient();
    var request = new WorkoutResponse
    {
      Id = 1,
      Creator = "alice",
      Name = "Morning Workout",
      StartedAt = startedAt,
      CompletedAt = completedAt,
      Note = "Great session",
      Sets =
      [
        DtoHelpers.MakeWeightAndRepsSetResponse(1, 1),
        DtoHelpers.MakeWeightAndRepsSetResponse(1, 2),
        DtoHelpers.MakeDistanceAndTimeSetResponse(2, 1),
      ],
    };

    // act
    var response = await client.PostAsJsonAsync(BaseUrl, request);

    // assert
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    Assert.Contains("/1", response.Headers.Location?.ToString());

    var body = await response.Content.ReadFromJsonAsync<WorkoutResponse>(JsonOptions);
    Assert.NotNull(body);
    Assert.Equal(1, body.Id);
    Assert.Equal("alice", body.Creator);
    Assert.Equal("Morning Workout", body.Name);
    Assert.Equal(startedAt, body.StartedAt);
    Assert.Equal(completedAt, body.CompletedAt);
    Assert.Equal("Great session", body.Note);
    Assert.NotNull(body.Sets);
    Assert.Equal(3, body.Sets.Count);
  }
}
