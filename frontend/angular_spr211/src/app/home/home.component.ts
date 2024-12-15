import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { ProductService } from '../services/product.service';
import { Product } from '../models/Product';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CartService } from '../services/cart.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule, CommonModule,],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  products: Product[] = []; // Список продуктів
  selectedProduct: Product | null = null; // Вибраний продукт для редагування
  newProduct: Product = { id: 0, name: '', price: 0, description: '' }; // Продукт для додавання
  isLoading: boolean = false; // Стан завантаження
  errorMessage: string = ''; // Повідомлення про помилки

  constructor(private productService: ProductService, private cartService: CartService) {}

  ngOnInit(): void {
    this.loadProducts(); // Завантаження продуктів при ініціалізації
  }

  // Завантаження продуктів
  loadProducts(): void {
    this.isLoading = true;
    this.productService.getProducts().subscribe({
      next: (data: Product[]) => {
        this.products = data;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Error loading products: ' + error.message;
        this.isLoading = false;
      },
    });
  }

  // Вибір продукту для редагування
  selectProduct(product: Product | null): void {
    this.selectedProduct = product ? { ...product } : { id: 0, name: '', price: 0 , description: ''}; // Якщо продукт null, створюємо новий
  }

  // Скасування редагування
  cancelEdit(): void {
    this.selectedProduct = null;
  }

  // Додавання нового продукту
  addProduct(): void {
    if (
      !this.newProduct.name || 
      this.newProduct.name.trim() === '' || 
      this.newProduct.price <= 0
    ) {
      this.errorMessage = 'Please fill in all fields before adding a product.';
      return;
    }

    this.productService.createProduct(this.newProduct).subscribe({
      next: (createdProduct: Product) => {
        this.products.push(createdProduct);
        this.newProduct = { id: 0, name: '', price: 0, description: '' }; // Очищення форми
        this.errorMessage = ''; // Скидання повідомлення про помилку
      },
      error: (error) => {
        this.errorMessage = 'Error adding product: ' + error.message;
      },
    });
  }

  // Оновлення продукту
  updateProduct(): void {
    if (this.selectedProduct && this.selectedProduct.id) {
      this.productService.updateProduct(this.selectedProduct.id, this.selectedProduct).subscribe({
        next: (updatedProduct: Product) => {
          const index = this.products.findIndex((p) => p.id === updatedProduct.id);
          if (index !== -1) {
            this.products[index] = updatedProduct;
          }
          this.selectedProduct = null;
        },
        error: (error) => {
          this.errorMessage = 'An error occurred while updating the product: ' + error.message;
        },
      });
    }
  }

  // Видалення продукту
  deleteProduct(id: number): void {
    this.productService.deleteProduct(id).subscribe({
      next: () => {
        this.products = this.products.filter((product) => product.id !== id);
      },
      error: (error) => {
        this.errorMessage = 'Error removing product: ' + error.message;
      },
    });
  }
  // Добавлення продукту в кошик
  addToCart(productId: number) {
    // Показуємо користувачу вікно для введення кількості товару
    const quantityInput = prompt('Enter the quantity of the product you want to add to the cart:', '1');
    
    // Перетворюємо введене значення на число
    let quantity = parseInt(quantityInput || '1', 10);
  
    // Якщо введено некоректне значення або кількість менша за 1, використовуємо 1 за замовчуванням
    if (isNaN(quantity) || quantity <= 0) {
      quantity = 1;
    }
  
    // Створюємо об'єкт товару
    const cartItem = { productId, quantity };
  
    // Відправляємо запит на сервер
    this.cartService.addToCart(cartItem).subscribe({
      next: (response) => {
        alert(response.Message || 'Product added to cart successfully!');
        console.log('Cart updated:', response);
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = err.error?.Message || 'Failed to add product to cart.';
      },
    });
  }
  
}
