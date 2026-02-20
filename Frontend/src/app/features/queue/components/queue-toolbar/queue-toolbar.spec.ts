import { ComponentFixture, TestBed } from '@angular/core/testing';
import { QueueToolbar } from './queue-toolbar';

describe('QueueToolbar', () => {
  let component: QueueToolbar;
  let fixture: ComponentFixture<QueueToolbar>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QueueToolbar]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QueueToolbar);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
