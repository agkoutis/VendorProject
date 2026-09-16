using FileLoader;
using Vendor.Interfaces.types;
using Vendor.Interfaces.Types;

namespace Vendor.Implementation.Extensions
{
    internal static class FileLoaderExtensions
    {
        internal static VendorResponse ToResponse(this Supplier supplier)
        {
            return new VendorResponse
            {
                Id = supplier.Id ?? string.Empty,
                Name = supplier.Name ?? string.Empty,
                Address = supplier.Address ?? string.Empty
            };
        }

        internal static Supplier ToSupplier(this VendorRequest request, string id)
        {
            return new Supplier
            {
                Id = id,
                Name = request.Name,
                Address = request.Address
            };
        }
    }
}
