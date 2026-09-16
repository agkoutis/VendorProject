using SqlServerLoader;
using Vendor.Implementation.Extensions;
using Vendor.Interfaces;
using Vendor.Interfaces.types;
using Vendor.Interfaces.Types;

namespace Vendor.Implementation.PersistedStore
{
    public class SqlServerVendorRepository : IVendorRepository
    {
        private readonly DataLoader _dataLoader;

        public SqlServerVendorRepository(DataLoader dataLoader)
        {
            _dataLoader = dataLoader;
        }

        public async Task<IEnumerable<VendorResponse>> GetAllAsync()
        {
            List<Trader> traders = await _dataLoader.LoadTraders();
            return traders.Select(x => x.ToResponse()).ToList();
        }

        public async Task<VendorResponse> GetByIdAsync(string id)
        {
            Trader trader = await _dataLoader.LoadTrader(id);
            return trader.ToResponse();
        }

        public Task InsertAsync(VendorRequest vendor)
        {
            Trader trader = vendor.ToTrader(Guid.NewGuid().ToString());
            return _dataLoader.InsertTrader(trader);
        }

        public Task UpdateAsync(VendorResponse vendor)
        {
            Trader trader = vendor.ToTrader(vendor.Id);
            return _dataLoader.UpdateTrader(trader);
        }

        public Task DeleteAsync(string id)
        {
            return _dataLoader.DeleteTrader(id);
        }
    }
}