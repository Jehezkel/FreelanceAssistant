using System.Text.Json.Serialization;

namespace WebApi.ApiClient.RequestParams;

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
    public UseInUrlEncodedBody(string ParamName) : base(ParamName)
    {
    }
    public UseInUrlEncodedBody() : base()
    {
    }
}
[AttributeUsage(AttributeTargets.Property)]
public class UseInUrl : Attribute
{

}

public class FormatValue : Attribute
{
    public FormatValue(string formatPattern, string separator)
    {
        FormatPattern = formatPattern;
        Separator = separator;
    }
    public FormatValue(string formatPattern)
    {
        FormatPattern = formatPattern;
    }
    public string FormatPattern { get; set; } = null!;
    public string? Separator { get; set; }
}

