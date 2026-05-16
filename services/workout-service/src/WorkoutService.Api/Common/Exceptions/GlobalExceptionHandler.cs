using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WorkoutService.Common.Exceptions;

namespace WorkoutService.Common.Exceptions;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
  public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken
  )
  {
    var (statusCode, title) = exception switch
    {
      NotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
      ForbiddenException => (StatusCodes.Status403Forbidden, "Forbidden"),
      ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
      _ => (StatusCodes.Status500InternalServerError, "Internal Server Error"),
    };

    if (statusCode == StatusCodes.Status500InternalServerError)
      logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
    else
      logger.LogWarning(exception, "{Title}: {Message}", title, exception.Message);

    var problem = new ProblemDetails
    {
      Status = statusCode,
      Title = title,
      Detail = exception.Message,
    };

    httpContext.Response.StatusCode = statusCode;
    await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
    return true;
  }
}
