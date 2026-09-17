using SqlServerLoader;
using Vendor.Interfaces.types;
using Vendor.Interfaces.Types;

namespace Vendor.Implementation.Extensions
{
    internal static class SqlServerLoaderExtensions
    {
        internal static VendorResponse ToResponse(this Trader trader)
        {
            return new VendorResponse
            {
                Id = trader.Code ?? string.Empty,
                Name = trader.Description ?? string.Empty,
                Address = trader.Street ?? string.Empty
            };
        }

        internal static Trader ToTrader(this VendorRequest request, string id)
        {
            return new Trader
            {
                Code = id,
                Description = request.Name,
                Street = request.Address
            };
        }
    }
}
