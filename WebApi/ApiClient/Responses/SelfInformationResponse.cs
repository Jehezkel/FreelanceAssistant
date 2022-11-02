using System.Text.Json.Serialization;

namespace WebApi.ApiClient.Responses
{
    public record SelfInformationResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; init; } = null!;
        [JsonPropertyName("result")]
        public UserResponse Result { get; set; } = null!;
    }
    public record UserResponse
    {
        [JsonPropertyName("username")]
        public string UserName { get; init; } = null!;
        [JsonPropertyName("id")]
        public int UserId { get; init; }
    }
        
}
