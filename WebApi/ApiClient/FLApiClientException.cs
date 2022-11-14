using WebApi.ApiClient.Responses;

namespace WebApi.ApiClient
{
    public class FLApiClientException : Exception
    {
        public FLApiClientException()
        {
        }
        public FLApiClientException(string message):base(message)
        {

        }
        public FLApiClientException(ErrorResponse errorResponse) :base(errorResponse.Message)
        {
            this.ErrorResponse = errorResponse;
        }
        public ErrorResponse ErrorResponse { get; set; }
    }
}
