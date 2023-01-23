using System.Text.Json.Serialization;
using WebApi.ApiClient.RequestParams;

namespace WebApi.FreelanceQueries;

public record ActiveProjectsInput
{
    [UseInRequestParameters("min_price")]
    public decimal? MinPrice { get; set; }
    [UseInRequestParameters("max_price")]
    public decimal? MaxPrice { get; set; }
    [UseInRequestParameters("jobs")]
    public List<int> Jobs { get; set; } = new List<int>();
}