using System.Net;
using System.Net.Http.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WorkoutService.Common.Enums;
using WorkoutService.Common.Exceptions;
using WorkoutService.Features.ExerciseCatalog.Dtos;
using WorkoutService.Tests.Common;
using WorkoutService.Tests.Common.Helpers;

namespace WorkoutService.Tests.Features.ExerciseCatalog.Endpoints;

public class GetExerciseByIdTests(TestWebApplicationFactory factory) : EndpointTestBase(factory)
{
  private const string BaseUrl = "/api/exercise-catalog";

  [Fact]
  public async Task GetById_Returns200_WhenExerciseExists()
  {
    // arrange
    Factory.ExerciseServiceMock.GetExerciseById(1).Returns(DtoHelpers.MakeExerciseResponse(1, "alice"));
    var client = CreateAuthenticatedClient();

    // act
    var response = await client.GetAsync($"{BaseUrl}/1");

    // assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var body = await response.Content.ReadFromJsonAsync<ExerciseResponse>(JsonOptions);
    Assert.NotNull(body);
    Assert.Equal("Bench Press", body.Name);
    Assert.Equal(1, body.Id);
    Assert.Equal("alice", body.Creator);
    Assert.Equal(Difficulty.Medium, body.Difficulty);
    Assert.Equal([MuscleGroup.Chest, MuscleGroup.Triceps], body.MuscleGroups);
  }

  [Fact]
  public async Task GetById_Returns404_WhenExerciseDoesNotExist()
  {
    // arrange
    Factory
      .ExerciseServiceMock.GetExerciseById(999)
      .ThrowsAsync(new NotFoundException("Exercise with ID 999 not found"));

    var client = CreateAuthenticatedClient();

    // act
    var response = await client.GetAsync($"{BaseUrl}/999");

    // assert
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }
}
