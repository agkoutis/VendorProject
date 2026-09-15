# VendorProject

## API

The controller uses **ActionHandler**, the **service**, and the **repository**:

- **ActionHandler** runs the request and turns errors into a clear response. If the service, repository, or a loader throws exception, ActionHandler turns it into AppResponse.Fail with the exception message and still returns HTTP 200.

- **Service** does the vendor work (list, add, update, delete).

- **Repository** reads and writes vendor data.

Switch the data source in `vendor.api/VendorLoaderSettings.json`:

- `"Sql"` — SQL loader
- `"File"` — file loader

Restart the API after you save.

## UI

The Angular service calls the controllers. Reverse proxy middleware (ReverseProxyMiddleware c# class) forwards those HTTP calls to Vendor API.

Set the API URL in `VendorUI/VendorUI.Server/appsettings.json`:

```json
"ReverseProxyTargetUrl": {
  "/api": "https://localhost:7100"
}
```

Restart `VendorUI.Server` after you save.
