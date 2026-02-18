import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { QueueFilters } from '../../models/queue';

@Component({
  selector: 'app-queue-toolbar',
  standalone: true,
  templateUrl: './queue-toolbar.html',
  styleUrl: './queue-toolbar.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class QueueToolbarComponent {
  @Input({ required: true }) filters!: QueueFilters;
  @Output() filtersChange = new EventEmitter<QueueFilters>();

  onQueryInput(event: Event): void {
    const value = (event.target as HTMLInputElement).value ?? '';
    this.filtersChange.emit({ ...this.filters, searchText: value });
  }

  onStatusChange(event: Event): void {
    const value = (event.target as HTMLSelectElement).value as QueueFilters['status'];
    this.filtersChange.emit({ ...this.filters, status: value });
  }

  onSortChange(event: Event): void {
    const value = (event.target as HTMLSelectElement).value as QueueFilters['sort'];
    this.filtersChange.emit({ ...this.filters, sort: value });
  }
}
