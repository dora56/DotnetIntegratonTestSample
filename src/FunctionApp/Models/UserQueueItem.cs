using System.Text.Json.Serialization;

namespace FunctionApp.Models;

public class UserQueueItem
{
    [JsonPropertyName("id")]
    public string Id { get; init; }
    
    [JsonPropertyName("name")]
    public string Name { get; init; }
}
