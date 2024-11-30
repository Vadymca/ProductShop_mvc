import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { ProductService } from '../services/product.service';
import { Product } from '../models/Product';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit, OnDestroy {
  products: Product[] = [];
  selectedProduct: Product = new Product();
  errorMessage: string = '';
  isLoading: boolean = true;
  private tokenSubscription: Subscription | null = null;

  constructor(private productService: ProductService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    // Subscribe to token changes and download products
    this.tokenSubscription = this.productService.tokenSubject.subscribe({
      next: (token) => {
        if (token) {
          this.loadProducts();
        }
      },
      error: (error) => {
        this.errorMessage = 'Ошибка при получении токена: ' + error.message;
      },
    });
  }

  ngOnDestroy(): void {
    // Unsubscribing from a token subject when a component is destroyed
    if (this.tokenSubscription) {
      this.tokenSubscription.unsubscribe();
    }
  }

  // Loading Products
  loadProducts(): void {
    this.isLoading = true;
  
    this.productService.getProducts().subscribe({
      next: (data: Product[]) => {
        console.log('Products loaded:', data);
        this.products = data; 
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.errorMessage = 'Ошибка при загрузке продуктов: ' + error.message;
        this.isLoading = false;
      },
    });
  }

  // Selecting a product to edit
  selectProduct(product: Product | null) {
    this.selectedProduct = product ? { ...product } : new Product();
  }

  // Reset product selection
  cancelEdit() {
    this.selectedProduct = new Product(); 
  }

  // Adding a new product
  addProduct() {
    if (this.selectedProduct) {
      this.productService.createProduct(this.selectedProduct).subscribe({
        next: (newProduct: Product) => {
          this.products.push(newProduct); 
          this.selectedProduct = new Product(); 
          this.errorMessage = '';
        },
        error: (error) => {
          this.errorMessage = 'Error adding product: ' + error.message;
        },
      });
    }
  }

  // Product Update
  updateProduct(): void {
    if (!this.selectedProduct || !this.selectedProduct.id) {
      this.errorMessage = 'Error: The selected product does not contain an ID.';
      return;
    }

    this.productService.updateProduct(this.selectedProduct.id, this.selectedProduct).subscribe({
      next: (updatedProduct: Product) => {
        const index = this.products.findIndex((p) => p.id === this.selectedProduct!.id);
        if (index !== -1) {
          this.products[index] = { ...updatedProduct }; 
        } else {
          console.warn('A product with this ID was not found in the local list.');
        }
        this.selectedProduct = new Product(); 
        this.errorMessage = '';
      },
      error: (error) => {
        this.errorMessage = 'Error when updating product: ' + error.message;
        console.error('Error:', error);
      },
    });
  }

  // Removing a product
  deleteProduct(id: number) {
    this.productService.deleteProduct(id).subscribe({
      next: () => {
        this.products = this.products.filter((p) => p.id !== id); 
        this.errorMessage = '';
      },
      error: (error) => {
        this.errorMessage = 'Error uninstalling product: ' + error.message;
      },
    });
  }
}
