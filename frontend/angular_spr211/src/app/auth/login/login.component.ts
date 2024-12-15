import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { User } from '../../models/User';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  user: User = new User();

  constructor(private authService: AuthService, private router: Router) {}

  login(): void {
    this.authService.getToken(this.user).subscribe({
      next: (response) => {
        localStorage.setItem('authToken', response.token);
        console.log('Login successful, token saved');
        this.router.navigate(['/home']); // Перенаправляємо на головну сторінку
      },
      error: (error) => {
        console.error('Login error:', error);
      }
    });
  }
}
