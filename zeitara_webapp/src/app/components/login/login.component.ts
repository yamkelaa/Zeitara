import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { UserService } from '../../services/user.service';
import { UserLogin, UserLoginResponse } from '../../shared/models/user/user';
import { ResponseDto } from '../../shared/models/response/response';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { Router, RouterModule } from '@angular/router';
import { UserSessionService } from '../../services/user-session.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputTextModule,
    ButtonModule,
    ToastModule,
    RouterModule
  ],
  providers: [MessageService],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  userLoginForm!: FormGroup;
  isLoading = false;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly userService: UserService,
    private readonly messageService: MessageService,
    private readonly router: Router,
    private readonly userSession: UserSessionService
  ) { }

  ngOnInit(): void {
    this.userLoginForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  get username() {
    return this.userLoginForm.get('username');
  }

  get password() {
    return this.userLoginForm.get('password');
  }

  onSubmit(): void {
    if (this.userLoginForm.invalid) {
      this.markFormTouched();
      return;
    }

    this.isLoading = true;
    const loginData: UserLogin = this.userLoginForm.value;

    this.userService.loginUser(loginData).subscribe({
      next: (response: ResponseDto<UserLoginResponse>) => {
        this.userSession.setCurrentUser({
          userId: response.data.user_id,
          username: response.data.username
        });

        this.messageService.add({
          severity: 'success',
          summary: 'Login Successful',
          detail: `Welcome back, ${response.data.username}!`,
          life: 3000
        });

        this.router.navigate(['/products']);
      },
      error: (err) => {
        this.isLoading = false;
        const errorDetail = err.error?.message || 'Invalid username or password';
        this.messageService.add({
          severity: 'error',
          summary: 'Login Failed',
          detail: errorDetail,
          life: 5000
        });
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  private markFormTouched(): void {
    Object.values(this.userLoginForm.controls).forEach(control => {
      control.markAsTouched();
    });
  }
}
