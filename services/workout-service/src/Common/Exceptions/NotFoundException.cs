namespace WorkoutService.Common.Exceptions;

/// <summary>
/// Thrown when a requested resource does not exist. Maps to HTTP 404 Not Found.
/// </summary>
public sealed class NotFoundException(string message) : Exception(message);
