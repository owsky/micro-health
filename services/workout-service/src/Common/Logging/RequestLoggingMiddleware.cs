using System.Diagnostics;

namespace WorkoutService.Common.Logging;

/// <summary>
/// Middleware that logs incoming HTTP requests and their outcomes.
/// Requests to paths listed in <see cref="SkippedPaths"/> are passed through without logging.
/// </summary>
/// <param name="next">The next middleware in the pipeline.</param>
/// <param name="logger">The logger instance used to emit log messages.</param>
public partial class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
  private static readonly HashSet<string> SkippedPaths = ["/healthz"];

  /// <summary>
  /// Processes an HTTP request: logs its arrival, invokes the next middleware,
  /// then logs the response status and elapsed time. Exceptions are logged before being re-thrown.
  /// </summary>
  /// <param name="context">The current HTTP context.</param>
  public async Task InvokeAsync(HttpContext context)
  {
    if (SkippedPaths.Contains(context.Request.Path.Value ?? ""))
    {
      await next(context);
      return;
    }

    var request = context.Request;
    var stopwatch = Stopwatch.StartNew();

    Log.RequestReceived(logger, request.Method, request.Path.Value ?? "", request.QueryString);

    try
    {
      await next(context);
      stopwatch.Stop();
      Log.RequestCompleted(
        logger,
        request.Method,
        request.Path.Value ?? "",
        context.Response.StatusCode,
        stopwatch.ElapsedMilliseconds
      );
    }
    catch (Exception ex)
    {
      stopwatch.Stop();
      Log.RequestFailed(logger, ex, request.Method, request.Path.Value ?? "", stopwatch.ElapsedMilliseconds);
      throw;
    }
  }

  /// <summary>
  /// Source-generated, high-performance log methods produced by the <see cref="LoggerMessageAttribute"/> generator.
  /// Each method checks whether the target log level is enabled before allocating any strings.
  /// </summary>
  private static partial class Log
  {
    [LoggerMessage(Level = LogLevel.Information, Message = "Request received: {Method} {Path}{Query}")]
    public static partial void RequestReceived(ILogger logger, string method, string path, QueryString query);

    [LoggerMessage(
      Level = LogLevel.Information,
      Message = "Request completed: {Method} {Path} responded {StatusCode} in {Duration}ms"
    )]
    public static partial void RequestCompleted(
      ILogger logger,
      string method,
      string path,
      int statusCode,
      long duration
    );

    [LoggerMessage(Level = LogLevel.Error, Message = "{Method} {Path} threw an exception in {Duration}ms")]
    public static partial void RequestFailed(ILogger logger, Exception ex, string method, string path, long duration);
  }
}
