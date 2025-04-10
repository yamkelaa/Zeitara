import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { UserService } from '../../services/user.service';
import { UserLogin } from '../../shared/models/user/user';
import { ResponseDto } from '../../shared/models/response/response';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api'; // For toast notifications

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputTextModule,
    ButtonModule,
    ToastModule
  ],
  providers: [MessageService], // Provide MessageService for toast notifications
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  userLoginForm!: FormGroup;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly userService: UserService,
    private messageService: MessageService // Inject MessageService
  ) { }

  ngOnInit(): void {
    this.userLoginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  // Getters to simplify access to form controls
  get username() {
    return this.userLoginForm.get('username');
  }

  get password() {
    return this.userLoginForm.get('password');
  }

  onSubmit(): void {
    if (this.userLoginForm.invalid) {
      return;
    }

    const loginData: UserLogin = this.userLoginForm.value;

    this.userService.loginUser(loginData).subscribe({
      next: (response: ResponseDto<UserLogin>) => {
        // Show success toast
        this.messageService.add({ severity: 'success', summary: 'Login Successful', detail: 'Welcome!' });
      },
      error: (err) => {
        // Show error toast
        this.messageService.add({ severity: 'error', summary: 'Login Failed', detail: 'Invalid username or password.' });
      }
    });
  }
}
