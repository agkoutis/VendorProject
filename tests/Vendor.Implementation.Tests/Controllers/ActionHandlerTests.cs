using Microsoft.AspNetCore.Mvc;
using Vendor.Controllers;
using Vendor.Interfaces.Types;

namespace Vendor.Implementation.Tests.Controllers
{
    public class ActionHandlerTests
    {
        private readonly ActionHandler _actionHandler = new();

        [Fact]
        public async Task ExecuteAppResponseAction_WhenActionSucceeds_ReturnsOkWithResponse()
        {
            AppResponse<string> expected = AppResponse<string>.Success("hello");

            IActionResult result = await _actionHandler.ExecuteAppResponseAction(
                () => Task.FromResult(expected));

            OkObjectResult ok = Assert.IsType<OkObjectResult>(result);
            AppResponse<string> body = Assert.IsType<AppResponse<string>>(ok.Value);
            Assert.False(body.HasError);
            Assert.Equal("hello", body.Data);
        }

        [Fact]
        public async Task ExecuteAppResponseAction_WhenActionThrows_ReturnsOkWithFailResponse()
        {
            const string exceptionMessage = "something went wrong";

            IActionResult result = await _actionHandler.ExecuteAppResponseAction<string>(
                () => throw new Exception(exceptionMessage));

            OkObjectResult ok = Assert.IsType<OkObjectResult>(result);
            AppResponse<string> body = Assert.IsType<AppResponse<string>>(ok.Value);
            Assert.True(body.HasError);
            Assert.Equal(exceptionMessage, body.Error);
        }

        [Fact]
        public async Task ExecuteAppResponseAction_WhenActionThrows_NeverReturnsNonOkStatusCode()
        {
            IActionResult result = await _actionHandler.ExecuteAppResponseAction<bool>(
                () => throw new InvalidOperationException("boom"));

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ExecuteAppResponseAction_WhenActionReturnsFailResponse_ReturnsOkWithThatResponse()
        {
            AppResponse<bool> failed = AppResponse<bool>.Fail("validation failed");

            IActionResult result = await _actionHandler.ExecuteAppResponseAction(
                () => Task.FromResult(failed));

            OkObjectResult ok = Assert.IsType<OkObjectResult>(result);
            AppResponse<bool> body = Assert.IsType<AppResponse<bool>>(ok.Value);
            Assert.True(body.HasError);
            Assert.Equal("validation failed", body.Error);
        }
    }
}
