import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Store } from '@ngrx/store';
import * as AuthActions from '../../store/auth.actions';
import { AuthPage } from './auth-page';

describe('AuthPage', () => {
  let component: AuthPage;
  let fixture: ComponentFixture<AuthPage>;
  const dispatch = vi.fn();

  beforeEach(async () => {
    dispatch.mockReset();

    await TestBed.configureTestingModule({
      imports: [AuthPage],
      providers: [
        {
          provide: Store,
          useValue: {
            dispatch,
            selectSignal: () => () => null,
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AuthPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should switch to register mode and clear auth error', () => {
    component.showRegister();

    expect(component.isLoginMode).toBe(false);
    expect(dispatch).toHaveBeenCalledWith(AuthActions.clearAuthError());
  });

  it('should dispatch login with the submitted payload', () => {
    component.onLoginSubmit({
      username: 'agent01',
      password: 'password123',
    });

    expect(dispatch).toHaveBeenCalledWith(
      AuthActions.login({
        user: {
          username: 'agent01',
          password: 'password123',
        },
      }),
    );
  });
});
