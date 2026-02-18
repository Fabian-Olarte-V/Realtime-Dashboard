import { Component, input } from '@angular/core';

@Component({
  selector: 'app-eye-icon',
  standalone: true,
  templateUrl: './eye-icon.html',
  styleUrl: './eye-icon.scss',
})
export class EyeIconComponent {
  readonly size = input(18);
}
