# Learning Goals
- [x] How to store passwords in the database
- [x] Using inheritance in C# - DRY
- [x] Using the C# debugger
- [x] Using Data Transfer Objects (DTOs)
- [x] Validation
- [x] JSON Web Tokens (JWTs)
- [x] Using services in C#
- [x] Middleware
- [x] Extension methods - DRY

---
## Storing passwords in the database
- [ ] Option 1 - Storing in clear text --> raw text --> No
- [ ] Option 2 - Hashing the password --> Dictionary password, same password in different accounts --> No
- [x] Option 3 - Hasing and salting the password --> Scramble hash password despite of same password in different accounts --> Yes --> 90%

---

APS.NET Identity

---

```
dotnet ef migrations add UserEntityUpdated

dotnet ef database update
```

---

# Create file controller api
D:\Code\.Net\Code\DatingApp\API\Controllers\BaseApiController.cs
```cs
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
```

remove controller parent and replate BaseApiController:
```cs
[Route("api/[controller]")] // localhost:5001/api/members
    [ApiController]

```

---

*Register API using & to connect string parameter (&)*
```csharp
public class AccountController(AppDbContext context) : BaseApiController
{
    [HttpPost("register")] // api/account/register
    public async Task<ActionResult<AppUser>> Register(string email, string displayName, string password)
    {
        var hmac = new HMACSHA512();
        var user = new AppUser
        {
            Email = email,
            DisplayName = displayName,
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }
}
```
--> Test API 
```js
{{url}}/api/account/register?email=sam@test.com&password=password&displayName=Sam
```
DTO - Data transfer objects
```cs
public class AccountController(AppDbContext context) : BaseApiController
{
    [HttpPost("register")] // api/account/register
    public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
    {
        var hmac = new HMACSHA512();
        var user = new AppUser
        {
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName,
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }
}

```

 --> Test API
```js
{{url}}/api/account/register
```

---

```
using System.ComponentModel.DataAnnotations;
```
---

*drop database:*
```
dotnet ef database drop
```

```
dotnet ef database update
```

---

*Login*
```cs
[HttpPost("login")] // api/account/login
    public async Task<ActionResult<AppUser>> Login([FromBody] LoginDto loginDto)
    {
        var user = await context.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Email);
        if (user == null) return Unauthorized("Invalid email address");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginDto.Password));
        //if (!computedHash.SequenceEqual(user.PasswordHash)) return Unauthorized("Invalid password");
        // for (int i = 0; i < computedHash.Length; i++)
        // {
        //     if (computedHash[i] != user.PasswordHash[i])
        //     {
        //         return Unauthorized("Invalid password");
        //     }
        // }
        int result = 0;
        for (int i = 0; i < computedHash.Length; i++)
        {
            result |= computedHash[i] ^ user.PasswordHash[i];
        }
        if (result != 0) return Unauthorized("Invalid password");
        return user;
    }

```

- Những cách comment thì không an toàn bởi timing attack
---

## JWT - Token authentication
single request --> industry standard for token:
- credentials
- claims
- other information

3 parts: header, payload, verify signature
--> Json web token

MFF, expired token

send username
![[Drawing 2025-07-31 11.14.04.excalidraw]]

No seesion to manage - JWT are self contained tokens
Portable - a single token can be used with multiple backends
No cookies required - mobile friendly
Performance - Once a token is ussed, there is no need to make a database request to verify a users authentication

---

Using ITokenService and TokenService have the suggested fix 
- Show code actions 
- ctrl + .
---
Adding Service in Program.cs

AddSingleton: starting and running service, login, a token

AddTransient: new version of service, short, compare to AddScoped


---
```cs
System.IdentityModel.Tokens.Jwt @Microsoft
Microsoft.IdentityModel.Tokens @Microsoft
```

SymmetricSecurityKey: encrypt and decrypt like ssl, public and private key

---
## 🧠 Mục đích chính

Lớp `TokenService` có nhiệm vụ:

- Nhận một `AppUser` (thường là user sau khi đăng nhập thành công)
    
- Tạo ra **JWT Token** có chứa thông tin người dùng (claims)
    
- Token này dùng để **xác thực (authentication)** ở client và các API khác
    

---

## 📦 Giải thích chi tiết

`public class TokenService(IConfiguration configuration) : ITokenService`

Đây là cách viết **constructor injection** rút gọn của C# 12. Lưu `IConfiguration` để lấy token key và cấu hình từ `appsettings.json`.

---

### 1. Đọc token key và kiểm tra:

`var tokenKey = configuration["TokenKey"] ?? throw new ArgumentNullException("TokenKey is not configured."); if (tokenKey.Length < 64) {     throw new ArgumentException("Token key must be at least 64 characters long."); }`

 Đảm bảo:

- `TokenKey` tồn tại
    
- Độ dài đủ mạnh (≥ 64 ký tự) để tránh bị tấn công brute-force
    

---

### 2. Tạo khóa bảo mật và credential để ký token:
`var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)); var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);`

- Mã hóa tokenKey thành dạng byte
    
- Dùng thuật toán `HmacSha512` để **ký token**
    

---

### 3. Tạo các `claim` bên trong token:
`var claims = new List<Claim> {     new Claim(ClaimTypes.Email, user.Email),     new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };`

- `ClaimTypes.Email`: lưu email của user
    
- `ClaimTypes.NameIdentifier`: lưu ID (dạng chuỗi)
    

→ Các `claim` này sẽ được **đọc lại ở phía backend** khi người dùng gửi token về sau.

---

## 4. Cách đang dùng: `SecurityTokenDescriptor` (an toàn, rõ ràng)
`var tokenDescriptor = new SecurityTokenDescriptor {     Subject = new ClaimsIdentity(claims),     Expires = DateTime.UtcNow.AddDays(7),     SigningCredentials = creds,     Issuer = configuration["TokenIssuer"],     Audience = configuration["TokenAudience"] }; var tokenHandler = new JwtSecurityTokenHandler(); var token = tokenHandler.CreateToken(tokenDescriptor); return tokenHandler.WriteToken(token);`

- Tạo 1 đối tượng `SecurityTokenDescriptor` chứa đầy đủ thông tin:
    
    - `Claims`, `Issuer`, `Audience`, `Expires`, `Credentials`
        
- Dễ cấu hình, dễ bảo trì, dễ tích hợp vào các thư viện như IdentityServer
    
---

##  5. Cách bị comment: `JwtSecurityToken` trực tiếp
`// var token = new JwtSecurityToken( //     issuer: configuration["TokenIssuer"], //     audience: configuration["TokenAudience"], //     claims: claims, //     expires: DateTime.UtcNow.AddDays(7), //     signingCredentials: creds // ); // return new JwtSecurityTokenHandler().WriteToken(token);`

###  Ưu điểm:
- Trực tiếp, rõ ràng, không qua trung gian
###  Nhược điểm:
- Không tiện mở rộng hoặc cấu hình sau này
    
- Thiếu các tiện ích như `Subject = ClaimsIdentity` (phải tự làm thủ công)
    
- Dễ sai sót nếu cấu hình nhiều thông tin
---

##  So sánh tóm tắt

| Tiêu chí                                  | `SecurityTokenDescriptor` (hiện tại) | `JwtSecurityToken` (bị comment)    |
| ----------------------------------------- | ------------------------------------ | ---------------------------------- |
| Dễ đọc, dễ mở rộng                        | ✅ Có                                 | ❌ Kém hơn                          |
| Phù hợp với cấu hình `Issuer`, `Audience` | ✅ Tốt                                | ✅ Tốt                              |
| Gọn gàng, chuẩn dùng thực tế              | ✅ Rất chuẩn                          | ⚠ Chỉ dùng khi cần tối ưu thủ công |
| Được dùng trong các thư viện lớn          | ✅ (IdentityServer, Auth0)            | ❌ Ít dùng                          |
|                                           |                                      |                                    |

---

# Kết luận

-  **Cách dùng `SecurityTokenDescriptor` là chuẩn hơn**, linh hoạt, mở rộng dễ, bảo trì tốt.
    
-  Cách dùng `JwtSecurityToken` trực tiếp không sai, nhưng **phù hợp với dự án đơn giản hoặc demo**.

---

Create UserDto and Apply in AccountController
D:\Code\.Net\Code\DatingApp\API\appsettings.Development.json
```cs
"TokenKey": "super key"
```

``` website
[JSON Web Tokens - jwt.io](https://www.jwt.io/)
```
---
Adding Authorize in MemberController.cs
```cs
[Authorize]
```
```nuget
Microsoft.AspNetCore.Authentication.JwtBearer @Microsoft
```
