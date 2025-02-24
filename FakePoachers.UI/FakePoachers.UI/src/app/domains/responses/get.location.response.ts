import { LocationDTO } from '../dtos/locations.dto';
import { BaseResponse } from '../models/baseresponse.model';

export class GetLocationsResponse extends BaseResponse {
  content: LocationDTO[] = [];
}
