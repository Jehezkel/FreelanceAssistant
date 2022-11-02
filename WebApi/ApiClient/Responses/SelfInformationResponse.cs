using System.Text.Json.Serialization;

namespace WebApi.ApiClient.Responses;


public record SelfInformationResponse
{
    [JsonPropertyName("username")]
    public string UserName { get; init; } = null!;
    [JsonPropertyName("id")]
    public int UserId { get; init; }
}

