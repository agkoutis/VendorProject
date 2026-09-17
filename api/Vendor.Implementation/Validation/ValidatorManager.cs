using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Vendor.Interfaces.Types;

namespace Vendor.Implementation.Validation
{
    public abstract class ValidatorManager
    {
        private readonly string defaultError;
        private readonly string validatorName;
        private readonly ILogger _logger;

        protected ValidatorManager(ILogger logger, string validatorName, string defaultError = "Request Validation Error")
        {
            this.defaultError = defaultError;
            this.validatorName = validatorName;
            _logger = logger;
        }

        protected ValidationResult CreateFailedValidationResult(string errorDescription)
        {
            return new ValidationResult(defaultError, errorDescription);
        }

        protected ValidationResult CreateSuccessValidationResult()
        {
            return new ValidationResult();
        }

        protected void LogError(string message, [CallerMemberName] string functionName = "")
        {
            _logger.LogError(validatorName + " - " + functionName + ": " + message);
        }

        protected void LogInfo(string message, [CallerMemberName] string functionName = "")
        {
            _logger.LogInformation(validatorName + " - " + functionName + ": " + message);
        }
    }
}