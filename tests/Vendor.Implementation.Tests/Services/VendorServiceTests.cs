using Moq;
using Vendor.Implementation;
using Vendor.Interfaces;
using Vendor.Interfaces.types;
using Vendor.Interfaces.Types;

namespace Vendor.Implementation.Tests.Services
{
    public class VendorServiceTests
    {
        private readonly Mock<IVendorRepository> _vendorRepository;
        private readonly Mock<IVendorRequestValidator> _vendorRequestValidator;
        private readonly VendorService _vendorService;

        public VendorServiceTests()
        {
            _vendorRepository = new Mock<IVendorRepository>(MockBehavior.Strict);
            _vendorRequestValidator = new Mock<IVendorRequestValidator>(MockBehavior.Strict);
            _vendorService = new VendorService(_vendorRepository.Object, _vendorRequestValidator.Object);
        }

        [Fact]
        public async Task GetAllAsync_WhenRepositoryReturnsVendors_ReturnsSuccessWithVendors()
        {
            List<VendorResponse> vendors =
            [
                CreateVendor("1", "Acme", "Athens"),
                CreateVendor("2", "Globex", "Patras")
            ];
            _vendorRepository
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(vendors);

            AppResponse<IEnumerable<VendorResponse>> result = await _vendorService.GetAllAsync();

            AssertSuccess(result, vendors);
            _vendorRepository.Verify(repository => repository.GetAllAsync(), Times.Once);
            VerifyNoUnexpectedCalls();
        }

        [Fact]
        public async Task GetAllAsync_WhenRepositoryReturnsEmpty_ReturnsSuccessWithEmptyCollection()
        {
            IEnumerable<VendorResponse> vendors = Enumerable.Empty<VendorResponse>();
            _vendorRepository
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(vendors);

            AppResponse<IEnumerable<VendorResponse>> result = await _vendorService.GetAllAsync();

            AssertSuccess(result, vendors);
            Assert.Empty(result.Data);
            _vendorRepository.Verify(repository => repository.GetAllAsync(), Times.Once);
            VerifyNoUnexpectedCalls();
        }

        [Fact]
        public async Task GetByIdAsync_WhenValidationFails_ReturnsErrorDescriptionAndDoesNotCallRepository()
        {
            const string id = " ";
            _vendorRequestValidator
                .Setup(validator => validator.ValidateGet(id))
                .Returns(Failed("Request Validation Error", "Vendor id is required."));

            AppResponse<VendorResponse> result = await _vendorService.GetByIdAsync(id);

            AssertFailure(result, "Vendor id is required.");
            Assert.Null(result.Data);
            _vendorRequestValidator.Verify(validator => validator.ValidateGet(id), Times.Once);
            VerifyNoUnexpectedCalls();
        }

        [Fact]
        public async Task GetByIdAsync_WhenValidationFailsWithoutDescription_ReturnsErrorField()
        {
            const string id = "missing";
            _vendorRequestValidator
                .Setup(validator => validator.ValidateGet(id))
                .Returns(Failed("Vendor id is required.", string.Empty));

            AppResponse<VendorResponse> result = await _vendorService.GetByIdAsync(id);

            AssertFailure(result, "Vendor id is required.");
            _vendorRequestValidator.Verify(validator => validator.ValidateGet(id), Times.Once);
            VerifyNoUnexpectedCalls();
        }

        [Fact]
        public async Task GetByIdAsync_WhenValid_ReturnsVendorFromRepository()
        {
            const string id = "vendor-1";
            VendorResponse vendor = CreateVendor(id);
            _vendorRequestValidator
                .Setup(validator => validator.ValidateGet(id))
                .Returns(Succeeded());
            _vendorRepository
                .Setup(repository => repository.GetByIdAsync(id))
                .ReturnsAsync(vendor);

            AppResponse<VendorResponse> result = await _vendorService.GetByIdAsync(id);

            Assert.False(result.HasError);
            Assert.Null(result.Error);
            Assert.Same(vendor, result.Data);
            _vendorRequestValidator.Verify(validator => validator.ValidateGet(id), Times.Once);
            _vendorRepository.Verify(repository => repository.GetByIdAsync(id), Times.Once);
            VerifyNoUnexpectedCalls();
        }

        [Fact]
        public async Task InsertAsync_WhenValidationFails_ReturnsErrorAndDoesNotInsert()
        {
            VendorRequest vendor = new() { Name = string.Empty, Address = "Athens" };
            _vendorRequestValidator
                .Setup(validator => validator.ValidateCreateRequest(vendor))
                .Returns(Failed("Request Validation Error", "Vendor name is required."));

            AppResponse<bool> result = await _vendorService.InsertAsync(vendor);

            AssertFailure(result, "Vendor name is required.");
            Assert.False(result.Data);
            _vendorRequestValidator.Verify(validator => validator.ValidateCreateRequest(vendor), Times.Once);
            VerifyNoUnexpectedCalls();
        }

        [Fact]
        public async Task InsertAsync_WhenValid_CallsRepositoryAndReturnsTrue()
        {
            VendorRequest vendor = CreateRequest();
            _vendorRequestValidator
                .Setup(validator => validator.ValidateCreateRequest(vendor))
                .Returns(Succeeded());
            _vendorRepository
                .Setup(repository => repository.InsertAsync(vendor))
                .Returns(Task.CompletedTask);

            AppResponse<bool> result = await _vendorService.InsertAsync(vendor);

            AssertSuccess(result, true);
            _vendorRequestValidator.Verify(validator => validator.ValidateCreateRequest(vendor), Times.Once);
            _vendorRepository.Verify(repository => repository.InsertAsync(vendor), Times.Once);
            VerifyNoUnexpectedCalls();
        }

        [Fact]
        public async Task UpdateAsync_WhenValidationFails_ReturnsErrorAndDoesNotUpdate()
        {
            VendorResponse vendor = CreateVendor(id: string.Empty);
            _vendorRequestValidator
                .Setup(validator => validator.ValidateUpdate(vendor))
                .Returns(Failed("Request Validation Error", "Vendor id is required."));

            AppResponse<bool> result = await _vendorService.UpdateAsync(vendor);

            AssertFailure(result, "Vendor id is required.");
            Assert.False(result.Data);
            _vendorRequestValidator.Verify(validator => validator.ValidateUpdate(vendor), Times.Once);
            VerifyNoUnexpectedCalls();
        }

        [Fact]
        public async Task UpdateAsync_WhenValid_CallsRepositoryAndReturnsTrue()
        {
            VendorResponse vendor = CreateVendor();
            _vendorRequestValidator
                .Setup(validator => validator.ValidateUpdate(vendor))
                .Returns(Succeeded());
            _vendorRepository
                .Setup(repository => repository.UpdateAsync(vendor))
                .Returns(Task.CompletedTask);

            AppResponse<bool> result = await _vendorService.UpdateAsync(vendor);

            AssertSuccess(result, true);
            _vendorRequestValidator.Verify(validator => validator.ValidateUpdate(vendor), Times.Once);
            _vendorRepository.Verify(repository => repository.UpdateAsync(vendor), Times.Once);
            VerifyNoUnexpectedCalls();
        }

        [Fact]
        public async Task DeleteAsync_WhenValidationFails_ReturnsErrorAndDoesNotDelete()
        {
            const string id = "";
            _vendorRequestValidator
                .Setup(validator => validator.ValidateDelete(id))
                .Returns(Failed("Request Validation Error", "Vendor id is required."));

            AppResponse<bool> result = await _vendorService.DeleteAsync(id);

            AssertFailure(result, "Vendor id is required.");
            Assert.False(result.Data);
            _vendorRequestValidator.Verify(validator => validator.ValidateDelete(id), Times.Once);
            VerifyNoUnexpectedCalls();
        }

        [Fact]
        public async Task DeleteAsync_WhenValid_CallsRepositoryAndReturnsTrue()
        {
            const string id = "vendor-1";
            _vendorRequestValidator
                .Setup(validator => validator.ValidateDelete(id))
                .Returns(Succeeded());
            _vendorRepository
                .Setup(repository => repository.DeleteAsync(id))
                .Returns(Task.CompletedTask);

            AppResponse<bool> result = await _vendorService.DeleteAsync(id);

            AssertSuccess(result, true);
            _vendorRequestValidator.Verify(validator => validator.ValidateDelete(id), Times.Once);
            _vendorRepository.Verify(repository => repository.DeleteAsync(id), Times.Once);
            VerifyNoUnexpectedCalls();
        }

        private void VerifyNoUnexpectedCalls()
        {
            _vendorRepository.VerifyNoOtherCalls();
            _vendorRequestValidator.VerifyNoOtherCalls();
        }

        private static void AssertSuccess<T>(AppResponse<T> result, T expectedData)
        {
            Assert.False(result.HasError);
            Assert.Null(result.Error);
            Assert.Equal(expectedData, result.Data);
        }

        private static void AssertFailure<T>(AppResponse<T> result, string expectedError)
        {
            Assert.True(result.HasError);
            Assert.Equal(expectedError, result.Error);
        }

        private static VendorRequest CreateRequest()
        {
            return new VendorRequest
            {
                Name = "Acme",
                Address = "Athens"
            };
        }

        private static VendorResponse CreateVendor(string id = "vendor-1", string name = "Acme", string address = "Athens")
        {
            return new VendorResponse
            {
                Id = id,
                Name = name,
                Address = address
            };
        }

        private static ValidationResult Succeeded()
        {
            return new ValidationResult();
        }

        private static ValidationResult Failed(string error, string errorDescription)
        {
            return new ValidationResult(error, errorDescription);
        }
    }
}
