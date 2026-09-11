using Vendor.Interfaces.types;
using Vendor.Interfaces.Types;

namespace Vendor.Interfaces
{
    public interface IVendorService
    {
        Task<AppResponse<IEnumerable<VendorResponse>>> GetAllAsync();

        Task<AppResponse<VendorResponse>> GetByIdAsync(string id);

        Task<AppResponse<bool>> InsertAsync(VendorRequest vendor);

        Task<AppResponse<bool>> UpdateAsync(VendorResponse vendor);

        Task<AppResponse<bool>> DeleteAsync(string id);
    }
}
