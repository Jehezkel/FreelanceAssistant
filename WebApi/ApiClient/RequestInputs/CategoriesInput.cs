using WebApi.ApiClient.RequestParams;

namespace WebApi.ApiClient.RequestInputs
{
    public record CategoriesInput
    {
        [UseInRequestParameters("categories")]
        public List<int> Categories { get; set; } = new();
        [UseInRequestParameters("lang")]
        public string? Lang { get; set; }
    }
}
