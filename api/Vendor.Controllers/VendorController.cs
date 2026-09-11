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
        public ActionResult<IEnumerable<VendorResponse>> GetAll()
        {
            throw new NotImplementedException();
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