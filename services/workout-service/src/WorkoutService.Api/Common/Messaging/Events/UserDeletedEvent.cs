using System.Text.Json.Serialization;

namespace WorkoutService.Common.Messaging.Events;

public record UserDeletedEvent([property: JsonPropertyName("username")] string Username);
