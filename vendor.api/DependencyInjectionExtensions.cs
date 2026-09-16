using FileLoader;
using SqlServerLoader;
using Vendor.Api.Options;
using Vendor.Controllers;
using Vendor.Implementation;
using Vendor.Implementation.PersistedStore;
using Vendor.Implementation.Validation;
using Vendor.Interfaces;

namespace Vendor.Api
{
    public static class DependencyInjectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterConfigurationSettings(services, configuration);

            services.Configure<VendorLoaderSettings>(configuration);
            services.AddScoped<IActionHandler, ActionHandler>();
        }

        private static void RegisterConfigurationSettings(IServiceCollection services, IConfiguration configuration)
        {
            VendorLoaderSettings settings = configuration.Get<VendorLoaderSettings>()
                ?? throw new InvalidOperationException("Vendor loader settings are missing");

            if (string.IsNullOrWhiteSpace(settings.SelectedLoaderType))
            {
                throw new InvalidOperationException("SelectedLoaderType is missing");
            }

            services.AddSingleton<IVendorService, VendorService>();
            services.AddSingleton<IVendorRequestValidator, VendorRequestValidator>();

            if (settings.SelectedLoaderType.Equals("Sql", StringComparison.OrdinalIgnoreCase))
            {
                SqlLoaderOptions sql = settings.VendorLoaderOptions.Sql;
                services.AddSingleton(new DataLoader(sql.Server, sql.UserId, sql.Password));
                services.AddSingleton<IVendorRepository, SqlServerVendorRepository>();
            }
            else if (settings.SelectedLoaderType.Equals("File", StringComparison.OrdinalIgnoreCase))
            {
                FileLoaderOptions file = settings.VendorLoaderOptions.File;
                services.AddSingleton(new Loader(file.FilePath));
                services.AddSingleton<IVendorRepository, FileVendorRepository>();
            }
            else
            {
                throw new InvalidOperationException($"Unknown SelectedLoaderType: {settings.SelectedLoaderType}");
            }
        }
    }
}