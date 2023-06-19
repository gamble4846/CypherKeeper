import { Injectable } from '@angular/core';
import { CommonService } from './common.service';

@Injectable({
  providedIn: 'root'
})
export class AppInitializerService {

  constructor(
    private _CommonService:CommonService,
  ) { }

  publicKey:string = '';
  privateKey:string = '';

  loadEverything(){
    var RSAKeys = this._CommonService.GenerateRSAPairKeys();
    this.publicKey = RSAKeys.PublicKey;
    this.privateKey = RSAKeys.PrivateKey;
  }
}
