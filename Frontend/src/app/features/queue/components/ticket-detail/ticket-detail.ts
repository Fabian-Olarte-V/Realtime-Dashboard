import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { QueueItem } from '../../models/queue';
import { CommonModule } from '@angular/common';
import { TicketEmptyIconComponent } from '../../../../shared/components/icons/ticket-empty-icon/ticket-empty-icon';

@Component({
  selector: 'app-ticket-detail',
  imports: [CommonModule, TicketEmptyIconComponent],
  templateUrl: './ticket-detail.html',
  styleUrl: './ticket-detail.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TicketDetailComponent {
  @Input({required: true}) item: QueueItem | null = null;
  @Input({required: true}) canAssignToMe = false;
  @Input({required: true}) canComplete = false;
  @Input() isLoading = false;
  @Input() error: string | null = null;

  @Output() assignToMe = new EventEmitter<void>();
  @Output() complete = new EventEmitter<void>();
}
