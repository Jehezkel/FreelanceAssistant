using WebApi.ApiClient.RequestParams;

namespace WebApi.ApiClient.RequestInputs
{
    public record JobsInput
    {
        public List<int> Jobs { get; set; } = new();
        [UseInRequestParameters("Categories[]")]
        [FormatValue("{0}", ",")]
        public List<int> Categories { get; set; } = new List<int> { 1 };
    }
}
