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
    public class MembersController(IMembeRepository membeRepository, IPhotoService photoService) : BaseApiController
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
            // First check if the member exists
            var member = await membeRepository.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound(); // Returns 404 only if member doesn't exist
            }
            
            var photos = await membeRepository.GetPhotosForMemberAsync(id);
            // Return empty array if no photos, not 404
            return Ok(photos ?? new List<Photo>()); // Returns the member's photos or empty array
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
        [HttpPost("add-photo")]
        public async Task<ActionResult<Photo>> AddPhoto([FromForm] IFormFile file)
        {
            var memberId = User.GetMemberId();
            var member = await membeRepository.GetMemberForUpdate(memberId);
            if (member == null)
            {
                return BadRequest("Could not get member");
            }

            var uploadResult = await photoService.UploadPhotoAsync(file);
            if (uploadResult.Error != null)
            {
                return BadRequest(uploadResult.Error.Message); // Returns 400 Bad Request if upload fails
            }

            var photo = new Photo
            {
                Url = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId,
                MemberId = memberId
            };

            if (member.ImageUrl == null)
            {
                member.ImageUrl = photo.Url;
                member.User.ImageUrl = photo.Url;
            }

            member.Photos.Add(photo);
            if (await membeRepository.SaveAllAsync())
            {
                return Ok(photo); // Returns the uploaded photo details
            }
            return BadRequest("Failed to add photo"); // Returns 400 Bad Request on failure
        }
        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var memberId = User.GetMemberId();
            var member = await membeRepository.GetMemberForUpdate(memberId);
            if (member == null)
            {
                return BadRequest("Could not get member from token");
            }

            var photo = member.Photos.SingleOrDefault(x => x.Id == photoId);
            if (member.ImageUrl == photo?.Url || photo == null)
            {
                return BadRequest("Cannot set this as main image");
            }

            member.ImageUrl = photo.Url;
            member.User.ImageUrl = photo.Url;

            if (await membeRepository.SaveAllAsync())
            {
                return NoContent(); // Returns 204 No Content on successful update
            }
            return BadRequest("Failed to set main photo"); // Returns 400 Bad Request on failure
        }
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var memberId = User.GetMemberId();
            var member = await membeRepository.GetMemberForUpdate(memberId);
            if (member == null)
            {
                return BadRequest("Could not get member from token");
            }

            var photo = member.Photos.SingleOrDefault(x => x.Id == photoId);
            
            if(photo == null || photo.Url == member.ImageUrl)
            {
                return BadRequest("Cannot delete this photo");
            }

            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null)
                {
                    return BadRequest(result.Error.Message); // Returns 400 Bad Request if deletion fails
                }
            }

            member.Photos.Remove(photo);
            if (await membeRepository.SaveAllAsync())
            {
                return Ok(); // Returns 200 OK on successful deletion
            }
            return BadRequest("Failed to delete photo"); // Returns 400 Bad Request on failure
        }
    }
}
