import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Vendor } from '../../../core/models/vendor';
import { VendorService } from '../../../core/services/vendor.service';
import { getErrorMessage } from '../../../core/utils/error-message';
import { PageFeedback } from '../../../shared/page-feedback/page-feedback';

@Component({
  selector: 'app-vendor-lookup',
  standalone: true,
  imports: [FormsModule, RouterLink, PageFeedback],
  templateUrl: './vendor-lookup.html'
})
export class VendorLookupPage implements OnInit {
  private readonly vendorService = inject(VendorService);
  private readonly route = inject(ActivatedRoute);

  lookupId = '';
  vendor: Vendor | null = null;
  isLoading = false;
  error = '';
  message = '';

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      const id = params.get('id');
      if (id) {
        this.lookupId = id;
        this.loadVendor();
      }
    });
  }

  loadVendor() {
    const id = this.lookupId.trim();
    if (!id) {
      this.error = 'Enter an id to look up.';
      this.message = '';
      this.vendor = null;
      return;
    }

    this.isLoading = true;
    this.error = '';
    this.message = '';
    this.vendor = null;

    this.vendorService.getById(id).subscribe({
      next: (vendor) => {
        this.vendor = vendor;
        this.lookupId = vendor.id;
        this.isLoading = false;
        this.message = 'Vendor loaded.';
      },
      error: (error) => {
        this.isLoading = false;
        this.error = getErrorMessage(error, 'Failed to load vendor.');
      }
    });
  }
}
