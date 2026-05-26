import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { Toolbar } from '../toolbar/toolbar';

// Shell je "ram" aplikacije — gornji toolbar + prostor ispod gde se 
// renderuju razlicite stranice (notes, tasks, settings...)
// Ovo je layout komponenta koja se ne menja, samo se sadrzaj ispod menja
@Component({
  selector: 'app-shell',
  imports: [
    CommonModule,
    RouterOutlet,
    Toolbar
  ],
  templateUrl: './shell.html',
  styleUrl: './shell.scss'
})

//ne radi nista, samo prikazuje layout
export class Shell {}