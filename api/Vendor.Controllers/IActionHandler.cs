using Microsoft.AspNetCore.Mvc;
using Vendor.Interfaces.Types;

namespace Vendor.Controllers
{
    public interface IActionHandler
    {
        Task<IActionResult> ExecuteAppResponseAction<T>(Func<Task<AppResponse<T>>> action);
    }
}
