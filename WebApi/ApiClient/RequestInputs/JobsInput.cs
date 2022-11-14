namespace WebApi.ApiClient.RequestInputs
{
    public record JobsInput
    {
        public List<int> Jobs { get; set; } = new();
        public List<int> Categories { get; set; } = new();
    }
}
