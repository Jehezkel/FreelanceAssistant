using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class BidTemplate
    {
        public int Id { get; set; }
        [JsonIgnore]
        public AppUser? User { get; set; } = null!;
        [JsonIgnore]
        public string? UserId { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
