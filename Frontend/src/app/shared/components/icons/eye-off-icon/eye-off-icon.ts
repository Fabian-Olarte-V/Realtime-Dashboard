import { Component, input } from '@angular/core';

@Component({
  selector: 'app-eye-off-icon',
  standalone: true,
  templateUrl: './eye-off-icon.html',
  styleUrl: './eye-off-icon.scss',
})
export class EyeOffIconComponent {
  readonly size = input(18);
}
