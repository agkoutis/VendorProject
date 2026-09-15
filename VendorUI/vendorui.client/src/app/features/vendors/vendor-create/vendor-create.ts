import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { VendorRequest } from '../../../core/models/vendor';
import { VendorService } from '../../../core/services/vendor.service';
import { getErrorMessage } from '../../../core/utils/error-message';
import { PageFeedback } from '../../../shared/page-feedback/page-feedback';

@Component({
  selector: 'app-vendor-create',
  standalone: true,
  imports: [FormsModule, RouterLink, PageFeedback],
  templateUrl: './vendor-create.html'
})
export class VendorCreatePage {
  private readonly vendorService = inject(VendorService);

  form: VendorRequest = { name: '', address: '' };
  isSaving = false;
  error = '';
  message = '';

  createVendor() {
    const name = this.form.name.trim();
    const address = this.form.address.trim();
    if (!name || !address) {
      this.error = 'Name and address are required.';
      this.message = '';
      return;
    }

    this.isSaving = true;
    this.error = '';
    this.message = '';

    this.vendorService.create({ name, address }).subscribe({
      next: () => {
        this.isSaving = false;
        this.form = { name: '', address: '' };
        this.message = 'Vendor created.';
      },
      error: (error) => {
        this.isSaving = false;
        this.error = getErrorMessage(error, 'Failed to create vendor.');
      }
    });
  }
}
