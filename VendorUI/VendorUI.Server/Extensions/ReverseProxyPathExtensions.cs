namespace VendorUI.Server.Extensions
{
    public static class ReverseProxyPathExtensions
    {
        public static string EnsureLeadingSlash(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "/";
            }

            return value.StartsWith('/') ? value : "/" + value;
        }

        public static string RemoveProxyKeySource(this PathString path, string proxyKey)
        {
            var remaining = path.Value ?? string.Empty;

            if (!string.IsNullOrEmpty(proxyKey) && remaining.StartsWith(proxyKey, StringComparison.OrdinalIgnoreCase))
            {
                remaining = remaining[proxyKey.Length..];
            }

            return remaining.EnsureLeadingSlash();
        }
    }
}
