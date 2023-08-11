using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleInjector;
using OWSShared.Interfaces;
using Serilog;
using $safeprojectname$.Requests.Example;
using $safeprojectname$.DTOs;

namespace $safeprojectname$.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExampleController : Controller
    {
        private readonly ILogger _logger;
        private readonly Container _container;
        private readonly IHeaderCustomerGUID _customerGuid;

        public ExampleController(ILogger logger, Container container, IHeaderCustomerGUID customerGuid)
        {
            _logger = logger;
            _container = container;
            _customerGuid = customerGuid;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            _logger.Information("Get Example Controller Called");
            GetRequest request = new GetRequest(_logger, _customerGuid);
            return await request.Handle();
        }

        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] ExampleDTO exampleDTO)
        {
            _logger.Information("Post Example Controller Called");
            PostRequest request = new PostRequest(exampleDTO, _logger, _customerGuid);
            return await request.Handle();
        }
    }
}