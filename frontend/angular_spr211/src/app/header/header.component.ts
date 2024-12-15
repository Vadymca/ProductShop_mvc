import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, FormsModule, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit {
  isAuthenticated: boolean = false;
  constructor(private authService: AuthService) {}
  ngOnInit(): void {
    this.isAuthenticated = this.authService.isLoggedIn(); // Перевіряємо стан авторизації
  }
  // Виклик методу logout
  onLogout(): void {
    this.authService.logout(); // Видаляємо токен і переходимо до логіну
  }
}
