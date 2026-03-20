import { Component, inject } from '@angular/core';
import { UiErrorService } from '../../services/ui-error.service';

@Component({
  selector: 'app-error-handler-toast',
  standalone: true,
  imports: [],
  templateUrl: './error-handler-toast.html',
  styleUrl: './error-handler-toast.scss',
})
export class ErrorHandlerToast {
  readonly uiError = inject(UiErrorService);
}
