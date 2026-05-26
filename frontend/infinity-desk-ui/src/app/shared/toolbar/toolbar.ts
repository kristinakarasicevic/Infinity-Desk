import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { AuthService } from '../../core/auth';

@Component({
  selector: 'app-toolbar',
  imports: [
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule
  ],
  templateUrl: './toolbar.html',
  styleUrl: './toolbar.scss'
})
export class Toolbar {
  // konstruktor injektuje AuthService da bi mogao da se pozove logout
  // public znaci da je dostupan u template-u (HTML-u)
  constructor(public authService: AuthService) {}

  // poziva logout iz AuthService-a — brise token i redirektuje na login
  onLogout(): void {
    this.authService.logout();
  }
}