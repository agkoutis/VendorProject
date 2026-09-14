import { Component, input } from '@angular/core';

@Component({
  selector: 'app-page-feedback',
  standalone: true,
  template: `
    @if (message()) {
      <p class="message">{{ message() }}</p>
    }
    @if (error()) {
      <p class="error">{{ error() }}</p>
    }
  `
})
export class PageFeedback {
  readonly message = input('');
  readonly error = input('');
}
