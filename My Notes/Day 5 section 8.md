#summary 
[[Day 5 section 7]]
## Learning goals:
- [x] API middleware
- [x] Angular interceptors
- [x] Troubleshooting exceptions

---
# Section 8:
#learn #Note #links
## Learning goals
- [x] Entity framework relationships
- [x] Entity framework conventions
- [x] Seeding Data into the database
- [x] The reporsitory pattern
- [x] Shaping data

---

### Extended the User entity
#dotnet #entiy
***Class Member***
```cs
namespace API.Entities;

public class Member
{
    public string Id { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public string? ImageUrl { get; set; }
    public required string DisplayName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public string? Gender { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }    
    // Navigation property
    [ForeignKey(nameof(Id))]
    public AppUser User { get; set; } = null!;
}

```
***Class Photo***
```cs

namespace API.Entities;

public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public string? PublicId { get; set; }
    // Navigation property
    public Member Member { get; set; } = null!;
}

```
***AppUser***
```cs
namespace API.Entities;

public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public string? ImageUrl { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
}

```

***AppDbContext***
```cs
using API.Entities;
using Microsoft.EntityFrameworkCore;
namespace API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Photo> Photos { get; set; }
}

```

```cs
dotnet ef migrations add MemberEntityAdded
dotnet ef database update
```

---

### Seed data

```cs
namespace API.Entities;

public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public string? PublicId { get; set; }
    // Navigation property
    public Member Member { get; set; } = null!;
    public string MemberId { get; set; } = null!;
    
}

```

```cs
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

public class Member
{
    public string Id { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public string? ImageUrl { get; set; }
    public required string DisplayName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public string? Gender { get; set; }
    public string? Description { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    // Navigation property
    public List<Photo> Photos { get; set; } = new();
    [ForeignKey(nameof(Id))]
    public AppUser User { get; set; } = null!;
}

```

```cs
namespace API.Data;

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;
public class Seed
{
    public static async Task SeedUser(AppDbContext context)
    {
        if (await context.Users.AnyAsync()) return;
        var memberData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var members = JsonSerializer.Deserialize<List<SeedUserDto>>(memberData);
        if (members == null)
        {
            Console.WriteLine("No members found in the seed data.");
            return;
        }

        using var hmac = new HMACSHA512();

        foreach (var member in members)
        {
            var user = new AppUser
            {
                Id = member.Id,
                Email = member.Email,
                DisplayName = member.DisplayName,
                ImageUrl = member.ImageUrl,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd")),
                PasswordSalt = hmac.Key,
                Member = new Member
                {
                    Id = member.Id,
                    DisplayName = member.DisplayName,
                    Description = member.Description,
                    DateOfBirth = member.DateOfBirth,
                    ImageUrl = member.ImageUrl,
                    Gender = member.Gender,
                    City = member.City,
                    Country = member.Country,
                    LastActive = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                }
            };
            user.Member.Photos.Add(new Photo
            {
                Url = member.ImageUrl!,
                MemberId = member.Id
            });
            
            context.Users.Add(user);
        }
        
        await context.SaveChangesAsync();
    }
}

```
when start app, method excutes, but database work incorrectly --> Automatically create DB, excute migration, correct schema

*check*:
```cs
dotnet ef migrations has-pending-model-changes
```

```cs
dotnet ef database drop
dotnet watch
```

---
***move this code in loop for hashing and receive different password***
```cs
using var hmac = new HMACSHA512();
```


---
### Repository pattern
- Between mediates between the domain and data mapping layers, acting like an in-memory domain object collection

#excalidraw
![[Pasted image 20250809215452.png]]

#excalidraw then
![[Pasted image 20250809215703.png]]

#Note
Notes: Abstraction --> directly dbcontext --> excute logic --> DbContext Save

**DbContext**:
- Users.First()
- Users.FirstOrDefault()
- Users.SingleOrDefault()
- User.Include(x=>x.Thing).FirstOrDefault
- another 1000 methods
**Repository**:
- GetUser()
- GetUsers()
- UpdateUser()
- SaveAll()

#excalidraw 
![[Pasted image 20250809220438.png]]

![[Pasted image 20250809220749.png]]

![[Pasted image 20250809221038.png]]

![[Pasted image 20250809221147.png]]


#### mocks
```cs
var user = new User(UserName = "bob");
var mockRepo = MockRepository.GenerateMock<IRepository>();
mockRepo.Expect(x=>x.GetUser("bob)).Return(user);
```

***Advantages of repository pattern***
- Minimize duplicate query logic
- Decouples application from persistence framwork
- All Database queries are centralised and not scattered throughout the app
- All us to chane ORM easily
- Promotes testability
--> easily mock a repository interface, testing against the dbcontext is more difficult

***Disadvantages of repository pattern***
- Adbstration of an abstraction
- each root entity should have it's own repository which means more code
- also need to implement the UnitofWork pattern to control transactions
---

### Creating member repository

*Interface member repository:*
```cs
using System;
using API.Entities;

namespace API.interfaces;

public interface IMembeRepository
{
    void Update(Member member);
    Task<bool> SaveAllAsync();
    Task<IReadOnlyList<Member>> GetMembersAsync();
    Task<Member?> GetMemberByIdAsync(string id);
    Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId);
}

```

Using code action (ctrl .)
```cs
using System;
using API.Entities;
using API.interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class MemberRepository(AppDbContext context) : IMembeRepository
{
    public async Task<Member?> GetMemberByIdAsync(string id)
    {
        return await context.Members.FindAsync(id);
    }

    public async Task<IReadOnlyList<Member>> GetMembersAsync()
    {
        return await context.Members.ToListAsync();
    }

    public async Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId)
    {
        // return await context.Photos.Where(p => p.MemberId == memberId).ToListAsync();
        return await context.Members
            .Where(m => m.Id == memberId)
            .SelectMany(m => m.Photos)
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Update(Member member)
    {
        context.Entry(member).State = EntityState.Modified;
    }
}

```

add this line in program.cs
```cs
builder.Services.AddScoped<IMembeRepository, MemberRepository>();
```

---
### Update member controller
```cs
using API.Entities;
using API.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MembersController(IMembeRepository membeRepository) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var members = await membeRepository.GetMembersAsync();
            return Ok(members); // Returns a list of members
        }
        [Authorize] // Ensures that only authenticated users can access this endpoint
        [HttpGet("{id}")] // localhost:5001/api/members/{id}
        public async Task<ActionResult<AppUser>> GetMember(string id)
        {
            var member = await membeRepository.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound(); // Returns 404 if member not found
            }
            return Ok(member); // Returns the member details
        }
        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string id)
        {
            var photos = await membeRepository.GetPhotosForMemberAsync(id);
            if (photos == null || photos.Count == 0)
            {
                return NotFound(); // Returns 404 if no photos found
            }
            return Ok(photos); // Returns the member's photos
        }
    }
}
```

---

**Using** 
[AutoMapper in C# with Examples - Dot Net Tutorials](https://dotnettutorials.net/lesson/automapper-in-c-sharp/)
(Commercial)

## JSONIGNORE
*****Member:*****
```cs
// Navigation property
    [JsonIgnore]
    public List<Photo> Photos { get; set; } = new();
    [JsonIgnore]
    [ForeignKey(nameof(Id))]
    public AppUser User { get; set; } = null!;
```
***Photo:***
```cs
 // Navigation property
    [JsonIgnore]
    public Member Member { get; set; } = null!;
    public string MemberId { get; set; } = null!;
```

