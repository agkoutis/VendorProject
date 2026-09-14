export interface Vendor {
  id: string;
  name: string;
  address: string;
}

export interface VendorRequest {
  name: string;
  address: string;
}

export function emptyVendor(): Vendor {
  return { id: '', name: '', address: '' };
}
