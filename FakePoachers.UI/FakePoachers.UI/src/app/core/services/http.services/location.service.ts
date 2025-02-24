import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { environment } from '../../../../enviroment/environment';
import { ErrorHandlingService } from '../error-handling.service';
import { GetLocationsResponse } from '../../../domains/responses/get.location.response';

@Injectable({
  providedIn: 'root',
})
export class LocationService {
  constructor(
    private http: HttpClient,
    private errorHandlingService: ErrorHandlingService
  ) {}

  getLocations(): Observable<GetLocationsResponse> {
    const url = `${environment.devPoacherService}/Location/GetLocations`;
    return this.http
      .get<GetLocationsResponse>(url)
      .pipe(catchError(this.errorHandlingService.handleError));
  }
}
