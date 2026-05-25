import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap, BehaviorSubject } from 'rxjs';
import { environment } from '../../environments/environment';
import { LoginRequest, LoginResponse } from '../models/user.model';


// @Injectable dekorator govori Angularu da ova klasa moze biti injektovana u druge klase
// providedIn: 'root' znaci da postoji samo jedna instanca u celoj aplikaciji
// (Singleton pattern — isti AuthService koriste sve komponente)
@Injectable({
  providedIn: 'root',
})
export class AuthService {

  //adresa bekenda
  private apiUrl = `${environment.apiUrl}/Auth`;

  // Kljuc pod kojim cuvamo token u localStorage
  private readonly TOKEN_KEY = 'auth_token';

  // BehaviorSubject drzi trenutno stanje (ulogovan/nije) i emituje promene
  // Komponente (npr. navbar) mogu da se "pretplate" preko isLoggedIn$
  // i automatski reaguju kad se korisnik uloguje/izloguje
  private isLoggedInSubject = new BehaviorSubject<boolean>(this.hasToken());
  public isLoggedIn$ = this.isLoggedInSubject.asObservable();

  // Constructor injection — Angular automatski kreira i ubacuje ove objekte
  // ne mora da se pise new HttpClient(), nego ce ang to sam
  // HttpClient — Angularov servis za slanje HTTP zahteva (GET, POST, PUT, DELETE)
  // Router — Angularov servis za navigaciju izmedju stranica + ne reloaduje celu stranicu (SPA)  
  constructor(private http: HttpClient, private router: Router) { }


  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials)
      .pipe(
        tap(response => { //tap() = RxJS operator koji radi snimanje tokena u localStorage i obavestava pretplatnike da se stanje promenilo (korisnik je ulogovan)
          localStorage.setItem(this.TOKEN_KEY, response.token);
          this.isLoggedInSubject.next(true);
        })
      );
  }

  // Brise token i vraca korisnika na login stranicu
  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    this.isLoggedInSubject.next(false);
    this.router.navigate(['/auth/login']);
  }

  // Vraca token (koristice ga interceptor da ga doda u svaki HTTP zahtev)
  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  // provera da li je korisnik ulogovan (koristi je AuthGuard)
  isLoggedIn(): boolean {
    return this.hasToken();
  }

  private hasToken(): boolean {
    return !!localStorage.getItem(this.TOKEN_KEY);
  }
}
