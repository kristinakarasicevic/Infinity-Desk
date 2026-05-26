import { Routes } from '@angular/router';
import { Login } from './auth/login/login';
import { authGuard } from './core/auth-guard';
import { Shell } from './shared/shell/shell';

export const routes: Routes = [
  // kad korisnik dodje na root, preusmeri ga na /auth/login
  { path: '', redirectTo: '/auth/login', pathMatch: 'full' },

  // login stranica — javno dostupna, bez guarda
  { path: 'auth/login', component: Login },

  // sve zasticene rute idu KROZ Shell — Shell je "parent", njihov sadrzaj 
  // se renderuje u Shell-ovom <router-outlet>
  // canActivate na parent ruti stiti SVE child rute odjednom
  {
    path: '',
    component: Shell,
    canActivate: [authGuard],
    children: [

      { path: 'notes', component: Login },
    ]
  },

  // ako korisnik ukuca nepostojecu rutu, vrati ga na login
  { path: '**', redirectTo: '/auth/login' }
];