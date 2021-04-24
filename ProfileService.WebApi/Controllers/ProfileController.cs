using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfileService.WebApi.DataContracts;
using ProfileService.WebApi.Exceptions;
using ProfileService.WebApi.Model;
using ProfileService.WebApi.Services;

namespace ProfileService.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<FullProfile>> GetProfile(string username)
        {
            try
            {
                var profile = await _profileService.GetFullProfile(username);
                return Ok(profile);
            }
            catch (ProfileNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<FullProfile>> CreateProfile(CreateProfileRequest request)
        {
            if (!request.IsValid(out var error))
            {
                return BadRequest(error);
            }

            try
            {
                Profile profile = request.ToProfile();
                await _profileService.AddProfile(profile);
                return CreatedAtAction(nameof(GetProfile), request.Username,
                    await _profileService.GetFullProfile(profile));
            }
            catch (DuplicateProfileException e)
            {
                return Conflict(e.Message);
            }
            catch (EmployerNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("{username}")]
        public async Task<ActionResult<FullProfile>> UpdateProfile(string username, UpdateProfileRequest request)
        {
            var profile = request.ToProfile(username);
            await _profileService.UpdateProfile(profile);
            return Ok(await _profileService.GetFullProfile(profile));
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteProfile(string username)
        {
            try
            {
                await _profileService.DeleteProfile(username);
                return Ok();
            }
            catch (ProfileNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}