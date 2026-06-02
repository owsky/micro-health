using System.Net;
using System.Net.Http.Json;
using NSubstitute;
using WorkoutService.Common.Auth;
using WorkoutService.Features.ExerciseCatalog.Dtos;
using WorkoutService.Tests.Common;
using WorkoutService.Tests.Common.Helpers;

namespace WorkoutService.Tests.Features.ExerciseCatalog.Endpoints;

public class GetAllExercisesTests(TestWebApplicationFactory factory) : EndpointTestBase(factory)
{
  private const string BaseUrl = "/api/exercise-catalog";

  [Fact]
  public async Task GetAll_Returns200WithList()
  {
    // arrange
    var expected = new List<ExerciseResponse>
    {
      DtoHelpers.MakeExerciseResponse(1, "alice"),
      DtoHelpers.MakeExerciseResponse(2, "alice"),
    };
    Factory
      .ExerciseServiceMock.GetAllExercises(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<UserInfo>())
      .Returns(expected);

    var client = CreateAuthenticatedClient();

    // act
    var response = await client.GetAsync(BaseUrl);

    // assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var body = await response.Content.ReadFromJsonAsync<List<ExerciseResponse>>(JsonOptions);
    Assert.NotNull(body);
    Assert.Equal(2, body.Count);
  }

  [Fact]
  public async Task GetAll_ForwardsQueryParameters_ToService()
  {
    // arrange
    Factory
      .ExerciseServiceMock.GetAllExercises(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<UserInfo>())
      .Returns([]);

    var client = CreateAuthenticatedClient();

    // act
    await client.GetAsync($"{BaseUrl}?pageSize=5&pageNumber=3&mine=true");

    // assert
    await Factory.ExerciseServiceMock.Received(1).GetAllExercises(5, 3, true, Arg.Any<UserInfo>());
  }
}
