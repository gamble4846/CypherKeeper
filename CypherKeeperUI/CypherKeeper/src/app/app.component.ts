import { Component } from '@angular/core';
import { FormsService } from './Modules/SharedModule/Services/OtherServices/forms.service';
import { AppInitializerService } from './Modules/SharedModule/Services/OtherServices/app-initializer.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'CypherKeeper';
  constructor(
    private _AppInitializerService:AppInitializerService
  ) {}

  ngOnInit(): void {
    
  }
}
