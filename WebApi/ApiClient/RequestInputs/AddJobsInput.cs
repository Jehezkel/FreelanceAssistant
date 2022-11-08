using System.Text.Json.Serialization;

namespace WebApi.ApiClient.RequestInputs
{
    public class AddJobsInput
    {
        [JsonPropertyName("jobs")]
        public List<int> Jobs { get; set; }=new List<int>();
    }
}
