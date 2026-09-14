import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { AppResponse } from '../models/app-response';
import { Vendor, VendorRequest } from '../models/vendor';

@Injectable({ providedIn: 'root' })
export class VendorService {
  private readonly http = inject(HttpClient);
  private readonly vendorUrl = '/api/Vendor';

  getAll(): Observable<Vendor[]> {
    return this.http.get<AppResponse<Vendor[]>>(this.vendorUrl).pipe(this.unwrap());
  }

  getById(id: string): Observable<Vendor> {
    return this.http
      .get<AppResponse<Vendor>>(`${this.vendorUrl}/${encodeURIComponent(id)}`)
      .pipe(this.unwrap());
  }

  create(vendor: VendorRequest): Observable<boolean> {
    return this.http.post<AppResponse<boolean>>(this.vendorUrl, vendor).pipe(this.unwrap());
  }

  update(vendor: Vendor): Observable<boolean> {
    return this.http.put<AppResponse<boolean>>(this.vendorUrl, vendor).pipe(this.unwrap());
  }

  delete(id: string): Observable<boolean> {
    return this.http
      .delete<AppResponse<boolean>>(`${this.vendorUrl}/${encodeURIComponent(id)}`)
      .pipe(this.unwrap());
  }

  private unwrap<T>() {
    return (source: Observable<AppResponse<T>>) =>
      source.pipe(
        map((response) => {
          if (response.hasError) {
            throw new Error(response.error || 'Request failed.');
          }

          return response.data;
        })
      );
  }
}
