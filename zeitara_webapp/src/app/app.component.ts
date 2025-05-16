import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { ActivatedRoute, NavigationEnd, Router, RouterModule } from '@angular/router';
import { HeaderComponent } from "./components/header/header.component";
import { filter, map } from 'rxjs';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  imports: [ButtonModule, RouterModule, HeaderComponent, CommonModule]
})
export class AppComponent {
  showHeader = true;
  tiile = "Zeitara"

  constructor(private router: Router, private activatedRoute: ActivatedRoute) {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        const noHeaderRoutes = ['/', '/register'];
        this.showHeader = !noHeaderRoutes.includes(event.urlAfterRedirects);
      });
  }
}
