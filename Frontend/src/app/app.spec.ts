import { TestBed } from '@angular/core/testing';
import { Router, NavigationEnd } from '@angular/router';
import { Subject } from 'rxjs';
import { Store } from '@ngrx/store';
import * as AuthActions from './features/auth/store/auth.actions';
import { App } from './app';

interface RouterSnapshotNode{
  data: Record<string, unknown>;
  firstChild: RouterSnapshotNode | null;
};


describe('App', () => {
  const routerEvents$ = new Subject<NavigationEnd>();
  const dispatch = vi.fn();

  const routerStub: Pick<Router, 'events'> & {
    routerState: {
      snapshot: {
        root: RouterSnapshotNode;
      };
    };
  } = {
    events: routerEvents$.asObservable(),
    routerState: {
      snapshot: {
        root: {
          data: {},
          firstChild: null,
        },
      },
    },
  };

  beforeEach(async () => {
    dispatch.mockReset();
    routerStub.routerState.snapshot.root = {
      data: {},
      firstChild: null,
    };

    await TestBed.configureTestingModule({
      imports: [App],
      providers: [
        { provide: Store, useValue: { dispatch } },
        { provide: Router, useValue: routerStub },
      ],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('should hide the header when the current route marks hideHeader', () => {
    routerStub.routerState.snapshot.root = {
      data: {},
      firstChild: {
        data: { hideHeader: true },
        firstChild: null,
      },
    };

    const fixture = TestBed.createComponent(App);
    routerEvents$.next(new NavigationEnd(1, '/login', '/login'));

    expect(fixture.componentInstance.showHeader).toBe(false);
  });

  it('should dispatch logout when logout is triggered', () => {
    const fixture = TestBed.createComponent(App);

    fixture.componentInstance.logout();

    expect(dispatch).toHaveBeenCalledWith(AuthActions.logout());
  });
});
