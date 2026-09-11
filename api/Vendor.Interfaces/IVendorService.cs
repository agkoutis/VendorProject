using Vendor.Interfaces.types;
using Vendor.Interfaces.Types;

namespace Vendor.Interfaces
{
    public interface IVendorService
    {
        Task<IEnumerable<VendorResponse>> GetAllAsync();

        Task<VendorResponse> GetByIdAsync(string id);

        Task InsertAsync(VendorRequest vendor);

        Task UpdateAsync(VendorResponse vendor);

        Task DeleteAsync(string id);
    }
}
