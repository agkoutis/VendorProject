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

        public Task<IEnumerable<VendorResponse>> GetAllAsync()
        {
            try
            {
                return _vendorRepository.GetAllAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<VendorResponse> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(VendorRequest vendor)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(VendorResponse vendor)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}