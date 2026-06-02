using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Exceptions;

namespace WorkoutService.Tests.Common.Auth;

public class UserInfoMiddlewareTest
{
  private readonly ILogger<UserInfoMiddleware> _logger = Substitute.For<ILogger<UserInfoMiddleware>>();
  private static readonly string[] Roles = ["admin", "user"];

  private static string CreateBase64EncodedUserInfo(string preferredUsername)
  {
    var json = JsonSerializer.Serialize(new { preferred_username = preferredUsername });
    return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
  }

  [Fact]
  public async Task InvokeAsync_SetsUserClaims_WhenValidXUserinfoHeaderIsPresent()
  {
    // arrange
    var middleware = new UserInfoMiddleware(_logger);
    var context = new DefaultHttpContext();
    context.Request.Headers["X-Userinfo"] = CreateBase64EncodedUserInfo("test-user");
    var nextCalled = false;

    // act
    await middleware.InvokeAsync(context, Next);

    // assert
    Assert.True(nextCalled);
    Assert.NotNull(context.User);
    Assert.Equal("test-user", context.User.FindFirstValue(ClaimTypes.Name));
    Assert.Equal("Gateway", context.User.Identity?.AuthenticationType);
    return;

    Task Next(HttpContext _)
    {
      nextCalled = true;
      return Task.CompletedTask;
    }
  }

  [Fact]
  public async Task InvokeAsync_ThrowsForbiddenException_WhenXUserinfoHeaderIsMissing()
  {
    // arrange
    var middleware = new UserInfoMiddleware(_logger);
    var context = new DefaultHttpContext();

    // act && assert
    await Assert.ThrowsAsync<ForbiddenException>(() => middleware.InvokeAsync(context, Next));
    return;

    Task Next(HttpContext _)
    {
      return Task.CompletedTask;
    }
  }

  [Fact]
  public async Task InvokeAsync_ThrowsException_WhenXUserinfoHeaderIsInvalidBase64()
  {
    // arrange
    var middleware = new UserInfoMiddleware(_logger);
    var context = new DefaultHttpContext();
    context.Request.Headers["X-Userinfo"] = "invalid-base64!!!";
    RequestDelegate next = _ => Task.CompletedTask;

    // act & assert
    var exception = await Assert.ThrowsAsync<FormatException>(() => middleware.InvokeAsync(context, next));
    Assert.NotNull(exception);
  }

  [Fact]
  public async Task InvokeAsync_ThrowsException_WhenXUserinfoHeaderIsInvalidJson()
  {
    // arrange
    var middleware = new UserInfoMiddleware(_logger);
    var context = new DefaultHttpContext();
    var invalidJson = Convert.ToBase64String(Encoding.UTF8.GetBytes("{invalid-json}"));
    context.Request.Headers["X-Userinfo"] = invalidJson;
    RequestDelegate next = _ => Task.CompletedTask;

    // act & assert
    var exception = await Assert.ThrowsAsync<JsonException>(() => middleware.InvokeAsync(context, next));
    Assert.NotNull(exception);
  }

  [Fact]
  public async Task InvokeAsync_ThrowsForbiddenException_WhenPreferredUsernameIsMissing()
  {
    // arrange
    var middleware = new UserInfoMiddleware(_logger);
    var context = new DefaultHttpContext();
    var json = JsonSerializer.Serialize(new { other_field = "value" });
    context.Request.Headers["X-Userinfo"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

    // act && assert
    await Assert.ThrowsAsync<ForbiddenException>(() => middleware.InvokeAsync(context, Next));
    return;

    Task Next(HttpContext _)
    {
      return Task.CompletedTask;
    }
  }

  [Fact]
  public async Task InvokeAsync_CallsNextDelegate_InAllSuccessfulScenarios()
  {
    // arrange
    var middleware = new UserInfoMiddleware(_logger);
    var context = new DefaultHttpContext();
    context.Request.Headers["X-Userinfo"] = CreateBase64EncodedUserInfo("alice");
    var nextCalled = false;

    // act
    await middleware.InvokeAsync(context, Next);

    // assert
    Assert.True(nextCalled);
    return;

    Task Next(HttpContext _)
    {
      nextCalled = true;
      return Task.CompletedTask;
    }
  }

  [Fact]
  public async Task InvokeAsync_HandlesMultipleClaimsScenarios_WithComplexJson()
  {
    // arrange
    var middleware = new UserInfoMiddleware(_logger);
    var context = new DefaultHttpContext();
    var json = JsonSerializer.Serialize(
      new
      {
        preferred_username = "complex_user",
        email = "test@example.com",
        roles = Roles,
      }
    );
    context.Request.Headers["X-Userinfo"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
    var nextCalled = false;

    // act
    await middleware.InvokeAsync(context, Next);

    // assert
    Assert.True(nextCalled);
    Assert.NotNull(context.User);
    Assert.Equal("complex_user", context.User.FindFirstValue(ClaimTypes.Name));
    return;

    Task Next(HttpContext _)
    {
      nextCalled = true;
      return Task.CompletedTask;
    }
  }
}
