import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from './auth';

// presrece svaki HTTP zahtev pre nego sto ode na backend 
// i automatski dodaje JWT token u header. 
// Bez ovoga bi token morao rucno da se dodaje u svaki http zahtev
export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  // ako imamo token, kloniramo zahtev i dodajemo Authorization header
  // HttpRequest je immutable (ne moze se menjati), zato pravimo kopiju
  if (token) {
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(cloned);
  }

  // ako nema tokena (npr. sam login zahtev), salji originalni zahtev
  return next(req);
};