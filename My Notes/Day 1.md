## Skeleton
a tiny implementation of system that performs a small end to end function --> Final architecture, linking together the main architectural components --> evolve in parallel

## Learning goals
- [x] using the dotnet cli
- [x] api controllers and endpoints
- [x] entity framework
- [x] the api project structure
- [x] configuration and environment variables
- [x] source control

## Creating the .Net app project
linux: ls
window: dir
the path of the project in my laptop: 
```
D:\Code\.Net\Code\DatingApp
```
the commands:
```
mkdir DatingApp
cd DatingApp
```
dotnet cli:
```
dotnet --info
```

```
dotnet new list
```

```
dotnet new webapi -h
```

#Note: using the controller 
```
dotnet new list
```
#start
```
dotnet new sln
```

```
dotnet new webapi -controllers -n API
```
-n: name
```
dotnet sln -h
```

```
dotnet sln add API
```

```
dotnet sln list
```

using VSCode:
setting(ctrl + ,) --> open settings(json) on the right side bar --> justify your setting
go to extentions store --> download C# dev kit, C#, .NET Install Tool, NuGet Gallery, material icons

#start
```
cd API && dotnet run
```

```
dotnet run --launch-profile https
```

if have a warning --> go to 
```
D:\Code\.Net\Code\DatingApp\API\Properties\launchSettings.json
```
and change remove http, keep https
such as:
``` json
{

  "$schema": "https://json.schemastore.org/launchsettings.json",

  "profiles": {

    "https": {

      "commandName": "Project",

      "dotnetRunMessages": true,

      "launchBrowser": false,

      "applicationUrl": "https://localhost:5000;",

      "environmentVariables": {

        "ASPNETCORE_ENVIRONMENT": "Development"

      }

    }

  }

}
```

**Go to file to see the route, end-point, httpget:
```
D:\Code\.Net\Code\DatingApp\API\Controllers\WeatherForecastController.cs
```
And this file has the site to check API work correctly
```
D:\Code\.Net\Code\DatingApp\API\API.http
```

_Changes this for development_
```
D:\Code\.Net\Code\DatingApp\API\appsettings.Development.json
```

```json
{

  "Logging": {

    "LogLevel": {

      "Default": "Information",

      "Microsoft.AspNetCore": "Information"

    }

  }

}
```

_Changes this file, not using OpenAI for document_
```
D:\Code\.Net\Code\DatingApp\API\Program.cs
```

```c#
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseAuthorization();
app.MapControllers();
app.Run();
```

**Some dotnet for cert for https when the site has warning about:
``` warning
Your connection to this site isn't secure

Don't enter any sensitive information on this site (for example, passwords or credit cards). It could be stolen by attackers.

You have chosen to turn off security warnings for this site. [Turn on warnings](edge://page-info/#)
```
## dotnet dev-certs commands:
--help
```
dotnet dev-certs https -h
```
--check
```
dotnet dev-certs https -c
```
--trust
```
dotnet dev-certs https -t
```
--clean
```
dotnet dev-certs https --clean
```

if it doesn't work, you would try to restart the browser or:
```
dotnet dev-certs https --clean 
dotnet dev-certs https --trust
```

# Entities - Class, OOP, Table, Property
*In API.csproj
```cs
<ImplicitUsings>enable</ImplicitUsings>
```
This help us auto define global to reduce the source code, so we can delete using system on classes where have them.*

typing prop and 'tab' in class to have template

*Compare*
## **Tổng hợp 6 cách khai báo thuộc tính Id:**
### **1. `public string? Id { get; set; }`**

- **Nullable**: Cho phép `null`
- **Default**: `null`
- **Use case**: Khi cần phân biệt "chưa có" vs "rỗng"

### **2. `public string Id { get; set; } = string.Empty;`**

- **Non-nullable**: Không cho phép `null`
- **Default**: `""`
- **Use case**: Đảm bảo luôn có string, tránh null exception

### **3. `public required string Id { get; set; }`**

- **Mandatory**: Bắt buộc khởi tạo (C# 11+)
- **Compile-time**: Compiler check
- **Use case**: Khi Id là required field

### **4. `public string Id { get; set; } = Guid.NewGuid().ToString();`**

- **Auto-generated**: Tự động tạo unique ID
- **Performance cost**: Tạo GUID mỗi object
- **Use case**: Cần unique identifier

### **5. `public string Id { get; set; } = "";`**

- **Empty string**: Giống string.Empty
- **Literal**: Hard-coded empty
- **Use case**: Tương đương option 2

### **6. `public string Id { get; set; } = Empty.ToString();`** ❌

- **Lỗi cú pháp**: `Empty` không tồn tại
- **Không sử dụng được**

## **So sánh toàn diện:**

| Tiêu chí            | 1 (`string?`)  | 2 (`string.Empty`) | 3 (`required`) | 4 (`Guid`)     | 5 (`""`)      |
| ------------------- | -------------- | ------------------ | -------------- | -------------- | ------------- |
| **Null Safety**     | ❌ Cần check    | ✅ An toàn          | ✅ An toàn      | ✅ An toàn      | ✅ An toàn     |
| **Performance**     | ✅ Tốt          | ✅ Tốt              | ✅ Tốt          | ❌ Chi phí GUID | ✅ Tốt         |
| **Flexibility**     | ✅ Cao          | ⚠️ Trung bình      | ⚠️ Hạn chế     | ❌ Thấp         | ⚠️ Trung bình |
| **Security**        | ❌ Runtime risk | ✅ An toàn          | ✅ An toàn      | ✅ Unique       | ✅ An toàn     |
| **Maintainability** | ❌ Phức tạp     | ✅ Đơn giản         | ✅ Rõ ràng      | ✅ Tự động      | ✅ Đơn giản    |

## **🎯 Khuyến nghị chọn:**

### **Cho Entity/Domain Model:**
```csharp
public required string Id { get; set; }  // Option #3
```
**Lý do**: Bắt buộc khởi tạo, compile-time safety
### **Cho DTO/API Model:**
```csharp
public string Id { get; set; } = string.Empty;  // Option #2
```
**Lý do**: An toàn, không cần validation phức tạp
### **Cho Auto-generated ID:**
```csharp
public string Id { get; set; } = Guid.NewGuid().ToString();  // Option #4
```
**Lý do**: Unique, phù hợp cho primary key
### **❌ Tránh:**
- Option #1: Dễ gây NullReferenceException
- Option #6: Lỗi cú pháp
**Kết luận**: Chọn **Option 3** (required) cho hầu hết trường hợp - an toàn nhất và rõ ràng nhất.

---
An Object Relational Mapper (ORM)
Translate code into SQL commands that update the table in database -CRUD
![[Drawing 2025-07-29 09.49.38.excalidraw]]
- Querying
- Change Tracking
- Saving
-  Concurrency
- Transactions
- Caching
- Built-in conventions
- Configurations
- Migrations

Download Nuget:
Microsoft.EntityFrameworkCore.Design @Microsoft
Microsoft.EntityFrameworkCore.Sqlite @Microsoft

dotnet restore

# Folder Data in API
using Microsoft.EntityFrameworkCore;
Class AppDbContext : DbContext
typing 'ctor' to have constructor

Command to setup dotnet ef:
*check:*
``` terminal
dotnet tool list -g
```
*EF:*
```
dotnet tool install -g dotnet-ef
```
```
dotnet ef
```
```
dotnet ef migrations -h
```

```
dotnet ef migrations add InitialCreate -o Data/Migrations
```
*Update*
```
dotnet ef database update
```


---
Bugs của extension bản mới =))
- Bug trong extension `alexcvzz.vscode-sqlite`
- SQLite 3.41.0+ không support double quotes cho string literals
- Extension dùng `type="table"` thay vì `type='table'`

**Fix options:**

**1. Manual patch (Windows path):**

```
%USERPROFILE%\.vscode\extensions\alexcvzz.vscode-sqlite-*\dist\extension.js
```

Tìm và thay:

javascript

```javascript
WHERE (type=\"table\" OR type=\"view\")
// thành:
WHERE (type='table' OR type='view')
```

**2. Alternative extensions:**

- `SQLite Viewer` (qwtel.sqlite-viewer)
- `vscode-sqlite3-editor`

**3. Workaround:**

- Dùng SQLite CLI: `sqlite3 dating.db`
- DB Browser for SQLite (standalone app)

**Security note:** Editing extension files có thể bị overwrite khi update. Alternative extension là safer approach.

**Recommendation:** Switch sang `SQLite Viewer` extension để tránh compatibility issues với SQLite versions mới.
---> Được thì có thể sửa và dùng, sau đó xóa

---

```c#
dotnet watch
```

*Sync*:
``` csharp
namespace API.Controllers
{
    [Route("api/[controller]")] // localhost:5001/api/members
    [ApiController]
    public class MembersController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IReadOnlyList<AppUser>> GetMembers()
        {
            var members = context.Users.ToList();
            return Ok(members); // Returns a list of members
        }
        [HttpGet("{id}")] // localhost:5001/api/members/{id}
        public ActionResult<AppUser> GetMember(string id)
        {
            var member = context.Users.Find(id);
            if (member == null)
            {
                return NotFound(); // Returns 404 if member not found
            }
            return Ok(member); // Returns the member details
        }
    }
}

```
*Async*:
```csharp
public class MembersController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var members = await context.Users.ToListAsync();
            return Ok(members); // Returns a list of members
        }
        [HttpGet("{id}")] // localhost:5001/api/members/{id}
        public async Task<ActionResult<AppUser>> GetMember(string id)
        {
            var member = await context.Users.FindAsync(id);
            if (member == null)
            {
                return NotFound(); // Returns 404 if member not found
            }
            return Ok(member); // Returns the member details
        }
    }
```

---
Template for gitignore
```
dotnet new gitignore
```















