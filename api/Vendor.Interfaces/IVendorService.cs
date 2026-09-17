using Vendor.Interfaces.types;
using Vendor.Interfaces.Types;

namespace Vendor.Interfaces
{
    public interface IVendorService
    {
        Task<AppResponse<IEnumerable<VendorResponse>>> GetAllAsync();

        Task<AppResponse<VendorResponse>> GetByIdAsync(string id);

        Task<AppResponse<VendorResponse>> InsertAsync(VendorRequest vendor);

        Task<AppResponse<VendorResponse>> UpdateAsync(VendorResponse vendor);

        Task<AppResponse<bool>> DeleteAsync(string id);
    }
}
