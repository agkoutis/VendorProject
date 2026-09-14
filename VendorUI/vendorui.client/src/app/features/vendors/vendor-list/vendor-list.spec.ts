import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { of } from 'rxjs';
import { VendorService } from '../../../core/services/vendor.service';
import { VendorListPage } from './vendor-list';

describe('VendorListPage', () => {
  let fixture: ComponentFixture<VendorListPage>;
  let component: VendorListPage;
  const vendorServiceSpy = jasmine.createSpyObj('VendorService', ['getAll']);

  beforeEach(async () => {
    vendorServiceSpy.getAll.and.returnValue(
      of([
        { id: '1', name: 'Acme', address: 'Athens' },
        { id: '2', name: 'Globex', address: 'Thessaloniki' }
      ])
    );

    await TestBed.configureTestingModule({
      imports: [VendorListPage],
      providers: [
        provideRouter([]),
        { provide: VendorService, useValue: vendorServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(VendorListPage);
    component = fixture.componentInstance;
  });

  it('should load vendors from GetAll', () => {
    fixture.detectChanges();

    expect(vendorServiceSpy.getAll).toHaveBeenCalled();
    expect(component.vendors.length).toBe(2);
    expect(component.vendors[0].name).toBe('Acme');
    expect(component.message).toBe('Vendors loaded.');
  });
});
