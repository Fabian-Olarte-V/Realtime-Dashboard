import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class UiErrorService {
  readonly errorMessage = signal<string | null>(null);
  private clearTimeoutId: ReturnType<typeof setTimeout> | null = null;

  show(message: string): void {
    this.errorMessage.set(message);

    if (this.clearTimeoutId) {
      clearTimeout(this.clearTimeoutId);
    }

    this.clearTimeoutId = setTimeout(() => {
      this.clear();
    }, 2000);
  }

  clear(): void {
    this.errorMessage.set(null);

    if (this.clearTimeoutId) {
      clearTimeout(this.clearTimeoutId);
      this.clearTimeoutId = null;
    }
  }
}
