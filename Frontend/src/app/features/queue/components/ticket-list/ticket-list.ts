import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { QueueItem } from '../../models/queue';
import { ScrollingModule } from '@angular/cdk/scrolling';

@Component({
  selector: 'app-ticket-list',
  imports: [ScrollingModule],
  templateUrl: './ticket-list.html',
  styleUrl: './ticket-list.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TicketListComponent {
  @Input({ required: true }) items!: readonly QueueItem[];
  @Input() selectedId: string | null = null;

  @Output() selectItem = new EventEmitter<string>();
  trackById = (_: number, item: QueueItem) => item.id;
}
