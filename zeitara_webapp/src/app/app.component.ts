import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { LoginComponent } from './components/login/login.component';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  imports: [ButtonModule, LoginComponent]
})
export class AppComponent {
  title = 'zeitara_webapp';
}
