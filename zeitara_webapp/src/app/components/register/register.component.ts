import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { MessageService } from 'primeng/api';
import { Gender, UserRegistrationRequestDto, UserRegistrationResponse } from '../../shared/models/user/user';
import { ToastModule } from 'primeng/toast';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { Router, RouterModule } from '@angular/router';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputTextModule,
    ButtonModule,
    AutoCompleteModule,
    ToastModule,
    RouterModule
  ],
  providers: [MessageService],
})
export class RegisterComponent implements OnInit {
  userRegistrationForm!: FormGroup;
  genderOptions: Gender[] = ['male', 'female', 'other']

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly userService: UserService,
    private readonly messageService: MessageService,
    private readonly router: Router
  ) { }

  ngOnInit(): void {
    this.userRegistrationForm = this.formBuilder.group({
      username: ['', Validators.required],
      user_FirstName: ['', Validators.required],
      user_LastName: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      age: [null, [Validators.required, Validators.min(1)]],
      gender: ['', Validators.required],
      street: ['', Validators.required],
      suburb: ['', Validators.required],
      province: ['', Validators.required],
      postal_Code: ['', [Validators.required, Validators.maxLength(4)]],
      city: ['', Validators.required],
      country: ['', Validators.required],
    });
  }

  search(event: { query: string }) {
    const query = event.query.toLowerCase();
    this.genderOptions = this.genderOptions.filter(
      gender => gender.toLowerCase().includes(query)
    );
  }

  onSubmit(): void {
    if (this.userRegistrationForm.invalid) {
      this.messageService.add({
        severity: 'error',
        summary: 'Form Errors',
        detail: 'Kindly check all form fields',
      });
      return;
    }


    const registrationDetails: UserRegistrationRequestDto = this.userRegistrationForm.value;
    this.userService.registerUser(registrationDetails).subscribe(
      (response) => {
        const data: UserRegistrationResponse = response.data;
        console.log(data)
        if (data.success) {
          this.messageService.add({
            severity: 'success',
            summary: 'Registration Successful',
            detail: `User '${data.username}' registered successfully!`,
          });
          this.router.navigate(['/products']);
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Registration Failed',
            detail: data.message,
          });
        }
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: error.message,
        });
      }
    );
  }
}
