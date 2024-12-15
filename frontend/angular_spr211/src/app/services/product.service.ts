import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { Product } from '../models/Product';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private apiUrl = 'https://localhost:7193/api/apiproducts';
  tokenSubject = new Subject<string | null>();

  constructor(private http: HttpClient) {}

  // Отримання токена з localStorage
  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  // Метод для отримання продуктів
  getProducts(): Observable<Product[]> {
    const token = this.getToken();
    const headers = token
      ? new HttpHeaders({ Authorization: `Bearer ${token}` })
      : undefined;

    return this.http.get<Product[]>(this.apiUrl, { headers });
  }

  // Додати продукт
  createProduct(product: Product): Observable<Product> {
    const token = this.getToken();
    const headers = token
      ? new HttpHeaders({ Authorization: `Bearer ${token}` })
      : undefined;

    return this.http.post<Product>(this.apiUrl, product, { headers });
  }

  // Оновити продукт
  updateProduct(id: number, product: Product): Observable<Product> {
    const token = this.getToken();
    const headers = token
      ? new HttpHeaders({ Authorization: `Bearer ${token}` })
      : undefined;

    return this.http.put<Product>(`${this.apiUrl}/${id}`, product, { headers });
  }

  // Видалити продукт
  deleteProduct(id: number): Observable<void> {
    const token = this.getToken();
    const headers = token
      ? new HttpHeaders({ Authorization: `Bearer ${token}` })
      : undefined;

    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers });
  }
}
