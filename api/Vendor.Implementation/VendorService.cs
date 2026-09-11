using Vendor.Interfaces;
using Vendor.Interfaces.types;
using Vendor.Interfaces.Types;

namespace Vendor.Implementation
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorService(IVendorRepository vendorRepository)
        {
            this._vendorRepository = vendorRepository;
        }

        public async Task<AppResponse<IEnumerable<VendorResponse>>> GetAllAsync()
        {
            IEnumerable<VendorResponse> vendors = await _vendorRepository.GetAllAsync();
            return AppResponse<IEnumerable<VendorResponse>>.Success(vendors);
        }

        public async Task<AppResponse<VendorResponse>> GetByIdAsync(string id)
        {
            VendorResponse vendor = await _vendorRepository.GetByIdAsync(id);
            return AppResponse<VendorResponse>.Success(vendor);
        }

        public async Task<AppResponse<bool>> InsertAsync(VendorRequest vendor)
        {
            await _vendorRepository.InsertAsync(vendor);
            return AppResponse<bool>.Success(true);
        }

        public async Task<AppResponse<bool>> UpdateAsync(VendorResponse vendor)
        {
            await _vendorRepository.UpdateAsync(vendor);
            return AppResponse<bool>.Success(true);
        }

        public async Task<AppResponse<bool>> DeleteAsync(string id)
        {
            await _vendorRepository.DeleteAsync(id);
            return AppResponse<bool>.Success(true);
        }
    }
}