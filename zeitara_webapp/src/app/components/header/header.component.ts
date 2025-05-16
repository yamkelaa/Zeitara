import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { PrimeIcons } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { UserSessionService } from '../../services/user-session.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styles: [`
    .p-button {
      transition: all 0.3s ease;
    }
    .p-button:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
    }
  `],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputTextModule,
    ButtonModule,
    ToastModule,
    RouterModule
  ],
  standalone: true // Add this if you're using standalone components
})
export class HeaderComponent {
  PrimeIcons = PrimeIcons;

  constructor(
    private router: Router,
    private userSessionService: UserSessionService
  ) { }

  navigateTo(route: string): void {
    this.router.navigate([route]);
  }

  logout(): void {
    this.userSessionService.clearCurrentUser();
    this.router.navigate(['']);
  }
}
