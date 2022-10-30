using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using WebApi.ApiClient.RequestParams;

namespace WebApi.ApiClient.Requests;


public  abstract class BaseRequest<T> where T : new() {

    public T RequestInputObject { get; init; } = new T();
    private static List<HttpMethod> ReqBodyMethods => new List<HttpMethod> { HttpMethod.Post, HttpMethod.Patch, HttpMethod.Put };

    public HttpRequestMessage GetHttpRequest(string BaseUrl)
    {
        Uri endpointUri = GetFullRequestUri(BaseUrl);
        var request = new HttpRequestMessage();
        request.RequestUri = endpointUri;
        request.Method = this.Method;
        if (this.RequestInputObject is not null && ReqBodyMethods.Contains(this.Method))
        {
            var searchAsJson = JsonSerializer.Serialize(this.RequestInputObject);
            request.Content = new StringContent(searchAsJson.ToString(), Encoding.UTF8, "application/json");
        }
        return request;
    }


    public abstract string EndpointUrl { get; }
    public abstract HttpMethod Method { get; }
    public Dictionary<string, string?> QueryParameters { get; set; } = new Dictionary<string, string?>();

    public Uri GetFullRequestUri(string baseUrl) {
        var baseAsUri = new Uri(baseUrl);
        var resultAsString = new Uri(baseAsUri, this.EndpointUrl).ToString();
        var queryParametersDictionary = getQueryParamsDictionary();
        if (queryParametersDictionary is not null)
            resultAsString =  QueryHelpers.AddQueryString(resultAsString, this.QueryParameters.Where(d => d.Value is not null));
        return new Uri(resultAsString);
    }
    private Dictionary<string,string?> getQueryParamsDictionary()
    {
        var test = this.RequestInputObject!.GetType().GetProperties();
        var nextStep = test.Where(p => Attribute.GetCustomAttribute(p, typeof(UseInRequestParameters)) is not null);
        return this.QueryParameters = RequestInputObject!.GetType().GetProperties().Where(p => Attribute.GetCustomAttribute(p, typeof(UseInRequestParameters)) is not null)
            .ToDictionary(p => p.Name, p => (string?)Convert.ToString(p.GetValue(RequestInputObject)));
    }
}