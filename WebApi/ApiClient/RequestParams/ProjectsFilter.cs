using System.Text.Json.Serialization;
using WebApi.ApiClient.RequestParams;

namespace WebApi.FreelanceQueries;

public record ProjectsFilter
{
    [JsonIgnore]
    [UseInRequestParameters]
    public decimal? MinPrice { get; set; }
    [UseInRequestParameters]
    public decimal? MaxPrice { get; set; }
    public decimal MaxPricex { get; set; }
    //   [UseInRequestParameters]
    // public IReadOnlyList<int> Jobs { get; set; } = new List<int>();
}