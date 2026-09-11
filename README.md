# VendorProject

## How to switch the data source

The API can read vendors from **SQL** or from a **file**

1. Open `vendor.api/VendorLoaderSettings.json`.
2. Change `SelectedLoaderType`:
   - `"Sql"` — use the SQL loader
   - `"File"` — use the file loader
3. Save the file and **restart the API**.

