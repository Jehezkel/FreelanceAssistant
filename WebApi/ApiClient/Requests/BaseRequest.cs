using System.Collections;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using WebApi.ApiClient.RequestInputs;
using WebApi.ApiClient.RequestParams;

namespace WebApi.ApiClient.Requests;


public abstract class BaseRequest
{
    public abstract string EndpointUrl { get; }
    public abstract HttpMethod Method { get; }
    public virtual HttpRequestMessage GetHttpRequest()
    {
        var request = new HttpRequestMessage
        {
            Method = this.Method,
            RequestUri = new Uri(this.EndpointUrl, UriKind.Relative)
        };
        return request;
    }
}

public abstract class BaseRequest<T> : BaseRequest where T : new()
{
    public BaseRequest(T? requestInputObject)
    {
        RequestInputObject = requestInputObject;
    }
    public BaseRequest()
    {

    }
    public T? RequestInputObject { get; set; }
    private static List<HttpMethod> ReqBodyMethods => new List<HttpMethod> { HttpMethod.Post, HttpMethod.Patch, HttpMethod.Put };

    public override HttpRequestMessage GetHttpRequest()
    {
        var request = new HttpRequestMessage
        {
            RequestUri = GetEndpointAndQueryParams(),
            Method = this.Method
        };
        if (this.RequestInputObject is not null && ReqBodyMethods.Contains(this.Method))
        {
            var formUrlDictionary = getAttributesAsDictionary<UseInUrlEncodedBody>();
            if (formUrlDictionary.Count > 0)
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
    public Uri GetEndpointAndQueryParams()
    {
        var resultAsString = this.EndpointUrl;
        if(RequestInputObject is null)
        {
            return new Uri(resultAsString, UriKind.Relative);
        }
        if(RequestInputObject is IHasRouteEndpointAddition)
        {
            resultAsString += ((IHasRouteEndpointAddition)RequestInputObject).GetEndpointAddition();
        }
        var queryParametersDictionary = getAttributesAsDictionary<UseInRequestParameters>();
        if (queryParametersDictionary is not null && queryParametersDictionary.Count>0)
            resultAsString = QueryHelpers.AddQueryString(resultAsString, queryParametersDictionary);
        return new Uri(resultAsString, UriKind.Relative);
    }
    private Dictionary<string, string?> getAttributesAsDictionary<attribType>() where attribType : NamedAttribute
    {
        Dictionary<string, string?> result = new();
        var listOfPropsWithAttribType = GetAllPropertiesWithAttribute<attribType>();
        foreach (var prop in listOfPropsWithAttribType)
        {
            var name = prop.GetCustomAttribute<attribType>()!.Name ?? prop.Name;
            var value = GetParamAsString(prop);
            result[name] = value;
        }
        return result;
    }
    private string? GetParamAsString(PropertyInfo propertyInfo)
    {
        var customFormat = propertyInfo.GetCustomAttribute<FormatValue>();
        var stringFormat = customFormat?.FormatPattern ?? "{0}";
        var propertyValue = propertyInfo.GetValue(RequestInputObject);
        if (propertyValue is null)
        {
            return null;
        }
        if (propertyValue is IEnumerable && propertyValue is ICollection)
        {
            var separator = customFormat?.Separator ?? ", ";
            var paramValAsList = propertyValue as IEnumerable;
            var resultList = new List<string>();
            foreach (var val in paramValAsList!)
            {
                var formattedValue = String.Format(stringFormat, val);
                resultList.Add(formattedValue);
            }
            return (string?)String.Join(separator, resultList); ;
        }
        else
        {
            return (string?)String.Format(stringFormat, propertyValue);
        }
    }

    private List<PropertyInfo> GetAllPropertiesWithAttribute<AttribType>()
    {
        return RequestInputObject!.GetType().GetProperties()
            .Where(p => Attribute.GetCustomAttribute(p, typeof(AttribType)) is not null).ToList();
    }

    //private string? GetSeparator(PropertyInfo prop)
    //{
    //    return prop.GetCustomAttribute<FormatValue>()?.FormatPattern;
    //}
}
