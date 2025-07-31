using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class MembersController(AppDbContext context) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var members = await context.Users.ToListAsync();
            return Ok(members); // Returns a list of members
        }
        [Authorize] // Ensures that only authenticated users can access this endpoint
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
}
