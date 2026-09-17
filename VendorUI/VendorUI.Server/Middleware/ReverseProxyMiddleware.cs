using Microsoft.Extensions.Primitives;
using VendorUI.Server.Extensions;

namespace VendorUI.Server.Middleware
{
    public partial class ReverseProxyMiddleware
    {
        private readonly RequestDelegate _nextMiddleware;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDictionary<string, string?> _reverseProxyTargets;
        private readonly ILogger<ReverseProxyMiddleware> _logger;

        public ReverseProxyMiddleware(
            RequestDelegate nextMiddleware,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            ILogger<ReverseProxyMiddleware> logger)
        {
            _nextMiddleware = nextMiddleware;
            _httpClientFactory = httpClientFactory;
            _reverseProxyTargets = configuration
                .GetSection("ReverseProxyTargetUrl")
                .GetChildren()
                .ToDictionary(x => x.Key.EnsureLeadingSlash(), x => x.Value);
            _logger = logger;
        }

        public Task Invoke(HttpContext context)
        {
            return HandleRequest(context);
        }

        private async Task HandleRequest(HttpContext context)
        {
            var targetApi = _reverseProxyTargets
                .Where(x => !string.IsNullOrWhiteSpace(x.Value)
                    && context.Request.Path.StartsWithSegments(x.Key, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(x => x.Key.Length)
                .FirstOrDefault();

            if (targetApi.Value != null)
            {
                var targetUri = new Uri(
                    targetApi.Value.TrimEnd('/')
                    + context.Request.Path.RemoveProxyKeySource(targetApi.Key)
                    + context.Request.QueryString);

                await ProxyThisRequest(context, targetUri);
                return;
            }

            await _nextMiddleware(context);
        }

        private async Task ProxyThisRequest(HttpContext context, Uri targetUri)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("reverse-proxy-client");
                using var targetRequestMessage = CreateTargetMessage(context, targetUri);
                using var responseMessage = await httpClient.SendAsync(
                    targetRequestMessage,
                    HttpCompletionOption.ResponseHeadersRead,
                    context.RequestAborted);

                context.Response.StatusCode = (int)responseMessage.StatusCode;
                CopyFromTargetResponseHeaders(context, responseMessage);
                await responseMessage.Content.CopyToAsync(context.Response.Body, context.RequestAborted);
            }
            catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
            {
                LogRequestCancelled(targetUri);
            }
            catch (Exception ex)
            {
                LogProxyFailed(ex, targetUri);

                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                }
            }
        }

        private static HttpRequestMessage CreateTargetMessage(HttpContext context, Uri targetUri)
        {
            var requestMessage = new HttpRequestMessage();
            CopyFromOriginalRequestContentAndHeaders(context, requestMessage);

            requestMessage.RequestUri = targetUri;
            requestMessage.Headers.Host = targetUri.Host;
            requestMessage.Method = GetMethod(context.Request.Method);
            return requestMessage;
        }

        private static void CopyFromOriginalRequestContentAndHeaders(HttpContext context, HttpRequestMessage requestMessage)
        {
            var requestMethod = context.Request.Method;

            if (!HttpMethods.IsGet(requestMethod)
                && !HttpMethods.IsHead(requestMethod)
                && !HttpMethods.IsDelete(requestMethod)
                && !HttpMethods.IsTrace(requestMethod))
            {
                requestMessage.Content = new StreamContent(context.Request.Body);

                foreach (KeyValuePair<string, StringValues> header in context.Request.Headers.Where(ShouldForwardHeader))
                {
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                    requestMessage.Content.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }
            else
            {
                foreach (KeyValuePair<string, StringValues> header in context.Request.Headers.Where(ShouldForwardHeader))
                {
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }
        }

        private static bool ShouldForwardHeader(KeyValuePair<string, StringValues> header)
        {
            return !string.Equals(header.Key, "cookie", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(header.Key, "host", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(header.Key, "content-length", StringComparison.OrdinalIgnoreCase)
                && !IsConnectionSpecificHeader(header.Key);
        }

        private static void CopyFromTargetResponseHeaders(HttpContext context, HttpResponseMessage responseMessage)
        {
            foreach (var header in responseMessage.Headers)
            {
                if (IsConnectionSpecificHeader(header.Key)
                    || string.Equals(header.Key, "content-length", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                context.Response.Headers[header.Key] = header.Value.ToArray();
            }

            foreach (var header in responseMessage.Content.Headers)
            {
                if (IsConnectionSpecificHeader(header.Key)
                    || string.Equals(header.Key, "content-length", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                context.Response.Headers[header.Key] = header.Value.ToArray();
            }

            // Streamed CopyToAsync: Kestrel owns framing (typically chunked).
            // Upstream Content-Length is for the proxy↔API connection, not the browser.
            context.Response.Headers.Remove("transfer-encoding");
            context.Response.Headers.Remove("content-length");
        }

        private static bool IsConnectionSpecificHeader(string name)
        {
            return string.Equals(name, "connection", StringComparison.OrdinalIgnoreCase)
                || string.Equals(name, "keep-alive", StringComparison.OrdinalIgnoreCase)
                || string.Equals(name, "proxy-authenticate", StringComparison.OrdinalIgnoreCase)
                || string.Equals(name, "proxy-authorization", StringComparison.OrdinalIgnoreCase)
                || string.Equals(name, "proxy-connection", StringComparison.OrdinalIgnoreCase)
                || string.Equals(name, "transfer-encoding", StringComparison.OrdinalIgnoreCase)
                || string.Equals(name, "upgrade", StringComparison.OrdinalIgnoreCase);
        }

        private static HttpMethod GetMethod(string method)
        {
            if (HttpMethods.IsDelete(method)) return HttpMethod.Delete;
            if (HttpMethods.IsGet(method)) return HttpMethod.Get;
            if (HttpMethods.IsHead(method)) return HttpMethod.Head;
            if (HttpMethods.IsOptions(method)) return HttpMethod.Options;
            if (HttpMethods.IsPatch(method)) return HttpMethod.Patch;
            if (HttpMethods.IsPost(method)) return HttpMethod.Post;
            if (HttpMethods.IsPut(method)) return HttpMethod.Put;
            if (HttpMethods.IsTrace(method)) return HttpMethod.Trace;
            return new HttpMethod(method);
        }

        [LoggerMessage(
            EventId = 1,
            Level = LogLevel.Information,
            Message = "Reverse proxy request was cancelled by the client. Target: {TargetUri}")]
        private partial void LogRequestCancelled(Uri targetUri);

        [LoggerMessage(
            EventId = 2,
            Level = LogLevel.Error,
            Message = "ReverseProxyMiddleware - ProxyThisRequest: Cannot proxy request to {TargetUri}")]
        private partial void LogProxyFailed(Exception exception, Uri targetUri);
    }
}