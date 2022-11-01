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


public abstract class BaseRequest<T> where T : new()
{

    public T RequestInputObject { get; set; } = new T();
    private static List<HttpMethod> ReqBodyMethods => new List<HttpMethod> { HttpMethod.Post, HttpMethod.Patch, HttpMethod.Put };

    public HttpRequestMessage GetHttpRequest()
    {
        Uri endpointUri = GetEndpointAndQueryParams();
        var request = new HttpRequestMessage();
        request.RequestUri = endpointUri;
        request.Method = this.Method;
        if (this.RequestInputObject is not null && ReqBodyMethods.Contains(this.Method))
        {
            var formUrlDictionary = getAttributesAsDictionary<UseInUrlEncodedBody>();
            if (formUrlDictionary is not null)
            {
                request.Content = new FormUrlEncodedContent(formUrlDictionary);
            }
            else
            {
                var searchAsJson = JsonSerializer.Serialize(this.RequestInputObject);
                request.Content = new StringContent(searchAsJson.ToString(), Encoding.UTF8, "application/json");
            }
        }
        return request;
    }
    public abstract string EndpointUrl { get; }
    public abstract HttpMethod Method { get; }
    public Uri GetEndpointAndQueryParams()
    {
        var resultAsString = new Uri(this.EndpointUrl, UriKind.Relative).ToString();
        var queryParametersDictionary = getAttributesAsDictionary<UseInRequestParameters>();
        if (queryParametersDictionary is not null)
            resultAsString = QueryHelpers.AddQueryString(resultAsString, queryParametersDictionary);
        return new Uri(resultAsString, UriKind.Relative);
    }
    private Dictionary<string, string?> getAttributesAsDictionary<attribType>() where attribType : NamedAttribute
    {
        var test = RequestInputObject!.GetType().GetProperties()
            .Where(p => Attribute.GetCustomAttribute(p, typeof(attribType)) is not null).FirstOrDefault();
        return RequestInputObject!.GetType().GetProperties()
            .Where(p => Attribute.GetCustomAttribute(p, typeof(attribType)) is not null)
            .Select(p => new { Name = GetNameValueFromAttribute<attribType>(p) ?? p.Name, Value = p.GetValue(RequestInputObject) })
            .Where(p => p.Value is not null)
            .ToDictionary(p => p.Name, p => GetParamAsString(p.Value!));
    }
    private string? GetParamAsString(object paramValue)
    {
        if (paramValue is IEnumerable && paramValue is ICollection)
        {
            var paramValAsList = paramValue as IEnumerable;
            var midList = new List<string>();
            foreach (var val in paramValAsList)
            {
                var currVal = Convert.ToString(val);
                midList.Add(currVal);
            }
            return (string?)String.Join(", ", midList); ;
        }
        return (string?)Convert.ToString(paramValue);
    }
    private string? GetNameValueFromAttribute<attribType>(PropertyInfo prop) where attribType : NamedAttribute
    {
        return prop.GetCustomAttribute<attribType>()?.Name;
    }
}