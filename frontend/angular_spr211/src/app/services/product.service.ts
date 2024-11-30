import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { switchMap, catchError } from 'rxjs/operators';
import { Product } from '../models/Product';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  public tokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);
  private url: string = 'https://localhost:7193/api/apiproducts';

  constructor(private http: HttpClient, private authService: AuthService) {
    this.initToken();
  }

  // Token initialization
  private initToken(): void {
    const user = { email: 'vadimancuta@gmail.com', password: '111111' };
    this.authService.getToken(user).pipe(
      catchError((error) => {
        console.error('Error receiving token:', error);
        return of(null);  
      })
    ).subscribe({
      next: (data: { token: string } | null) => {
        if (data?.token) {
          this.tokenSubject.next(data.token);  
        } else {
          console.error('Token was not received');
        }
      },
    });
  }

  // Retrieving headers with authorization
  private getHeaders(): Observable<HttpHeaders> {
    return this.tokenSubject.pipe(
      switchMap((token) => {
        if (!token) {
          throw new Error('Token not available');
        }
        return of(new HttpHeaders({ Authorization: `Bearer ${token}` }));
      })
    );
  }

  // Get products with token 
  getProducts(): Observable<Product[]> {
    return this.tokenSubject.pipe(
      switchMap((token) => {
        if (!token) {
        
          return of([]);  
        }
        return this.getHeaders().pipe(
          switchMap((headers) => {
            return this.http.get<Product[]>(this.url, { headers }).pipe(
              catchError((error) => {
                console.error('Error loading products:', error);
                return of([]);  
              })
            );
          })
        );
      })
    );
  }

  // Create
  createProduct(product: Product): Observable<any> {
    return this.getHeaders().pipe(
      switchMap((headers) => {
        return this.http.post(this.url, product, { headers }).pipe(
          catchError((error) => {
            console.error('Error creating product:', error);
            return of(null);  
          })
        );
      })
    );
  }

  // Update
  updateProduct(id: number, product: Product): Observable<any> {
    return this.getHeaders().pipe(
      switchMap((headers) => {
        return this.http.put(`${this.url}/${id}`, product, { headers }).pipe(
          catchError((error) => {
            console.error('Error while updating product:', error);
            return of(null);  
          })
        );
      })
    );
  }

  // Delete
  deleteProduct(id: number): Observable<any> {
    return this.getHeaders().pipe(
      switchMap((headers) => {
        return this.http.delete(`${this.url}/${id}`, { headers }).pipe(
          catchError((error) => {
            console.error('Error uninstalling product:', error);
            return of(null);  
          })
        );
      })
    );
  }
}
