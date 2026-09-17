import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Vendor, emptyVendor } from '../../../core/models/vendor';
import { VendorService } from '../../../core/services/vendor.service';
import { getErrorMessage } from '../../../core/utils/error-message';
import { PageFeedback } from '../../../shared/page-feedback/page-feedback';

@Component({
  selector: 'app-vendor-update',
  standalone: true,
  imports: [FormsModule, RouterLink, PageFeedback],
  templateUrl: './vendor-update.html'
})
export class VendorUpdatePage implements OnInit {
  private readonly vendorService = inject(VendorService);
  private readonly route = inject(ActivatedRoute);
  private readonly destroyRef = inject(DestroyRef);
  private loadRequestId = 0;

  form: Vendor = emptyVendor();
  loadedId = '';
  isLoading = false;
  isSaving = false;
  error = '';
  message = '';

  get canUpdate() {
    return this.loadedId !== '' && this.loadedId === this.form.id.trim();
  }

  ngOnInit() {
    this.route.paramMap.pipe(takeUntilDestroyed(this.destroyRef)).subscribe((params) => {
      const id = params.get('id');
      if (id) {
        this.resetForm(id);
        this.loadVendor(id);
      }
    });
  }

  loadVendor(id = this.form.id) {
    const vendorId = id.trim();
    if (!vendorId) {
      this.error = 'Enter an id to load.';
      this.message = '';
      return;
    }

    const requestId = ++this.loadRequestId;
    this.resetForm(vendorId);
    this.isLoading = true;
    this.error = '';
    this.message = '';

    this.vendorService.getById(vendorId).subscribe({
      next: (vendor) => {
        if (requestId !== this.loadRequestId) {
          return;
        }

        this.form = { ...vendor };
        this.loadedId = vendor.id;
        this.isLoading = false;
        this.message = 'Vendor loaded.';
      },
      error: (error) => {
        if (requestId !== this.loadRequestId) {
          return;
        }

        this.loadedId = '';
        this.isLoading = false;
        this.error = getErrorMessage(error, 'Failed to load vendor.');
      }
    });
  }

  updateVendor() {
    const id = this.form.id.trim();
    const name = this.form.name.trim();
    const address = this.form.address.trim();
    if (!id || !name || !address) {
      this.error = 'Id, name, and address are required.';
      this.message = '';
      return;
    }

    if (!this.canUpdate) {
      this.error = 'Load the vendor before updating.';
      this.message = '';
      return;
    }

    this.isSaving = true;
    this.error = '';
    this.message = '';

    this.vendorService.update({ id, name, address }).subscribe({
      next: (vendor) => {
        this.form = { ...vendor };
        this.loadedId = vendor.id;
        this.isSaving = false;
        this.message = 'Vendor updated.';
      },
      error: (error) => {
        this.isSaving = false;
        this.error = getErrorMessage(error, 'Failed to update vendor.');
      }
    });
  }

  private resetForm(id: string) {
    this.form = { id, name: '', address: '' };
    this.loadedId = '';
  }
}
