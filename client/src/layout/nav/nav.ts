import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastService } from '../../core/services/toast-service';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.html',
  styleUrl: './nav.css'
})
export class Nav {
  protected accountService = inject(AccountService);
  private router = inject(Router);
  private toast = inject(ToastService);
  protected creds: any = {}

  login() {
    this.accountService.login(this.creds).subscribe({
      next: () => {
        this.router.navigateByUrl('/members');
        this.toast.success('Login successful');
        this.creds = {}; // Clear credentials after successful login
      },
      error: error => {
        // Situation 1: No email and No password
        if (!this.creds.email || !this.creds.password) {
          this.toast.error('Please enter your email and password.');
          return;
        }
        // Situation 2: Wrong email or password
        this.toast.error('Wrong email or password.');
      },
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
    console.log('Logout successful');
    this.toast.success('Logout successful');
  }
}
