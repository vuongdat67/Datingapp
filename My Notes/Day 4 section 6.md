#learn #links #Note
# Angular routing
Implement routing in Angular app and have an understanding of:
- [x] Angular routing
- [x] Adding toasts
- [x] Using Angualr route guards
- [x] App initialization
- [x] Route animations

---
--> Create members, lists and member-detailed
#Angular
``` js 
ng g c features/members/member-list --dry-run
ng g c features/members/member-list
```

```js
ng g c features/members/member-detailed --dry-run

ng g c features/members/member-detailed
```
`
```js
ng g c features/lists --dry-run

ng g c features/lists
```
---

## Using Ngclass for routing

```ts
[ngClass]="{'container mx-auto': !(router.url === '/')}">
```
but also different ways: #css #ts
```ts
[class.mt-24]="!(router.url === '/')"

    [class.container]="!(router.url === '/')"

    [class.mx-auto]="!(router.url === '/')"

>
```

---

## RouterLink and RouterLinkActive
```html
<nav class="flex gap-3 my-2 uppercase text-lg text-white">

            <a class="hover:text-amber-300" routerLink="/members" routerLinkActive="text-accent">Matches</a>

            <a class="hover:text-amber-300" routerLink="/Lists" routerLinkActive="text-accent">Lists</a>

            <a class="hover:text-amber-300" routerLink="/Messages" routerLinkActive="text-accent">Messages</a>

        </nav>
```

--> Using routerLink to link routers and record the state active of links

```ts
<a routerLink="/" routerLinkActive="text-accent" [routerLinkActiveOptions]="{ exact: true }"
```

---

### Using  Router for Login and Logout to forward links:
```ts
private router = inject(Router);
login() {
    this.accountService.login(this.creds).subscribe({
      next: result => {
        this.router.navigateByUrl('/members');
        console.log('Login successful', result);
        this.creds = {}; // Clear credentials after successful login
      },
      error: error => alert('Login failed: ' + error.message)
    });
  }
  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
    console.log('Logout successful');
  }
```

---
#daisyui
### Toaster
```ts
ngx toastr
```
#links:
[Angular Toastr](https://ngx-toastr.vercel.app/)
--> But this web hasn't caught to update Angular v20, so we use DaisyUI
[Tailwind Toast Component — Tailwind CSS Components ( version 5 update is here )](https://daisyui.com/components/toast/)

**Angular command**:
```cs
ng g s toast-service --dry-run
```
#js #ts
```ts
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  constructor() {
    this.createToastContainer();
  }
  private createToastContainer(){
    if(!document.getElementById('toast-container'))
    {
      const container = document.createElement('div');
      container.id = 'toast-container';
      container.className = 'toast toast-bottom toast-end';
      // container.style.position = 'fixed';
      // container.style.top = '1rem';
      // container.style.right = '1rem';
      // container.style.zIndex = '9999';
      document.body.appendChild(container);
    }
  }
  private createToastElement(message: string, alertClass: string, duration = 5000)
  {
    const toastContainer = document.getElementById('toast-container');
    if(!toastContainer) return;

    const toast = document.createElement('div');
    toast.classList.add('alert', alertClass, 'shadow-lg');
    toast.innerHTML = `
      <span>${message}</span>
      <button class="ml-4 btn btn-sm btn-ghost">x</button>
    `;

    toast.querySelector('button')?.addEventListener('click', () => {
      // toastContainer.removeChild(toast);
      toast.remove();
    });
    toastContainer.appendChild(toast);
    setTimeout(() => {
      if (toast.parentNode) {
        toast.parentNode.removeChild(toast);
      }
    }, duration);
  }
  success(message: string, duration?: number) {
    this.createToastElement(message, 'alert-success', duration);
  }
  error(message: string, duration?: number) {
    this.createToastElement(message, 'alert-error', duration);
  }
  warning(message: string, duration?: number) {
    this.createToastElement(message, 'alert-warning', duration);
  }
  info(message: string, duration?: number) {
    this.createToastElement(message, 'alert-info', duration);
  }
}

```

---

### *Route Guard*
```ts
ng g g auth --dry-run
✔ Which type of guard would you like to create? CanActivate
```

---

### *Setting compact* : Ctrl + , --> compact --> unable


```ts
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../services/account-service';
import { inject } from '@angular/core/primitives/di';
import { ToastService } from '../services/toast-service';

export const authGuard: CanActivateFn = () => {
  const accountService = inject(AccountService);
  const toast = inject(ToastService)

  if (accountService.currentUser()) return true;

  toast.error('You shall not pass!');
  return false;
};


```

---

### **Adding duming route**
```ts
@if (accountService.currentUser()) {
	<a class="hover:text-amber-300" routerLink="/members" routerLinkActive="text-accent">Matches</a>
	<a class="hover:text-amber-300" routerLink="/lists" routerLinkActive="text-accent">Lists</a>
	<a class="hover:text-amber-300" routerLink="/messages" routerLinkActive="text-accent">Messages</a>
            }
```

---

```ts
ng g s init-service
```

*copy init from app.ts*
```ts
	 const userString = localStorage.getItem('user');
     if(!userString) return;
     const user = JSON.parse(userString);
     this.accountService.currentUser.set(user);
```

---

END