import { BaseResponse } from '../models/baseresponse.model';

export class PostAddAnimalResponse extends BaseResponse {
  id: string | undefined;
  constructor(id: string | undefined) {
    super();
    this.id = id;
  }
}
