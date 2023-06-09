import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ErrorInterceptor } from './Modules/SharedModule/Interceptors/error.interceptor';
import { TokenInterceptor } from './Modules/SharedModule/Interceptors/token.interceptor';
import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { en_US } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DecryptInterceptor } from './Modules/SharedModule/Interceptors/decrypt.interceptor';
import { AppInitializerService } from './Modules/SharedModule/Services/OtherServices/app-initializer.service';
import { PublicEncryptionKeyInterceptor } from './Modules/SharedModule/Interceptors/public-encryption-key.interceptor';

registerLocaleData(en);

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule
  ],
  providers: [
    AppInitializerService,
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: DecryptInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: PublicEncryptionKeyInterceptor, multi: true },
    { provide: NZ_I18N, useValue: en_US },
    { provide: APP_INITIALIZER, multi: true, deps: [AppInitializerService], useFactory: (_AppInitializerService: AppInitializerService) => { return () => { return _AppInitializerService.loadEverything(); }; }, },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
