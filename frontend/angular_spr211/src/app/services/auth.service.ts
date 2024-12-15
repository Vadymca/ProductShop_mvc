import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from '../models/User';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl: string = `https://localhost:7193/api/apiauth`;

  constructor(private http: HttpClient, private router: Router) {}

  // Метод для отримання ID користувача з токена або локального сховища
  getUserId(): number | null {
    const token = this.getAuthToken(); // Отримуємо токен з localStorage
    if (!token) {
      return null; // Якщо токен відсутній
    }
  
    const payload = JSON.parse(atob(token.split('.')[1])); // Декодування payload із JWT
    return payload?.id || null; // Повертаємо ID користувача
  }

  // Отримання токена при вході
  getToken(user: User): Observable<{ token: string }> {
    const loginUrl = `${this.baseUrl}/login`;
    return this.http.post<{ token: string }>(loginUrl, user);
  }

  // Реєстрація нового користувача
  register(user: User): Observable<any> {
    const registerUrl = `${this.baseUrl}/register`;
    return this.http.post<any>(registerUrl, user);
  }

  // Перевірка, чи користувач авторизований
  isLoggedIn(): boolean {
    const token = localStorage.getItem('authToken');
    return !!token; // Якщо токен існує, повертаємо true
  }

  // Отримання токена з LocalStorage
  getAuthToken(): string | null {
    return localStorage.getItem('authToken');
  }

  // Логаут (видалення токена)
  logout(): void {
    localStorage.removeItem('authToken');// Видаляємо токен
    this.router.navigate(['/login']); // Переходимо на сторінку логіну
  }
}
