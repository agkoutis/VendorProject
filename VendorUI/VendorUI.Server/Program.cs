using VendorUI.Server.Middleware;

var builder = WebApplication.CreateBuilder(args);

var reverseProxyClient = builder.Services.AddHttpClient("reverse-proxy-client");

reverseProxyClient.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseMiddleware<ReverseProxyMiddleware>();

app.MapFallbackToFile("/index.html");

app.Run();