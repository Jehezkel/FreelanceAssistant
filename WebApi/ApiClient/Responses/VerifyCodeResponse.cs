using System.Text.Json.Serialization;

namespace WebApi.ApiClient.Responses;
public record VerifyCodeResponse
{
    [JsonPropertyName("scope")]
    public string Scope { get; init; } = null!;
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = null!;
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; init; } = null!;
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; init; }
    [JsonPropertyName("token_type")]
    public string TokenType { get; init; } = null!;
}