import { Component, OnInit } from '@angular/core';
import { CartService } from '../services/cart.service';
import { CartItem } from '../models/CartItem';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];
  totalPrice: number = 0;
  errorMessage: string = '';

  constructor(private cartService: CartService) {}

  ngOnInit(): void {
      this.loadCartItems();
  }

  loadCartItems(): void {
    this.cartService.getCartItems().subscribe({
      next: (items) => {
        this.cartItems = items;
        this.calculateTotalPrice();
      },
      error: (err) => {
        this.errorMessage = 'Failed to load cart items: ' + err.message;
      },
    });
  }

  calculateTotalPrice(): void {
    this.totalPrice = this.cartItems.reduce((sum, item) => sum + item.totalPrice, 0);
  }

  removeItem(cartItemId: number): void {
    this.cartService.removeFromCart(cartItemId).subscribe({
      next: () => {
        this.cartItems = this.cartItems.filter((item) => item.id !== cartItemId);
        this.calculateTotalPrice();
      },
      error: (err) => {
        this.errorMessage = 'An error occurred when removing the product: ' + err.message;
      },
    });
  }

  clearCart(): void {
    const itemsToRemove = [...this.cartItems];
    for (const item of itemsToRemove) {
      this.removeItem(item.id);
    }
  }

  createOrder(): void {
    this.cartService.createOrder().subscribe({
      next: (order) => {
        alert(`Order №${order.id} created! Total amount: ${order.totalPrice} USD`);
        this.cartItems = [];
        this.totalPrice = 0;
      },
      error: (err) => {
        this.errorMessage = 'An error occurred when creating the order: ' + err.message;
      },
    });
  }
}
