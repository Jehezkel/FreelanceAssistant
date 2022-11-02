using System.Text.Json.Serialization;

namespace WebApi.ApiClient.Responses
{
    public class ResponseWrapper<T>
    {
        [JsonPropertyName("status")]
        public string Status { get; init; } = null!;
        [JsonPropertyName("result")]
        public T? Result { get; init; }
    }
}
