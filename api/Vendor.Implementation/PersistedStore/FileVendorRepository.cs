using FileLoader;
using Vendor.Implementation.Extensions;
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
            List<VendorResponse> vendors = suppliers.Select(x => x.ToResponse()).ToList();
            return Task.FromResult<IEnumerable<VendorResponse>>(vendors);
        }

        public Task<VendorResponse> GetByIdAsync(string id)
        {
            Supplier supplier = _loader.LoadSupplier(id);
            return Task.FromResult(supplier.ToResponse());
        }

        public Task InsertAsync(VendorRequest vendor)
        {
            Supplier supplier = vendor.ToSupplier(Guid.NewGuid().ToString());
            _loader.InsertSupplier(supplier);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(VendorResponse vendor)
        {
            Supplier supplier = vendor.ToSupplier(vendor.Id);
            _loader.UpdateSupplier(supplier);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string id)
        {
            _loader.DeleteSupplier(id);
            return Task.CompletedTask;
        }
    }
}
