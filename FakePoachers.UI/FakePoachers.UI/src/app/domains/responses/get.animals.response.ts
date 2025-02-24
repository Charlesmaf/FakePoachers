import { AnimalListDTO } from '../dtos/animallist.dto';
import { BaseResponse } from '../models/baseresponse.model';

export class GetAnimalsResponse extends BaseResponse {
  content: AnimalListDTO[] = [];
}
