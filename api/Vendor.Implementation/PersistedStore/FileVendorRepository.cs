using FileLoader;
using Vendor.Interfaces;
using Vendor.Interfaces.types;
using Vendor.Interfaces.Types;

namespace Vendor.Implementation.PersistedStore
{
    public class FileVendorRepository : IVendorRepository
    {
        private readonly Loader _loader;

        public FileVendorRepository(Loader loader)
        {
            _loader = loader;
        }

        public Task<IEnumerable<VendorResponse>> GetAllAsync()
        {
            IEnumerable<Supplier> suppliers = _loader.LoadSuppliers();
            List<VendorResponse> vendors = suppliers.Select(ToResponse).ToList();
            return Task.FromResult<IEnumerable<VendorResponse>>(vendors);
        }

        public Task<VendorResponse> GetByIdAsync(string id)
        {
            Supplier supplier = _loader.LoadSupplier(id);
            return Task.FromResult(ToResponse(supplier));
        }

        public Task InsertAsync(VendorRequest vendor)
        {
            Supplier supplier = ToSupplier(vendor, Guid.NewGuid().ToString());
            _loader.InsertSupplier(supplier);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(VendorResponse vendor)
        {
            Supplier supplier = ToSupplier(vendor, vendor.Id);
            _loader.UpdateSupplier(supplier);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string id)
        {
            _loader.DeleteSupplier(id);
            return Task.CompletedTask;
        }

        private static VendorResponse ToResponse(Supplier supplier)
        {
            return new VendorResponse
            {
                Id = supplier.Id ?? string.Empty,
                Name = supplier.Name ?? string.Empty,
                Address = supplier.Address ?? string.Empty
            };
        }

        private static Supplier ToSupplier(VendorRequest request, string id)
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
