using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
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
        [HttpPut]
        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
        {
            var memberId = User.GetMemberId();

           var member = await membeRepository.GetMemberForUpdate(memberId);
           if (member == null)
           {
               return BadRequest("Could not get member");
           }

           member.DisplayName = memberUpdateDto.DisplayName ?? member.DisplayName;
           member.Description = memberUpdateDto.Description ?? member.Description;
           member.City = memberUpdateDto.City ?? member.City;
           member.Country = memberUpdateDto.Country ?? member.Country;


            member.User.DisplayName = memberUpdateDto.DisplayName ?? member.User.DisplayName;
            membeRepository.Update(member);  // optional

           if (await membeRepository.SaveAllAsync())
           {
               return NoContent(); // Returns 204 No Content on successful update
           }
           return BadRequest("Failed to update the member"); // Returns 400 Bad Request on failure
        }
    }
}
