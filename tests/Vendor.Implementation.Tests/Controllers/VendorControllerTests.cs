using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Vendor.Controllers;
using Vendor.Interfaces;
using Vendor.Interfaces.types;
using Vendor.Interfaces.Types;

namespace Vendor.Implementation.Tests.Controllers
{
    public class VendorControllerTests
    {
        private readonly Mock<IVendorService> _vendorService;
        private readonly VendorController _controller;

        public VendorControllerTests()
        {
            _vendorService = new Mock<IVendorService>(MockBehavior.Strict);
            _controller = new VendorController(
                _vendorService.Object,
                new ActionHandler(NullLogger<ActionHandler>.Instance));
        }

        [Fact]
        public async Task GetAll_CallsGetAllAsync()
        {
            List<VendorResponse> vendors = [CreateVendor("1")];
            _vendorService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(AppResponse<IEnumerable<VendorResponse>>.Success(vendors));

            IActionResult result = await _controller.GetAll();

            AppResponse<IEnumerable<VendorResponse>> body = AssertOkBody<IEnumerable<VendorResponse>>(result);
            Assert.Equal(vendors, body.Data);
            _vendorService.Verify(service => service.GetAllAsync(), Times.Once);
            _vendorService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetById_CallsGetByIdAsyncWithTheSameId()
        {
            const string id = "vendor-1";
            VendorResponse vendor = CreateVendor(id);
            _vendorService
                .Setup(service => service.GetByIdAsync(id))
                .ReturnsAsync(AppResponse<VendorResponse>.Success(vendor));

            IActionResult result = await _controller.GetById(id);

            AppResponse<VendorResponse> body = AssertOkBody<VendorResponse>(result);
            Assert.Same(vendor, body.Data);
            _vendorService.Verify(service => service.GetByIdAsync(id), Times.Once);
            _vendorService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Insert_CallsInsertAsyncWithTheSameRequest()
        {
            VendorRequest vendor = new() { Name = "Acme", Address = "Athens" };
            VendorResponse created = CreateVendor("vendor-1");
            _vendorService
                .Setup(service => service.InsertAsync(vendor))
                .ReturnsAsync(AppResponse<VendorResponse>.Success(created));

            IActionResult result = await _controller.Insert(vendor);

            AppResponse<VendorResponse> body = AssertOkBody<VendorResponse>(result);
            Assert.Same(created, body.Data);
            _vendorService.Verify(service => service.InsertAsync(vendor), Times.Once);
            _vendorService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Update_CallsUpdateAsyncWithTheSameVendor()
        {
            VendorResponse vendor = CreateVendor();
            _vendorService
                .Setup(service => service.UpdateAsync(vendor))
                .ReturnsAsync(AppResponse<VendorResponse>.Success(vendor));

            IActionResult result = await _controller.Update(vendor);

            AppResponse<VendorResponse> body = AssertOkBody<VendorResponse>(result);
            Assert.Same(vendor, body.Data);
            _vendorService.Verify(service => service.UpdateAsync(vendor), Times.Once);
            _vendorService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Delete_CallsDeleteAsyncWithTheSameId()
        {
            const string id = "vendor-1";
            _vendorService
                .Setup(service => service.DeleteAsync(id))
                .ReturnsAsync(AppResponse<bool>.Success(true));

            IActionResult result = await _controller.Delete(id);

            AppResponse<bool> body = AssertOkBody<bool>(result);
            Assert.True(body.Data);
            _vendorService.Verify(service => service.DeleteAsync(id), Times.Once);
            _vendorService.VerifyNoOtherCalls();
        }

        private static AppResponse<T> AssertOkBody<T>(IActionResult result)
        {
            OkObjectResult ok = Assert.IsType<OkObjectResult>(result);
            return Assert.IsType<AppResponse<T>>(ok.Value);
        }

        private static VendorResponse CreateVendor(string id = "vendor-1")
        {
            return new VendorResponse
            {
                Id = id,
                Name = "Acme",
                Address = "Athens"
            };
        }
    }
}
