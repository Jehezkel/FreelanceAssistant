using Microsoft.AspNetCore.Mvc;
using WebApi.DAL;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DescriptionTemplateController : ControllerBase
    {
        private readonly IFLDbContext _fLDbContext;

        public DescriptionTemplateController(IFLDbContext fLDbContext)
        {
            _fLDbContext = fLDbContext;
        }
        //[HttpGet]
    }
}
