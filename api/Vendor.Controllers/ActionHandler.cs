using Microsoft.AspNetCore.Mvc;
using Vendor.Interfaces.Types;

namespace Vendor.Controllers
{
    public class ActionHandler : IActionHandler
    {
        public async Task<IActionResult> ExecuteAppResponseAction<T>(Func<Task<AppResponse<T>>> action)
        {
            try
            {
                AppResponse<T> response = await action();
                return new OkObjectResult(response);
            }
            catch (Exception exception)
            {
                AppResponse<T> errorResponse = AppResponse<T>.Fail(exception.Message);
                return new OkObjectResult(errorResponse);
            }
        }
    }
}
