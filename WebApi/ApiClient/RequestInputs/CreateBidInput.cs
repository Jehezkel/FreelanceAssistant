using System.Text.Json.Serialization;

namespace WebApi.ApiClient.RequestInputs
{
    public class CreateBidInput
    {
        [JsonPropertyName("project_id")]
        public int ProjectId { get; set; }
        [JsonPropertyName("bidder_id")]
        public int BidderId { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("period")]
        public int Period { get; set; } = 7;
        [JsonPropertyName("milestone_percentage")]
        public decimal MilestonePercentage { get; set; } = 100;
        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;
    }
}
