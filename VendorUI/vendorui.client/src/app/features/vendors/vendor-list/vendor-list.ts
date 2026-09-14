import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Vendor } from '../../../core/models/vendor';
import { VendorService } from '../../../core/services/vendor.service';
import { getErrorMessage } from '../../../core/utils/error-message';
import { PageFeedback } from '../../../shared/page-feedback/page-feedback';

@Component({
  selector: 'app-vendor-list',
  standalone: true,
  imports: [RouterLink, PageFeedback],
  templateUrl: './vendor-list.html'
})
export class VendorListPage implements OnInit {
  private readonly vendorService = inject(VendorService);

  vendors: Vendor[] = [];
  isLoading = false;
  error = '';
  message = '';

  ngOnInit() {
    this.loadVendors();
  }

  loadVendors() {
    this.isLoading = true;
    this.error = '';
    this.message = '';

    this.vendorService.getAll().subscribe({
      next: (vendors) => {
        this.vendors = vendors ?? [];
        this.isLoading = false;
        this.message = 'Vendors loaded.';
      },
      error: (error) => {
        this.vendors = [];
        this.isLoading = false;
        this.error = getErrorMessage(error, 'Failed to load vendors.');
      }
    });
  }
}
