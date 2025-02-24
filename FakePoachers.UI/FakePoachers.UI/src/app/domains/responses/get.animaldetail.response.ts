import { AnimalDetailsDTO } from '../dtos/animaldetails.dto';
import { BaseResponse } from '../models/baseresponse.model';

export class GetAnimalDetailResponse extends BaseResponse {
  content: AnimalDetailsDTO | null | undefined;
}
