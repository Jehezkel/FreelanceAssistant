using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApi.ApiClient.Requests;

public abstract class BaseRequest<T>
{
    // public HttpMethod Method { get; set; }
    private static List<HttpMethod> ReqBodyMethods => new List<HttpMethod> { HttpMethod.Post, HttpMethod.Patch, HttpMethod.Put };
    public abstract string EndpointUrl { get; }
    public abstract HttpMethod Method { get; }
    public Dictionary<string, string?> QueryParameters { get; set; } = new Dictionary<string, string?>();
    public HttpRequestMessage GetHttpRequest(string? Token)
    {
        Uri endpointUri = new Uri(this.FullEndpoint);
        var request = new HttpRequestMessage();
        request.RequestUri = endpointUri;
        request.Method = this.Method;
        var searchAsJson = JsonSerializer.Serialize(this.SearchObject);
        // var test = new JsonContent();
        if (this.SearchObject is not null && ReqBodyMethods.Contains(this.Method))
            request.Content = new StringContent(searchAsJson.ToString(), Encoding.UTF8, "application/json");
        return request;
    }
    public T? SearchObject { get; set; }
    public string FullEndpoint
    {
        get
        {
            if (this.QueryParameters is not null)
            {

                return QueryHelpers.AddQueryString(this.EndpointUrl, this.QueryParameters); ;
            }
            else
            {
                return this.EndpointUrl;
            }
        }
    }
}
// public abstract class BaseRequest
// {
//     private static List<HttpMethod> ReqBodyMethods => new List<HttpMethod> { HttpMethod.Post, HttpMethod.Patch, HttpMethod.Put };
//     public abstract string EndpointUrl { get; }
//     public abstract HttpMethod Method { get; }
//     public Dictionary<string, string?> QueryParameters { get; set; } = new Dictionary<string, string?>();
//     public HttpRequestMessage GetHttpRequest(string? Token)
//     {
//         Uri endpointUri = new Uri(this.FullEndpoint);
//         var request = new HttpRequestMessage();
//         request.RequestUri = endpointUri;
//         request.Method = this.Method;
//         var searchAsJson = JsonSerializer.Serialize(this.SearchObject);
//         // var test = new JsonContent();
//         if (this.SearchObject is not null && ReqBodyMethods.Contains(this.Method))
//             request.Content = new StringContent(searchAsJson.ToString(), Encoding.UTF8, "application/json");
//         return request;
//     }
//     public string FullEndpoint
//     {
//         get
//         {
//             if (this.QueryParameters is not null)
//             {

//                 return QueryHelpers.AddQueryString(this.EndpointUrl, this.QueryParameters); ;
//             }
//             else
//             {
//                 return this.EndpointUrl;
//             }
//         }
//     }
// }