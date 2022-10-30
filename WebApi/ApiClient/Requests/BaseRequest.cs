using System.Collections;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using WebApi.ApiClient.RequestParams;

namespace WebApi.ApiClient.Requests;


public  abstract class BaseRequest<T> where T : new() {

    public T RequestInputObject { get; init; } = new T();
    private static List<HttpMethod> ReqBodyMethods => new List<HttpMethod> { HttpMethod.Post, HttpMethod.Patch, HttpMethod.Put };

    public HttpRequestMessage GetHttpRequest()
    {
        Uri endpointUri = GetEndpointAndQueryParams();
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

    public Uri GetEndpointAndQueryParams() {
        //var baseAsUri = new Uri(baseUrl);
        //var resultAsString = new Uri(baseAsUri, this.EndpointUrl).ToString();
        var resultAsString = new Uri(this.EndpointUrl, UriKind.Relative).ToString();
        var queryParametersDictionary = getQueryParamsDictionary();
        if (queryParametersDictionary is not null)
            resultAsString =  QueryHelpers.AddQueryString(resultAsString, queryParametersDictionary);
        return new Uri(resultAsString, UriKind.Relative);
    }
    private Dictionary<string,string?> getQueryParamsDictionary()
    {
        var test = RequestInputObject!.GetType().GetProperties()
            .Where(p => Attribute.GetCustomAttribute(p, typeof(UseInRequestParameters)) is not null).FirstOrDefault();
        return RequestInputObject!.GetType().GetProperties()
            .Where(p => Attribute.GetCustomAttribute(p, typeof(UseInRequestParameters)) is not null)
            .Select(p => new { Name= GetNameValueFromAttribute(p)?? p.Name, Value=p.GetValue(RequestInputObject)})
            .Where(p=>p.Value is not null)
            //.ToDictionary(p => p.Name, p => (string?)Convert.ToString(p.Value));
            .ToDictionary(p => p.Name, p => GetParamAsString(p.Value!));
    }
    private string? GetParamAsString(object paramValue)
    {
        if(paramValue is IEnumerable)
        {
            var paramValAsList = paramValue as IEnumerable;
            var midList = new List<string>();
            foreach(var val in paramValAsList)
            {
                var currVal = Convert.ToString(val);
                midList.Add(currVal);
            }
            return (string?)String.Join(", ", midList); ;
        }
        return (string?)Convert.ToString(paramValue);
    }
    private string? GetNameValueFromAttribute(PropertyInfo prop)
    {
        return prop.GetCustomAttribute<UseInRequestParameters>()?.Name;
    }
}