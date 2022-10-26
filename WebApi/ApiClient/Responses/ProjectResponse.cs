using System.Text.Json.Serialization;

namespace WebApi.ApiClient.Responses;
public record ProjectSearchResponse
{
    [JsonPropertyName("status")]
    public string Status { get; init; } = null!;
    [JsonPropertyName("result")]
    public ProjectsResultResponse Result { get; init; } = null!;
}
public class ProjectsResultResponse
{
    [JsonPropertyName("projects")]
    public IReadOnlyList<ProjectResponse> Projects { get; init; } = new List<ProjectResponse>();
}
public class ProjectResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    [JsonPropertyName("title")]
    public string Title { get; init; } = null!;
    [JsonPropertyName("preview_description")]
    public string PreviewDescription { get; init; } = null!;
    [JsonPropertyName("type")]
    public string Type { get; init; } = null!;
    [JsonPropertyName("budget")]
    public BudgetResponse Budget { get; init; } = new BudgetResponse();
    [JsonPropertyName("currency")]
    public CurrencyResponse Currency { get; init; } = new CurrencyResponse();
    [JsonPropertyName("bid_stats")]
    public BidStatsResponse BidStats { get; init; } = new BidStatsResponse();

}
public record BudgetResponse
{
    
    [JsonPropertyName("minimum")]
    public decimal? Minimum { get; init; }
    [JsonPropertyName("maximum")]
    public decimal? Maximum { get; init; }
}
public record CurrencyResponse
{
    [JsonPropertyName("code")]
    public string Code { get; init; } = null!;
    [JsonPropertyName("sign")]
    public string Sign { get; init; } = null!;
    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;
    [JsonPropertyName("exchange_rate")]
    public decimal? ExchangeRate { get; init; }
    [JsonPropertyName("country")]
    public string Country { get; init; } = null!;
}
public record BidStatsResponse
{
    [JsonPropertyName("bid_count")]
    public int? BidCount { get; init; }
    [JsonPropertyName("bid_avg")]
    public decimal? BidAvg { get; init; }
}