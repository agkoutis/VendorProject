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

        public VendorController(IVendorService vendorService)
        {
            this._vendorService = vendorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VendorResponse>>> GetAll()
        {
            IEnumerable<VendorResponse> vendors = await this._vendorService.GetAllAsync();
            return Ok(vendors);
        }

        [HttpGet("{id}")]
        public ActionResult<VendorResponse> GetById(string id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Insert(VendorRequest vendor)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public IActionResult Update(VendorResponse vendor)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}