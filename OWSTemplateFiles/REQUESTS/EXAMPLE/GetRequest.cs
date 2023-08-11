using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OWSShared.Interfaces;
using Serilog;
using $safeprojectname$.DTOs;

namespace $safeprojectname$.Requests.Example
{
    public class GetRequest : IRequestHandler<GetRequest, IActionResult>, IRequest
    {
        private ILogger _logger;
        private Guid _customerGUID;

        public GetRequest(ILogger logger, IHeaderCustomerGUID customerGuid)
        {
            _logger = logger;
            _customerGUID = customerGuid.CustomerGUID;
        }

        public async Task<IActionResult> Handle()
        {
            _logger.Information("Example Get Request");
            return new OkObjectResult("Example Get Request");
        }
    }
}
