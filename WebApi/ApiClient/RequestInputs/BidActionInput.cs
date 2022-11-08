using System.Text.Json.Serialization;
using WebApi.ApiClient.RequestParams;

namespace WebApi.ApiClient.RequestInputs
{
    public class BidActionInput : IHasRouteEndpointAddition
    {
        public BidActionInput()
        {

        }
        public BidActionInput(BidderAction bidderAction)
        {
            _bidderAction = bidderAction;
        }
        [JsonIgnore]
        public int BidId { get; set; }
        private BidderAction _bidderAction { get; set; }
        [JsonPropertyName("action")]
        public string Action { 
            get { return Enum.GetName(typeof(BidderAction), this._bidderAction)!; } 
            set { try
                {
                    this._bidderAction = Enum.Parse<BidderAction>(value);
                }
                catch (System.ArgumentException)
                {
                    throw new ArgumentException($"Value :{value} is not correct value for enum BidderAction");
                }
                } }
        public enum BidderAction
        {
            accept,
            deny,
            retract,
            highlight,
            sponsor

        }

        public string GetEndpointAddition()
        {
            return $"/{this.BidId}";
        }
    }
}
