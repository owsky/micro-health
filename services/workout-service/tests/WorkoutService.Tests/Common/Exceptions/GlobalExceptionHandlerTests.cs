using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Exceptions;

namespace WorkoutService.Tests.Common.Exceptions;

public class GlobalExceptionHandlerTests(TestWebApplicationFactory factory) : EndpointTestBase(factory)
{
  // use an existing endpoint as the trigger surface.
  private const string WorkoutUrl = "/api/workouts/1";

  [Fact]
  public async Task TryHandleAsync_Returns404WithProblemDetails_ForNotFoundException()
  {
    // arrange
    const string message = "Workout not found";
    Factory
      .WorkoutsServiceMock.GetWorkoutById(Arg.Any<long>(), Arg.Any<UserInfo>())
      .ThrowsAsync(new NotFoundException(message));
    var client = CreateAuthenticatedClient();

    // act
    var response = await client.GetAsync(WorkoutUrl);

    // assert
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
    Assert.NotNull(problem);
    Assert.Equal(404, problem.Status);
    Assert.Equal("Not Found", problem.Title);
    Assert.Equal(message, problem.Detail);
  }

  [Fact]
  public async Task TryHandleAsync_Returns403WithProblemDetails_ForForbiddenException()
  {
    // arrange
    const string message = "Access denied";
    Factory
      .WorkoutsServiceMock.GetWorkoutById(Arg.Any<long>(), Arg.Any<UserInfo>())
      .ThrowsAsync(new ForbiddenException(message));
    var client = CreateAuthenticatedClient();

    // act
    var response = await client.GetAsync(WorkoutUrl);

    // assert
    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

    var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
    Assert.NotNull(problem);
    Assert.Equal(403, problem.Status);
    Assert.Equal("Forbidden", problem.Title);
    Assert.Equal(message, problem.Detail);
  }

  [Fact]
  public async Task TryHandleAsync_Returns409WithProblemDetails_ForConflictException()
  {
    // arrange
    const string message = "Resource already exists";
    Factory
      .WorkoutsServiceMock.GetWorkoutById(Arg.Any<long>(), Arg.Any<UserInfo>())
      .ThrowsAsync(new ConflictException(message));
    var client = CreateAuthenticatedClient();

    // act
    var response = await client.GetAsync(WorkoutUrl);

    // assert
    Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

    var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
    Assert.NotNull(problem);
    Assert.Equal(409, problem.Status);
    Assert.Equal("Conflict", problem.Title);
    Assert.Equal(message, problem.Detail);
  }

  [Fact]
  public async Task TryHandleAsync_Returns500WithProblemDetails_ForUnhandledException()
  {
    // arrange
    const string message = "Something went wrong internally";
    Factory
      .WorkoutsServiceMock.GetWorkoutById(Arg.Any<long>(), Arg.Any<UserInfo>())
      .ThrowsAsync(new InvalidOperationException(message));
    var client = CreateAuthenticatedClient();

    // act
    var response = await client.GetAsync(WorkoutUrl);

    // assert
    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

    var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
    Assert.NotNull(problem);
    Assert.Equal(500, problem.Status);
    Assert.Equal("Internal Server Error", problem.Title);
    Assert.Equal(message, problem.Detail);
  }

  [Theory]
  [InlineData(typeof(NotFoundException), 404)]
  [InlineData(typeof(ForbiddenException), 403)]
  [InlineData(typeof(ConflictException), 409)]
  public async Task TryHandleAsync_ResponseStatusCode_MatchesProblemDetailsStatus(
    Type exceptionType,
    int expectedStatus
  )
  {
    // arrange
    var exception = (Exception)Activator.CreateInstance(exceptionType, "test message")!;
    Factory.WorkoutsServiceMock.GetWorkoutById(Arg.Any<long>(), Arg.Any<UserInfo>()).ThrowsAsync(exception);
    var client = CreateAuthenticatedClient();

    // act
    var response = await client.GetAsync(WorkoutUrl);
    var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);

    // assert
    Assert.Equal(expectedStatus, (int)response.StatusCode);
    Assert.NotNull(problem);
    Assert.Equal(expectedStatus, problem.Status);
  }

  [Fact]
  public async Task TryHandleAsync_ExceptionMessageIsPreservedInDetail()
  {
    // arrange
    const string detailedMessage = "Workout with ID 42 was not found in the database";
    Factory
      .WorkoutsServiceMock.GetWorkoutById(Arg.Any<long>(), Arg.Any<UserInfo>())
      .ThrowsAsync(new NotFoundException(detailedMessage));
    var client = CreateAuthenticatedClient();

    // act
    var response = await client.GetAsync(WorkoutUrl);

    // assert
    var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
    Assert.NotNull(problem);
    Assert.Equal(detailedMessage, problem.Detail);
  }
}
