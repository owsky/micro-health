using System.Net;
using System.Net.Http.Json;
using NSubstitute;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Enums;
using WorkoutService.Features.ExerciseCatalog.Dtos;
using WorkoutService.Tests.Common;
using WorkoutService.Tests.Common.Helpers;

namespace WorkoutService.Tests.Features.ExerciseCatalog.Endpoints;

public class CreateExerciseTests(TestWebApplicationFactory factory) : EndpointTestBase(factory)
{
  private const string BaseUrl = "/api/exercise-catalog";

  [Fact]
  public async Task Create_Returns201_WithLocationHeader()
  {
    // arrange
    var created = DtoHelpers.MakeExerciseResponse(42, "alice");
    Factory.ExerciseServiceMock.CreateExercise(Arg.Any<CreateExerciseRequest>(), Arg.Any<UserInfo>()).Returns(created);

    var client = CreateAuthenticatedClient();
    var request = new ExerciseResponse()
    {
      Name = "Bench Press",
      Creator = "alice",
      Difficulty = Difficulty.Medium,
      TrackingType = TrackingType.WeightAndReps,
      MuscleGroups = [MuscleGroup.Chest, MuscleGroup.Triceps],
    };

    // act
    var response = await client.PostAsJsonAsync(BaseUrl, request);

    // assert
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    Assert.Contains("/42", response.Headers.Location?.ToString());

    var body = await response.Content.ReadFromJsonAsync<ExerciseResponse>(JsonOptions);
    Assert.NotNull(body);
    Assert.Equal(42, body.Id);
    Assert.Equal("Bench Press", body.Name);
    Assert.Equal("alice", body.Creator);
    Assert.Equal(Difficulty.Medium, body.Difficulty);
    Assert.Equal([MuscleGroup.Chest, MuscleGroup.Triceps], body.MuscleGroups);
  }
}
