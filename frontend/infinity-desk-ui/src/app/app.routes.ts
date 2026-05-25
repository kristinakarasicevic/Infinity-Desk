import { Routes } from '@angular/router';
import { Login } from './auth/login/login';
import { authGuard } from './core/auth-guard';

export const routes: Routes = [
  // kad korisnik dodje na root, preusmeri ga na /auth/login
  { path: '', redirectTo: '/auth/login', pathMatch: 'full' },

  // login stranica — javno dostupna, bez guarda
  { path: 'auth/login', component: Login },

  // notes stranica — zasticena guardom (samo ulogovani korisnici)
  // privremeno Login kao placeholder
  { path: 'notes', component: Login, canActivate: [authGuard] },

  // ako korisnik ukuca nepostojecu rutu, vrati ga na login
  { path: '**', redirectTo: '/auth/login' }
];