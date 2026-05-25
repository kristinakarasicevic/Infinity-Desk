import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../core/auth';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  loginForm!: FormGroup;
  errorMessage = ''; //poruka greske koja se prik ako login ne uspe
  isLoading = false; //koristi se za disable dugmeta dok traje login

  constructor(
    private fb: FormBuilder, //helper za pravljenje formi
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    // fb.group({}) kreira FormGroup sa kontrolama
    // svaka kontrola je niz: [pocetna_vrednost, [validatori]]
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  //metoda koja se poziva kada se submituje forma(klikne login dugme)
  onSubmit(): void {

    if (this.loginForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    // loginForm.value automatski vraca { username: '...', password: '...' }
    this.authService.login(this.loginForm.value).subscribe({
      // next se izvrsava kad zahtev uspe
      next: () => {
        this.snackBar.open('Uspesno ste se prijavili! 🎉', 'OK', {
          duration: 3000,
          verticalPosition: 'top'
        });
        this.router.navigate(['/notes']); // posle uspesnog logina idi na notes
      },
      // error se izvrsava kad zahtev pukne 
      error: (err) => {
        this.errorMessage = 'Pogresno korisnicko ime ili lozinka';
        this.isLoading = false;
      }
    });
  }

}
