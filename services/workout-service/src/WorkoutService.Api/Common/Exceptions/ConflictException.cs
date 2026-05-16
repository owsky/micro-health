namespace WorkoutService.Common.Exceptions;

/// <summary>
/// Thrown when the requested operation would result in a conflict state. Maps to HTTP 409 Conflict.
/// </summary>
public class ConflictException(string message) : Exception(message);
