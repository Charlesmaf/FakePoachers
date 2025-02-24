import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ErrorHandlingService {
  constructor() {}

  handleError(error: HttpErrorResponse) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `An error occurred: ${error.error.message}`;
    } else {
      switch (error.status) {
        case 400:
          errorMessage = error.error.additionalInformation;
          break;
        case 401:
          errorMessage = 'Unauthorized.';
          break;
        case 403:
          errorMessage = 'Forbidden.';
          break;
        case 404:
          errorMessage = 'Not found.';
          break;
        case 409:
          errorMessage = error.error.message || 'Conflict occurred.';
          break;
        case 500:
          errorMessage = 'Internal server error.';
          break;
        default:
          errorMessage = `Unexpected error please contact support xxx-xxx-xxx`;
          break;
      }
    }
    return throwError(() => new Error(errorMessage));
  }
}
