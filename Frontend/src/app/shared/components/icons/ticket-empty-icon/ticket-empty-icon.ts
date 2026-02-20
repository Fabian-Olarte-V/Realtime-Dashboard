import { Component, input } from '@angular/core';

@Component({
  selector: 'app-ticket-empty-icon',
  standalone: true,
  templateUrl: './ticket-empty-icon.html',
  styleUrl: './ticket-empty-icon.scss',
})
export class TicketEmptyIconComponent {
  readonly size = input(68);
}
