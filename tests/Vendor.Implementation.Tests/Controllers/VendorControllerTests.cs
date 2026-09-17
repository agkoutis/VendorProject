using Microsoft.AspNetCore.Mvc;
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
        private readonly Mock<IActionHandler> _actionHandler;
        private readonly VendorController _controller;

        public VendorControllerTests()
        {
            _vendorService = new Mock<IVendorService>(MockBehavior.Strict);
            _actionHandler = new Mock<IActionHandler>(MockBehavior.Strict);
            _controller = new VendorController(_vendorService.Object, _actionHandler.Object);
        }

        [Fact]
        public async Task GetAll_DelegatesToActionHandlerWithGetAllAsync()
        {
            IActionResult expected = new OkObjectResult(AppResponse<IEnumerable<VendorResponse>>.Success([]));
            _actionHandler
                .Setup(handler => handler.ExecuteAppResponseAction(It.IsAny<Func<Task<AppResponse<IEnumerable<VendorResponse>>>>>()))
                .ReturnsAsync(expected);

            IActionResult result = await _controller.GetAll();

            Assert.Same(expected, result);
            _actionHandler.Verify(
                handler => handler.ExecuteAppResponseAction(It.IsAny<Func<Task<AppResponse<IEnumerable<VendorResponse>>>>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetById_DelegatesToActionHandlerWithGetByIdAsync()
        {
            const string id = "vendor-1";
            IActionResult expected = new OkObjectResult(AppResponse<VendorResponse>.Success(new VendorResponse { Id = id }));
            _actionHandler
                .Setup(handler => handler.ExecuteAppResponseAction(It.IsAny<Func<Task<AppResponse<VendorResponse>>>>()))
                .ReturnsAsync(expected);

            IActionResult result = await _controller.GetById(id);

            Assert.Same(expected, result);
            _actionHandler.Verify(
                handler => handler.ExecuteAppResponseAction(It.IsAny<Func<Task<AppResponse<VendorResponse>>>>()),
                Times.Once);
        }

        [Fact]
        public async Task Insert_DelegatesToActionHandlerWithInsertAsync()
        {
            VendorRequest vendor = new() { Name = "Acme", Address = "Athens" };
            VendorResponse created = new() { Id = "vendor-1", Name = "Acme", Address = "Athens" };
            IActionResult expected = new OkObjectResult(AppResponse<VendorResponse>.Success(created));
            _actionHandler
                .Setup(handler => handler.ExecuteAppResponseAction(It.IsAny<Func<Task<AppResponse<VendorResponse>>>>()))
                .ReturnsAsync(expected);

            IActionResult result = await _controller.Insert(vendor);

            Assert.Same(expected, result);
            _actionHandler.Verify(
                handler => handler.ExecuteAppResponseAction(It.IsAny<Func<Task<AppResponse<VendorResponse>>>>()),
                Times.Once);
        }

        [Fact]
        public async Task Update_DelegatesToActionHandlerWithUpdateAsync()
        {
            VendorResponse vendor = new() { Id = "vendor-1", Name = "Acme", Address = "Athens" };
            IActionResult expected = new OkObjectResult(AppResponse<VendorResponse>.Success(vendor));
            _actionHandler
                .Setup(handler => handler.ExecuteAppResponseAction(It.IsAny<Func<Task<AppResponse<VendorResponse>>>>()))
                .ReturnsAsync(expected);

            IActionResult result = await _controller.Update(vendor);

            Assert.Same(expected, result);
            _actionHandler.Verify(
                handler => handler.ExecuteAppResponseAction(It.IsAny<Func<Task<AppResponse<VendorResponse>>>>()),
                Times.Once);
        }

        [Fact]
        public async Task Delete_DelegatesToActionHandlerWithDeleteAsync()
        {
            const string id = "vendor-1";
            IActionResult expected = new OkObjectResult(AppResponse<bool>.Success(true));
            _actionHandler
                .Setup(handler => handler.ExecuteAppResponseAction(It.IsAny<Func<Task<AppResponse<bool>>>>()))
                .ReturnsAsync(expected);

            IActionResult result = await _controller.Delete(id);

            Assert.Same(expected, result);
            _actionHandler.Verify(
                handler => handler.ExecuteAppResponseAction(It.IsAny<Func<Task<AppResponse<bool>>>>()),
                Times.Once);
        }
    }
}
