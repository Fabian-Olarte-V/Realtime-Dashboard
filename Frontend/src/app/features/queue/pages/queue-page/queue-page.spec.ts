import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { Store } from '@ngrx/store';
import * as QueueActions from '../../store/queue.actions';
import { QueuePage } from './queue-page';

describe('QueuePage', () => {
  let component: QueuePage;
  let fixture: ComponentFixture<QueuePage>;
  const dispatch = vi.fn();

  beforeEach(async () => {
    dispatch.mockReset();

    await TestBed.configureTestingModule({
      imports: [QueuePage],
      providers: [
        {
          provide: Store,
          useValue: {
            dispatch,
            select: () => of(null),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(QueuePage);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should start polling on init', () => {
    component.ngOnInit();

    expect(dispatch).toHaveBeenCalledWith(QueueActions.startPolling());
  });

  it('should dispatch selected ticket id', () => {
    component.onSelectItem('ticket-123');

    expect(dispatch).toHaveBeenCalledWith(QueueActions.selectTicket({ id: 'ticket-123' }));
  });

  it('should stop polling on destroy', () => {
    component.ngOnDestroy();

    expect(dispatch).toHaveBeenCalledWith(QueueActions.stopPolling());
  });
});
