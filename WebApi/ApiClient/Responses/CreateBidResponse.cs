using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebApi.ApiClient.Responses;
public class CreateBidResponse
{
    [JsonPropertyName("status")]
    public string Status { get; init; } = null!;
    [JsonPropertyName("retracted")]
    public bool Retracted { get; init; }
    [JsonPropertyName("amount")]
    public decimal Amount { get; init; }
    [JsonPropertyName("description")]
    public string Description { get; init; } = null!;
}


