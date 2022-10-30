using System.Text.Json.Serialization;

namespace WebApi.ApiClient.RequestParams
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UseInRequestParameters : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class UseInRequestBody : Attribute
    {
    }
}
