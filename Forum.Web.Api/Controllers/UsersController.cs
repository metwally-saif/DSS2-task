using Forum.Application.Dto;
using Forum.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Forum.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(UserService userService) : ControllerBase
    {
        private readonly UserService _userService = userService;

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
        {
            var (Users, Error) = await _userService.GetAllUsersAsync();
            if (!string.IsNullOrEmpty(Error))
            {
                return BadRequest(Error);
            }

            if (!Users.Any())
            {
                return NoContent();
            }

            return Ok(Users);


        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAsync(
            [Required, FromRoute] long? userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var result = await _userService.GetUsersAsync(userId);

            if (result.User == null || !string.IsNullOrEmpty(result.Error))
            {
                return NotFound(result.Error); // Return error message if user not found or other error occurred
            }

            return Ok(result.User);
        }

        [HttpPost("{role}")]
        public async Task<IActionResult> CreateAsync(
            [FromBody, Required] CreateUserDto user,
            [FromRoute] Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var newUser = await _userService.CreateUserAsync(user, role);
            if (user == null)
            {
                return BadRequest("somthing went wrong");
            }

            // Assuming GetAsync returns the user details by ID
            return Ok(newUser);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateAsync(
            [Required, FromRoute] long? userId,
            [FromBody, Required] UpdateUserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var (_, Error) = await _userService.UpdatedUserAsync(userId, user);
            if (!string.IsNullOrEmpty(Error))
            {
                return BadRequest(Error);
            }

            var userDto = await _userService.GetUsersAsync(userId);
            if (!string.IsNullOrEmpty(userDto.Error))
            {
                return BadRequest(userDto.Error);
            }

            return Ok(userDto);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAsync(
           [Required, FromRoute] long? userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var user = await _userService.DeleteUserAsync(userId);
            if (!string.IsNullOrEmpty(user.Error))
            {
                return BadRequest(user.Error);
            }

            return Ok(user.User);
        }
    }
}
