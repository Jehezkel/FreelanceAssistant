using System.Text.Json.Serialization;

namespace WebApi.ApiClient.Responses
{
    public class BidResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("project_id")]
        public int ProjectId { get; set; }
        [JsonPropertyName("retracted")]
        public bool Retracted { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; } = "";
        [JsonPropertyName("time_submitted")]
        public int TimeSubmitted { get; set; }
        [JsonPropertyName("award_status")]
        public string AwardStatus { get; set; } = "";
        [JsonPropertyName("complete_status")]
        public string CompleteStatus { get; set; } = "";
    }
}
