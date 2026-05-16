namespace WorkoutService.Common.Exceptions;

/// <summary>
/// Thrown when the current user is not authorized to perform the requested operation. Maps to HTTP 403 Forbidden.
/// </summary>
public sealed class ForbiddenException(string message) : Exception(message);
