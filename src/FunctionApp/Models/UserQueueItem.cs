using System.Text.Json.Serialization;

namespace FunctionApp.Models;

public record UserQueueItem([property: JsonPropertyName("name")]string Name);
