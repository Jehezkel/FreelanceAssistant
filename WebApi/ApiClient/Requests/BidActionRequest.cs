using WebApi.ApiClient.RequestInputs;

namespace WebApi.ApiClient.Requests
{
    public class BidActionRequest : BaseRequest<BidActionInput>
    {
        public BidActionRequest()
        {

        }
        //public BidActionRequest(int BidId)
        //{
        //    this.BidId = BidId;
        //}
        //public int BidId { get; set; }
        public override string EndpointUrl => $"projects/0.1/bids";

        public override HttpMethod Method => HttpMethod.Put;
    }
}
