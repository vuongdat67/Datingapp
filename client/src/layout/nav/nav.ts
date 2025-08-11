import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastService } from '../../core/services/toast-service';
import { themes } from '../theme';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.html',
  styleUrl: './nav.css'
})
export class Nav implements OnInit {
  protected accountService = inject(AccountService);
  private router = inject(Router);
  private toast = inject(ToastService);
  protected creds: any = {}
  protected selectedTheme = signal<string>(localStorage.getItem('theme') || 'light');
  protected themes = themes;

  ngOnInit(): void {
    document.documentElement.setAttribute('data-theme', this.selectedTheme());
  }

  handleSelectTheme(theme: string){
    this.selectedTheme.set(theme);
    localStorage.setItem('theme', theme);
    document.documentElement.setAttribute('data-theme', theme);
    const element= document.activeElement as HTMLElement;
    if(element) {
      element.blur(); // Remove focus from the button
    }
  }

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
