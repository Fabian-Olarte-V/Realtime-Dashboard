import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QueuePage } from './queue-page';

describe('QueuePage', () => {
  let component: QueuePage;
  let fixture: ComponentFixture<QueuePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QueuePage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QueuePage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
