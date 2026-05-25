import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth';

// cuvar ruta: proverava da li je korisnik ulogovan pre nego 
// sto dozvoli pristup stranici. 
// Ako nije ulogovan, preusmerava na login 
export const authGuard: CanActivateFn = (route, state) => {
  
  // inject() — moderniji nacin za dependency injection
  // koristi se umesto konstruktora jer ovo nije klasa, vec funkcija
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLoggedIn()) {
    return true; // korisnik ima token, dozvoljen pristup
  }

  // korisnik nije ulogovan, salji na login
  router.navigate(['/auth/login']);
  return false;
};