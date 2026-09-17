using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vendor.Interfaces.Types;

namespace Vendor.Controllers
{
    public class ActionHandler : IActionHandler
    {
        private readonly ILogger<ActionHandler> _logger;

        public ActionHandler(ILogger<ActionHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> ExecuteAppResponseAction<T>(Func<Task<AppResponse<T>>> action)
        {
            try
            {
                AppResponse<T> response = await action();
                return new OkObjectResult(response);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Action Handler failed: {Error}", exception.Message);
                AppResponse<T> errorResponse = AppResponse<T>.Fail(exception.Message);
                return new OkObjectResult(errorResponse);
            }
        }
    }
}