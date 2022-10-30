using System.Text.Json.Serialization;

namespace WebApi.ApiClient.RequestParams
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UseInRequestParameters : Attribute
    {
        public UseInRequestParameters(string ParamName)
        {
            Name = ParamName;
        }
        public UseInRequestParameters()
        {

        }

        public string? Name { get; }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class UseInRequestBody : Attribute
    {
    }
}
