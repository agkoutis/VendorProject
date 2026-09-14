import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { App } from './app';

describe('App', () => {
  let fixture: ComponentFixture<App>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [App],
      imports: [RouterTestingModule]
    }).compileComponents();

    fixture = TestBed.createComponent(App);
  });

  it('should create the app shell', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});
