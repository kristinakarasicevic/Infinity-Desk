import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Note } from '../models/note.model';

@Injectable({
  providedIn: 'root',
})

export class NotesService {
  private apiUrl = `${environment.apiUrl}/Notes`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Note[]> {
    return this.http.get<Note[]>(this.apiUrl);
  }

  getById(id: number): Observable<Note> {
    return this.http.get<Note>(`${this.apiUrl}/${id}`);
  }

  // Partial<Note> znaci da saljemo samo neka polja (title, content) 
  // jer id i userId postavlja backend
  create(note: Partial<Note>): Observable<Note> {
    return this.http.post<Note>(this.apiUrl, note);
  }

  update(id: number, note: Partial<Note>): Observable<Note> {
    return this.http.put<Note>(`${this.apiUrl}/${id}`, note);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}