using Vendor.Interfaces.types;
using Vendor.Interfaces.Types;

namespace Vendor.Interfaces
{
    public interface IVendorRequestValidator
    {
        ValidationResult ValidateCreateRequest(VendorRequest vendor);

        ValidationResult ValidateUpdate(VendorResponse vendor);

        ValidationResult ValidateDelete(string id);

        ValidationResult ValidateGet(string id);
    }
}