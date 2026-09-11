using Microsoft.AspNetCore.Mvc;
using Vendor.Interfaces;
using Vendor.Interfaces.types;
using Vendor.Interfaces.Types;

namespace Vendor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;
        private readonly IActionHandler _actionHandler;

        public VendorController(IVendorService vendorService, IActionHandler actionHandler)
        {
            this._vendorService = vendorService;
            this._actionHandler = actionHandler;
        }

        [HttpGet]
        public Task<IActionResult> GetAll()
        {
            return _actionHandler.ExecuteAppResponseAction(() => this._vendorService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetById(string id)
        {
            return _actionHandler.ExecuteAppResponseAction(() => this._vendorService.GetByIdAsync(id));
        }

        [HttpPost]
        public Task<IActionResult> Insert(VendorRequest vendor)
        {
            return _actionHandler.ExecuteAppResponseAction(() => this._vendorService.InsertAsync(vendor));
        }

        [HttpPut]
        public Task<IActionResult> Update(VendorResponse vendor)
        {
            return _actionHandler.ExecuteAppResponseAction(() => this._vendorService.UpdateAsync(vendor));
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> Delete(string id)
        {
            return _actionHandler.ExecuteAppResponseAction(() => this._vendorService.DeleteAsync(id));
        }
    }
}
