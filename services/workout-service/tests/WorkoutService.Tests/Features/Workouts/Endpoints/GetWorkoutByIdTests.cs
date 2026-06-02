using System.Net;
using System.Net.Http.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Exceptions;
using WorkoutService.Features.Workouts.Dto;
using WorkoutService.Tests.Common;
using WorkoutService.Tests.Common.Helpers;

namespace WorkoutService.Tests.Features.Workouts.Endpoints;

public class GetWorkoutByIdTests(TestWebApplicationFactory factory) : EndpointTestBase(factory)
{
  private const string BaseUrl = "/api/workouts";

  [Fact]
  public async Task GetWorkoutById_Returns200_WhenWorkoutExists()
  {
    // arrange
    var startedAt = DateTime.UtcNow.AddHours(-1);
    var completedAt = DateTime.UtcNow;
    var expected = DtoHelpers.MakeWorkoutResponse(1, "alice", startedAt, completedAt);
    Factory.WorkoutsServiceMock.GetWorkoutById(Arg.Any<long>(), Arg.Any<UserInfo>()).Returns(expected);
    var client = CreateAuthenticatedClient();

    // act
    var response = await client.GetAsync($"{BaseUrl}/1");

    // assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var body = await response.Content.ReadFromJsonAsync<WorkoutResponse>(JsonOptions);
    Assert.NotNull(body);
    Assert.Equal(1, body.Id);
    Assert.Equal("alice", body.Creator);
    Assert.Equal("Morning Workout", body.Name);
    Assert.Equal(startedAt, body.StartedAt);
    Assert.Equal(completedAt, body.CompletedAt);
  }

  [Fact]
  public async Task GetWorkoutById_Returns404_WhenWorkoutNotFound()
  {
    // arrange
    Factory
      .WorkoutsServiceMock.GetWorkoutById(Arg.Any<long>(), Arg.Any<UserInfo>())
      .ThrowsAsync(new NotFoundException("Workout with ID 999 not found"));

    var client = CreateAuthenticatedClient();

    // act
    var response = await client.GetAsync($"{BaseUrl}/999");

    // assert
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  [Fact]
  public async Task GetWorkoutById_Returns403_WhenUserDoesNotOwnWorkout()
  {
    // arrange
    Factory
      .WorkoutsServiceMock.GetWorkoutById(Arg.Any<long>(), Arg.Any<UserInfo>())
      .ThrowsAsync(new ForbiddenException("User alice does not own workout with ID 1"));

    var client = CreateAuthenticatedClient();

    // act
    var response = await client.GetAsync($"{BaseUrl}/1");

    // assert
    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
  }
}
