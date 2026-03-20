import { ComponentFixture, TestBed } from '@angular/core/testing';
import { QueueToolbarComponent } from './queue-toolbar';


describe('QueueToolbar', () => {
  let component: QueueToolbarComponent;
  let fixture: ComponentFixture<QueueToolbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QueueToolbarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QueueToolbarComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
