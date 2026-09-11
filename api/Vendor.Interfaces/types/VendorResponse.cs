using Vendor.Interfaces.types;

namespace Vendor.Interfaces.Types
{
    public class VendorResponse : VendorRequest
    {
        public string Id { get; set; } = string.Empty;
    }
}