using WebApi.ApiClient.RequestInputs;

namespace WebApi.ApiClient.Requests
{
    public class CategoriesRequest : BaseRequest<CategoriesInput>
    {
        public override string EndpointUrl => "projects/0.1/categories";

        public override HttpMethod Method => HttpMethod.Get;
    }
}
