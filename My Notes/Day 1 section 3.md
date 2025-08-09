#links #learn
[[Day 1 section 2]]
# The next section
## Learning targets:
#dotnet
- [x] Using the dotnet CLI
- [x] API controllers and endpoints
- [x] Entity framework
- [x] The API Project structure
- [x] Configureation and Environment variables
- [x] Source control

---
#Angular
- [x] Using the Angular CLI
- [x] How to create a new Angular app
- [x] Angular project file
- [x] The Angilar bootstrap process
- [x] Using the Angular HTTP Client Service
- [x] Running an Angular app over HTTPS
- [x] How to add packages using NPM

Angular commands:
```c
npm install -g @angular/cli
```

```c
ng version
```

```c
ng new client
```

```c
ng serve
```

---

*go to 'linked' in setting(ctrl + ,) and turn on.*
*go to 'brackets' in setting, and change mode auto save ( Auto Closing Brackets)*

---
*adding HTTP in this path D:\Code\.Net\Code\DatingApp\client\src\app\app.config.ts*
``` ts
provideHttpClient()
```

*There is a recommeded method for new version*
```ts
import { HttpClient } from '@angular/common/http';
constructor(private http: HttpClient){}
```

``` ts
private http = inject(HttpClient);
```

``` ts
export class App implements OnInit {
  private http = inject(HttpClient);
  protected title = 'Dating App';
  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/members').subscribe(
      {
        next: (response) => console.log(response),
        error: (error) => console.error('Error fetching members:', error),
        complete: () => console.log('Completed the http request')
      }
    )
  }
}
```

*Add these code the have CORS for browser:
```cs
app.UseCors(policy =>

{

    policy.AllowAnyOrigin()

          .AllowAnyMethod()

          .AllowAnyHeader()

          .WithOrigins("http://localhost:4200", "https://localhost:4200");

});
```

*Ctrl C + dotnet watch or Ctrl R to update 

---

## app.ts and app.html

version 1:
```ts
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private http = inject(HttpClient);
  protected title = 'Dating App';
  protected members : any;

  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/members').subscribe(
      {
        next: (response) => this.members = response,
        error: (error) => console.error('Error fetching members:', error),
        complete: () => console.log('Completed the http request')
      }
    )
  }
}
```

```html
<h1>{{title}}</h1>
<ul>
    @for (member of members; track member.id) {
        <li>{{member.id}} - {{member.displayName}}</li>
    }
</ul>

```


Version 2:
```ts
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private http = inject(HttpClient);
  protected title = 'Dating App';
  protected members = signal<any[]>([]);

  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/members').subscribe(
      {
        next: (response) => this.members.set(response as any[]),
        error: (error) => console.error('Error fetching members:', error),
        complete: () => console.log('Completed the http request')
      }
    )
  }
}
```

```html
<h1>{{title}}</h1>

<ul>

    @for (member of members(); track member.id) {

        <li>{{member.id}} - {{member.displayName}}</li>

    }

</ul>
```

--> It have future application: a Zoneless. Reload browser and not lost the infor

---

Don't need subscribe, just promise
```ts
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private http = inject(HttpClient);
  protected title = 'Dating App';
  protected members = signal<any[]>([]);

  async ngOnInit() {
    this.members.set(await this.getMembers());
  }

  async getMembers() {
    try
    {
      return lastValueFrom(this.http.get<any[]>('https://localhost:5001/api/members'))
    } catch (error) {
      console.error('Error fetching members:', error);
      return [];
    }
  }
}
```
#tailwind #ts
[Tailwind CSS - Rapidly build modern websites without ever leaving your HTML.](https://tailwindcss.com/)
[daisyUI — Tailwind CSS Components ( version 5 update is here )](https://daisyui.com/)

Boostrap depends on Angular, when Angular have changes for updates, it may be haved bugs because third-side development don't cacth up angular, so tailwind is alternative method.

go to setting --> CSS › Lint: Unknown At Rules


---

[FiloSottile/mkcert: A simple zero-config tool to make locally trusted development certificates with any names you'd like.](https://github.com/FiloSottile/mkcert)
--> HTTPS
#mkcert
Powershell Admin:
```powershell
Set-ExecutionPolicy Bypass -Scope Process -Force; `
[System.Net.ServicePointManager]::SecurityProtocol = `
[System.Net.ServicePointManager]::SecurityProtocol -bor 3072; `
iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))

```

```cs
choco
```

```cs
choco install mkcert -y
```

```cs
mkcert -install
```

```cs
mkcert -version
```

```cs
cd .\client\
mkdir ssl
cd ssl
mkcert localhost
```

angular.json add serve:
```json
"options": {
            "ssl":true,
            "sslCert": "./ssl/localhost.pem",
            "sslKey": "./ssl/localhost-key.pem"
          },
```
