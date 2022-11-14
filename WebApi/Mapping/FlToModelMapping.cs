using WebApi.ApiClient.Responses;
using WebApi.Models;
namespace WebApi.Mapping;

public static class FLToModelMapping
{
    public static FLApiToken ToFLApiToken(this VerifyCodeResponse response, string UserId)
    {
        return new FLApiToken
        {
            AccessToken = response.AccessToken,
            RefreshToken = response.RefreshToken,
            UserId = UserId
        };
    }
}