import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as CONSTANTS from 'src/app/Modules/SharedModule/Constants/CONSTANTS';

@Injectable({
  providedIn: 'root'
})
export class ImageControllerService {

  constructor(private http: HttpClient) { }

  UploadImage(file: File){
    let apiLink = `${CONSTANTS.APIUrl}/api/Image/UploadImage`;

    const formData = new FormData();
    formData.append('image', file);

    return this.http.post(apiLink, formData);
  }
}
