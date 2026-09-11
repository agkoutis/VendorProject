using SqlServerLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.Interfaces.types;
using Vendor.Interfaces.Types;

namespace Vendor.Implementation.Extensions
{
    internal static class FileLoaderExtensions
    {
        internal static VendorResponse ToResponse(this Trader trader)
        {
            return new VendorResponse
            {
                Id = trader.Code,
                Address = trader.Street,
                Name = trader.Description
            };
        }

        internal static Trader ToModel(this VendorRequest request, string? id = null)
        {
            return new Trader
            {
                Code = id,
                Street = request.Address,
                Description = request.Name
            };
        }
    }
}