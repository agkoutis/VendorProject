using Vendor.Interfaces;
using Vendor.Interfaces.types;
using Vendor.Interfaces.Types;

namespace Vendor.Implementation
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IVendorRequestValidator _vendorRequestValidator;

        public VendorService(IVendorRepository vendorRepository, IVendorRequestValidator vendorRequestValidator)
        {
            this._vendorRepository = vendorRepository;
            this._vendorRequestValidator = vendorRequestValidator;
        }

        public async Task<AppResponse<IEnumerable<VendorResponse>>> GetAllAsync()
        {
            IEnumerable<VendorResponse> vendors = await _vendorRepository.GetAllAsync();
            return AppResponse<IEnumerable<VendorResponse>>.Success(vendors);
        }

        public async Task<AppResponse<VendorResponse>> GetByIdAsync(string id)
        {
            ValidationResult validation = _vendorRequestValidator.ValidateGet(id);
            if (validation.HasError)
            {
                return ToFailedResponse<VendorResponse>(validation);
            }

            VendorResponse vendor = await _vendorRepository.GetByIdAsync(id);
            return AppResponse<VendorResponse>.Success(vendor);
        }

        public async Task<AppResponse<VendorResponse>> InsertAsync(VendorRequest vendor)
        {
            ValidationResult validation = _vendorRequestValidator.ValidateCreateRequest(vendor);
            if (validation.HasError)
            {
                return ToFailedResponse<VendorResponse>(validation);
            }

            string id = await _vendorRepository.InsertAsync(vendor);
            return await GetByIdAsync(id);
        }

        public async Task<AppResponse<VendorResponse>> UpdateAsync(VendorResponse vendor)
        {
            ValidationResult validation = _vendorRequestValidator.ValidateUpdate(vendor);
            if (validation.HasError)
            {
                return ToFailedResponse<VendorResponse>(validation);
            }

            await _vendorRepository.UpdateAsync(vendor);
            return await GetByIdAsync(vendor.Id);
        }

        public async Task<AppResponse<bool>> DeleteAsync(string id)
        {
            ValidationResult validation = _vendorRequestValidator.ValidateDelete(id);
            if (validation.HasError)
            {
                return ToFailedResponse<bool>(validation);
            }

            await _vendorRepository.DeleteAsync(id);
            return AppResponse<bool>.Success(true);
        }

        private static AppResponse<T> ToFailedResponse<T>(ValidationResult validation)
        {
            string error = string.IsNullOrEmpty(validation.ErrorDescription)
                ? validation.Error
                : validation.ErrorDescription;

            return AppResponse<T>.Fail(error);
        }
    }
}