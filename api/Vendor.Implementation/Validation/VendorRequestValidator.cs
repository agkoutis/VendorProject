using Microsoft.Extensions.Logging;
using Vendor.Interfaces;
using Vendor.Interfaces.types;
using Vendor.Interfaces.Types;

namespace Vendor.Implementation.Validation
{
    public class VendorRequestValidator : ValidatorManager, IVendorRequestValidator
    {
        public VendorRequestValidator(ILogger<VendorRequestValidator> logger)
            : base(logger, nameof(VendorRequestValidator))
        {
        }

        public ValidationResult ValidateCreateRequest(VendorRequest vendor)
        {
            if (vendor is null)
            {
                LogError("Vendor request is missing.");
                return CreateFailedValidationResult("Vendor request is required.");
            }

            return ValidateNameAndAddress(vendor.Name, vendor.Address);
        }

        public ValidationResult ValidateUpdate(VendorResponse vendor)
        {
            if (vendor is null)
            {
                LogError("Vendor is missing.");
                return CreateFailedValidationResult("Vendor is required.");
            }

            ValidationResult idResult = ValidateVendorId(vendor.Id);
            if (idResult.HasError)
            {
                return idResult;
            }

            return ValidateNameAndAddress(vendor.Name, vendor.Address);
        }

        public ValidationResult ValidateDelete(string id)
        {
            return ValidateVendorId(id);
        }

        public ValidationResult ValidateGet(string id)
        {
            return ValidateVendorId(id);
        }

        private ValidationResult ValidateVendorId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                LogError("Vendor id is missing.");
                return CreateFailedValidationResult("Vendor id is required.");
            }

            return CreateSuccessValidationResult();
        }

        private ValidationResult ValidateNameAndAddress(string name, string address)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                LogError("Vendor name is missing.");
                return CreateFailedValidationResult("Vendor name is required.");
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                LogError("Vendor address is missing.");
                return CreateFailedValidationResult("Vendor address is required.");
            }

            return CreateSuccessValidationResult();
        }
    }
}