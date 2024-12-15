import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CartItem } from '../models/CartItem';
import { Order } from '../models/Order';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private baseUrl: string = `https://localhost:7193/api`;
  constructor(private http: HttpClient) { }

  // Отримання токена з localStorage
  getToken(): string | null {
    return localStorage.getItem('authToken');
  }
  
// Додати до кошика
addToCart(cartItem: { productId: number; quantity: number }): Observable<any> {

  const token = this.getToken();
      const headers = token
        ? new HttpHeaders({ Authorization: `Bearer ${token}` })
        : undefined;
  return this.http.post<any>(`${this.baseUrl}/cart/add`, cartItem, { headers });
}

getCartItems(): Observable<CartItem[]> {
  const token = this.getToken();
      const headers = token
        ? new HttpHeaders({ Authorization: `Bearer ${token}` })
        : undefined;
  return this.http.get<CartItem[]>(`${this.baseUrl}/cart`, { headers });
}

  removeFromCart(cartItemId: number): Observable<any> {const token = this.getToken();
      const headers = token
        ? new HttpHeaders({ Authorization: `Bearer ${token}` })
        : undefined;
    return this.http.delete<any>(`${this.baseUrl}/cart/${cartItemId}`, { headers });
  }

  createOrder(): Observable<Order> {
    const token = this.getToken();
    const headers = token
      ? new HttpHeaders({ Authorization: `Bearer ${token}` })
      : undefined;
    return this.http.post<Order>(`${this.baseUrl}/order/create`, {});
  }
}
