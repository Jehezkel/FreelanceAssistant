using System.Text.Json.Serialization;

namespace WebApi.ApiClient.RequestParams
{
    public abstract class NamedAttribute : Attribute
    {
        public NamedAttribute(string ParamName)
        {
            Name = ParamName;
        }
        public NamedAttribute()
        {
        }

        public string? Name { get; }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class UseInRequestParameters : NamedAttribute
    {
        public UseInRequestParameters(string ParamName) : base(ParamName)
        {
        }
        public UseInRequestParameters() : base()
        {
        }

    }
    [AttributeUsage(AttributeTargets.Property)]
    public class UseInUrlEncodedBody : NamedAttribute
    {
        public UseInUrlEncodedBody(string ParamName) :base(ParamName)
        {
        }
        public UseInUrlEncodedBody() : base()
        {
        }
    }
}
