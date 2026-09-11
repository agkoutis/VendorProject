namespace Vendor.Api.Options
{
    public class VendorLoaderSettings
    {
        public string SelectedLoaderType { get; set; } = string.Empty;

        public VendorLoaderOptions VendorLoaderOptions { get; set; } = new();
    }

    public class VendorLoaderOptions
    {
        public SqlLoaderOptions Sql { get; set; } = new();

        public FileLoaderOptions File { get; set; } = new();
    }

    public class SqlLoaderOptions
    {
        public string Server { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    public class FileLoaderOptions
    {
        public string FilePath { get; set; } = string.Empty;
    }
}
