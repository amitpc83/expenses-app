import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-signup',
  imports: [ReactiveFormsModule, RouterLink, CommonModule],
  templateUrl: './signup.html',
  styleUrl: './signup.css',
})
export class Signup {
  signupForm: FormGroup;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
  ) {
    this.signupForm = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
      },
      {
        validators: this.passwordMatchValidator,
      },
    );
  }
  hasError(controlName: string, errorName: string) {
    const control = this.signupForm.get(controlName);
    return ((control?.touched || control?.dirty) && control.hasError(errorName)) || false;
  }
  passwordMatchValidator(fg: FormGroup) {
    const password = fg.get('password')?.value;
    const confirmPassword = fg.get('confirmPassword')?.value;

    return password === confirmPassword ? null : { passwordMismatch: true };
  }

  onSubmit() {
    if (this.signupForm.valid) {
      const formValues = this.signupForm.value;

      const credentials = {
        email: formValues.email,
        password: formValues.password,
      };

      this.authService.register(credentials).subscribe({
        next: () => {
          this.router.navigate(['/transactions']);
        },
        error: (error) => {
          console.log('Error -' + error);
          this.errorMessage =
            error.error?.message || 'An error occured during signup. Please, try again.';
        },
      });
    }
  }
}
