import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { PostAddAnimalResponse } from '../../../domains/responses/post.addanimal.response';
import { environment } from '../../../../enviroment/environment';
import { ErrorHandlingService } from '../error-handling.service';
import { GetAnimalsResponse } from '../../../domains/responses/get.animals.response';
import { GetAnimalDetailResponse } from '../../../domains/responses/get.animaldetail.response';

@Injectable({
  providedIn: 'root',
})
export class AnimalService {
  constructor(
    private http: HttpClient,
    private errorHandlingService: ErrorHandlingService
  ) {}
  addAnimal(formData: FormData): Observable<PostAddAnimalResponse> {
    const url = `${environment.devPoacherService}/Animal/AddAnimal`;
    return this.http
      .post<PostAddAnimalResponse>(url, formData)
      .pipe(catchError(this.errorHandlingService.handleError));
  }
  getAnimals(page: number, pageSize: number): Observable<GetAnimalsResponse> {
    const url = `${environment.devPoacherService}/Animal/GetAnimals?page=${page}&pageSize=${pageSize}`;
    return this.http
      .get<GetAnimalsResponse>(url)
      .pipe(catchError(this.errorHandlingService.handleError));
  }

  getAnimalById(id: number): Observable<GetAnimalDetailResponse> {
    const url = `${environment.devPoacherService}/Animal/GetAnimalById/${id}`;
    return this.http.get<GetAnimalDetailResponse>(url);
  }
}
