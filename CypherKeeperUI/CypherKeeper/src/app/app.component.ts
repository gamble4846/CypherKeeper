import { Component } from '@angular/core';
import { FormsService } from './Modules/SharedModule/Services/OtherServices/forms.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'CypherKeeper';
  constructor(
  ) {}

  ngOnInit(): void {
  }
}
