import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http'; 
import { User } from '../models/User';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private url: string = `https://localhost:7193/api/apiauth/login`;
  constructor(private http: HttpClient) { }
  getToken(user: User): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(this.url, user);
  }
  
}
