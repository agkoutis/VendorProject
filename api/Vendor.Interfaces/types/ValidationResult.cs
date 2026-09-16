namespace Vendor.Interfaces.Types
{
    public class ValidationResult
    {
        public string Error { get; } = string.Empty;

        public string ErrorDescription { get; } = string.Empty;

        public bool HasError => !string.IsNullOrEmpty(Error);

        public ValidationResult()
        {
        }

        public ValidationResult(string error, string errorDescription)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }
    }
}
