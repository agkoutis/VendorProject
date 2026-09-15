import { Routes } from '@angular/router';
import { VendorCreatePage } from './vendor-create/vendor-create';
import { VendorDeletePage } from './vendor-delete/vendor-delete';
import { VendorListPage } from './vendor-list/vendor-list';
import { VendorLookupPage } from './vendor-lookup/vendor-lookup';
import { VendorUpdatePage } from './vendor-update/vendor-update';

export const VENDOR_ROUTES: Routes = [
  { path: '', component: VendorListPage, title: 'Get all vendors' },
  { path: 'lookup', component: VendorLookupPage, title: 'Get vendor by id' },
  { path: 'lookup/:id', component: VendorLookupPage, title: 'Get vendor by id' },
  { path: 'create', component: VendorCreatePage, title: 'Create vendor' },
  { path: 'update', component: VendorUpdatePage, title: 'Update vendor' },
  { path: 'update/:id', component: VendorUpdatePage, title: 'Update vendor' },
  { path: 'delete', component: VendorDeletePage, title: 'Delete vendor' },
  { path: 'delete/:id', component: VendorDeletePage, title: 'Delete vendor' }
];
