#summary
## Section 6: [[Day 4 section 6]]
- [x] Angular routing
- [x] Adding toasts
- [x] Using Angular route guards
- [x] App initialization
- [x] Route animations

---
#learn #Note #links
## Section 7: Error handing
## Learning goals:
- [x] API middleware
- [x] Angular interceptors
- [x] Troubleshooting exceptions
### Creating error controller for testing

*Using class in controller:*
```cs
public class BuggyController : BaseApiController
{
    [HttpGet("auth")]
    public IActionResult GetAuth()
    {
        return Unauthorized();
    }
    [HttpPatch("not-found")]
    public IActionResult GetNotFound()
    {
        return NotFound();
    }
    [HttpGet("server-error")]
    public IActionResult GetServerError()
    {
        throw new Exception("This is a server error");
    }
    [HttpGet("bad-request")]
    public IActionResult GetBadRequest()
    {
        return BadRequest("This is a bad request");
    }
}
```

---

Then, Tesing API with Postman

---

this doesn't recognize and these code also need to have configuration in Program.cs
, although this line doesn't appear, but it may be relevant in diiferent file
```cs
app.UseDeveloperExceptionPage();
```

try changing mode "Developer" to see error "Production"

---
[ASP.NET Core Middleware | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-9.0)

### Using ILogger to write console log
### Middleware and Statecode
```cs
namespace API.Errors;
public class ApiException(int StatusCode, string Message, string? Details)
{
    public int StatusCode { get; set; } = StatusCode;
    public string? Message { get; set; } = Message;
    public string? Details { get; set; } = Details;
}

```

```cs
namespace API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var respone = env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace)
                : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var json = JsonSerializer.Serialize(respone, options);
            await context.Response.WriteAsync(json);
        }
    }
}
```

Program.cs
```cs
app.UseMiddleware<ExceptionMiddleware>();
```

---

### Creates component for tesing errors
```cs
ng g c features/test-errors
```

```ts
import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  imports: [],
  templateUrl: './test-errors.html',
  styleUrl: './test-errors.css'
})
export class TestErrors {
  private http = inject(HttpClient);
  baseUrl = 'https://localhost:5001/api/';
  
  get404Error() {
    return this.http.get(`${this.baseUrl}buggy/not-found`).subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }
  get400Error() {
    return this.http.get(`${this.baseUrl}buggy/bad-request`).subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }
  get500Error() {
    return this.http.get(`${this.baseUrl}buggy/server-error`).subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }
  get401Error() {
    return this.http.get(`${this.baseUrl}buggy/auth`).subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }
  get400ValidationError() {
    return this.http.post(`${this.baseUrl}account/register`, {}).subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }
}

```

---
### Components testing errors
```ts
<div class="flex justify-center gap-3 pt-10">
    <button class="btn btn-neutral" (click)="get500Error()">Get 500 Error</button>
    <button class="btn btn-accent" (click)="get400Error()">Get 400 Error</button>
    <button class="btn btn-secondary" (click)="get401Error()">Get 401 Error</button>
    <button class="btn btn-success" (click)="get404Error()">Get 404 Error</button>
    <button class="btn btn-info" (click)="get400ValidationError()">Get 400 Validation Error</button>
</div>
```


--> ***adding in app.routes and adding nav bar components***

---

### Interceptor error with angular

```cs
ng g interceptor error
```

```json
"@schematics/angular:interceptor": {
          "skipTests": true,
          "path": "src/core/interceptors"
        }
```

```ts
import { HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { ToastService } from '../services/toast-service';
import { Router } from '@angular/router';
import { inject } from '@angular/core/primitives/di';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toast = inject(ToastService);
  const router = inject(Router);
  return next(req).pipe(
    catchError((error) => {
      if(error)
      {
        switch(error.status) {
          case 400:
            toast.error(error.error);
            break;
          case 401:
            toast.error('Unauthorized');
            break;
          case 404:
            toast.error('Not Found');
            break;
          case 500:
            toast.error('Server Error');
            break;
          default:
            toast.error('Something went wrong');
            break;
        }
      }
        // return throwError(() => error);
        throw error;
    })
  );
};

```

*app.config.ts*
```ts
    provideHttpClient(withInterceptors([errorInterceptor])),
```

```html
@if (validationErrors().length > 0) {
    <div class="card bg-base-100 flex mt-5 rounded-2xl w-1/2 p-3 mx-auto">
      <ul class="flex flex-col text-red-600 space-y-2">
        @for (error of validationErrors(); track $index) {
          <li>{{error}}</li>
        }
      </ul>
    </div>
 }
```

```cs
ng g c shared/errors/not-found
ng g c shared/errors/server-erro
```

---

Adding not-found page
```html
<div class="card bg-base-100 gap-3 w-4xl mx-auto flex flex-col items-center rounded-xl shadow-xl p-10">
    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor"
        class="size-32 text-error">
        <path stroke-linecap="round" stroke-linejoin="round"
            d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z" />
    </svg>
    <h1 class="card-title text-4xl justify-center">Not found</h1>
    <p class="card-body text-xl text-center">Sory, what you are looking for cannot be found</p>
    <button class="btn btn-primary" (click)="goBack()">Go back</button>
</div>
```


```ts
{path: '**', component: NotFound},
```

```ts
import { Component, inject } from '@angular/core';
import { Location } from '@angular/common';
@Component({
  selector: 'app-not-found',
  imports: [],
  templateUrl: './not-found.html',
  styleUrl: './not-found.css'
})
export class NotFound {
  private location = inject(Location);
  goBack() {
    this.location.back();
  }
}

```

---
Adding server-error page

### Component
#Angular #ts
```ts
import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ApiError } from '../../../types/error';

@Component({
  selector: 'app-server-error',
  imports: [],
  templateUrl: './server-error.html',
  styleUrl: './server-error.css'
})
export class ServerError {
  protected error : ApiError;
  private router = inject(Router);
  protected showDetails = false;

  constructor() {
    const navigation = this.router.getCurrentNavigation();
    this.error = navigation?.extras?.state?.['error'];
  }

  detailsToggle() {
    this.showDetails = !this.showDetails;
  }
}

```

```html
<div class="card bg-base-100 gap-3 w-4xl mx-auto flex flex-col items-center rounded-xl shadow-xl p-10">
    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-32 text-error">
  <path stroke-linecap="round" stroke-linejoin="round" d="M12 12.75c1.148 0 2.278.08 3.383.237 1.037.146 1.866.966 1.866 2.013 0 3.728-2.35 6.75-5.25 6.75S6.75 18.728 6.75 15c0-1.046.83-1.867 1.866-2.013A24.204 24.204 0 0 1 12 12.75Zm0 0c2.883 0 5.647.508 8.207 1.44a23.91 23.91 0 0 1-1.152 6.06M12 12.75c-2.883 0-5.647.508-8.208 1.44.125 2.104.52 4.136 1.153 6.06M12 12.75a2.25 2.25 0 0 0 2.248-2.354M12 12.75a2.25 2.25 0 0 1-2.248-2.354M12 8.25c.995 0 1.971-.08 2.922-.236.403-.066.74-.358.795-.762a3.778 3.778 0 0 0-.399-2.25M12 8.25c-.995 0-1.97-.08-2.922-.236-.402-.066-.74-.358-.795-.762a3.734 3.734 0 0 1 .4-2.253M12 8.25a2.25 2.25 0 0 0-2.248 2.146M12 8.25a2.25 2.25 0 0 1 2.248 2.146M8.683 5a6.032 6.032 0 0 1-1.155-1.002c.07-.63.27-1.222.574-1.747m.581 2.749A3.75 3.75 0 0 1 15.318 5m0 0c.427-.283.815-.62 1.155-.999a4.471 4.471 0 0 0-.575-1.752M4.921 6a24.048 24.048 0 0 0-.392 3.314c1.668.546 3.416.914 5.223 1.082M19.08 6c.205 1.08.337 2.187.392 3.314a23.882 23.882 0 0 1-5.223 1.082" />
</svg>

    <h1 class="card-title text-4xl justify-center">Server error</h1>
    <p class="card-body text-xl text-center">{{error.message}}</p>
    <button class="btn btn-primary" (click)="detailsToggle()">Details</button>
    @if (showDetails) {
      <div>{{error.details}}</div>
    }
</div>
```

app.route.ts
```ts
{path: 'server-error', component: ServerError},
```

---
## Devtools
[DevTools • Overview • Angular](https://angular.dev/tools/devtools)

