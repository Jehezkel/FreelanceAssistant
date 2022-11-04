using System.Net;

namespace WebApi.ApiClient.Responses
{
    public class ErrorResponse
    {

        public HttpStatusCode StatusCode { get; set; }
        public string Status { get; set; } = "";
        public string Message { get; set; } = "";
        public string ErrorCode { get; set; } = "";
        public string RequestId { get; set; } = "";
    }
}

//{
//    "status":"error",
//"message":"You have already bid on that project.",
//"error_code":"ProjectExceptionCodes.DUPLICATE_BID",
//"request_id":"2190c61b75c443fc319ac93a4e0accb1"
//}