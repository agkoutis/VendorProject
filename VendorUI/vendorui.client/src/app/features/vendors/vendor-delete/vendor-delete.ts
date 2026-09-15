import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Vendor } from '../../../core/models/vendor';
import { VendorService } from '../../../core/services/vendor.service';
import { getErrorMessage } from '../../../core/utils/error-message';
import { PageFeedback } from '../../../shared/page-feedback/page-feedback';

@Component({
  selector: 'app-vendor-delete',
  standalone: true,
  imports: [FormsModule, RouterLink, PageFeedback],
  templateUrl: './vendor-delete.html'
})
export class VendorDeletePage implements OnInit {
  private readonly vendorService = inject(VendorService);
  private readonly route = inject(ActivatedRoute);

  vendorId = '';
  vendor: Vendor | null = null;
  isLoading = false;
  isSaving = false;
  error = '';
  message = '';

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      const id = params.get('id');
      if (id) {
        this.vendorId = id;
        this.loadVendor();
      }
    });
  }

  loadVendor() {
    const id = this.vendorId.trim();
    if (!id) {
      this.error = 'Enter an id to delete.';
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
        this.vendorId = vendor.id;
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.error = getErrorMessage(error, 'Failed to load vendor.');
      }
    });
  }

  deleteVendor() {
    const id = this.vendorId.trim();
    if (!id) {
      this.error = 'Enter an id to delete.';
      this.message = '';
      return;
    }

    this.isSaving = true;
    this.error = '';
    this.message = '';

    this.vendorService.delete(id).subscribe({
      next: () => {
        this.isSaving = false;
        this.vendor = null;
        this.vendorId = '';
        this.message = 'Vendor deleted.';
      },
      error: (error) => {
        this.isSaving = false;
        this.error = getErrorMessage(error, 'Failed to delete vendor.');
      }
    });
  }
}
