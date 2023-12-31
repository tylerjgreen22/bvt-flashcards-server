using System.Security.Claims;
using API.DTOs;
using API.Services;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    // API controller for managing Identity
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        // Injecting token service and user manager to facilitate management of users and tokens via constructor
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        // Login method that logs a user in as long as they pass checks. Returns a user
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            // Retrieve user from user manager by email. Return unauthorized if failed
            var user = await _userManager.Users.Include(p => p.Pictures).FirstOrDefaultAsync(user => user.Email == loginDto.Email);
            if (user == null) return Unauthorized();

            // Check if provided password matches user password. If pass, return user object otherwise return unauthorized
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (result)
            {
                return CreateUserObject(user);
            }

            return Unauthorized();
        }

        // Register method that registers a user as long as they pass checks. Returns a user
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // Check if username is already taken
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
            {
                return BadRequest("Username is already taken");
            }

            // Check is email is already taken
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                return BadRequest("Email is already taken");
            }

            // Create a new app user with the provided username, email and password
            var user = new AppUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Username
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            // If user successfully created, return user object, otherwise return bad request   
            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return BadRequest(result.Errors);
        }

        // Get user based on the email passed via the claims in the token. Returns a user
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            // Find the user based on email claim from token. Returns user if found, bad request if not found
            var user = await _userManager.Users.Include(p => p.Pictures).FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));
            if (user == null) return BadRequest("User not found");

            return CreateUserObject(user);
        }

        // Change users username
        [Authorize]
        [HttpPut("username")]
        public async Task<IActionResult> ChangeUsername(ChangeUserDto changeUserDto)
        {
            // Find user based on email claim from token. If user not found returns bad request
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            if (user == null) return BadRequest("User not found");

            // Check if provided password matches user password. If no match, return bad request
            if (!await _userManager.CheckPasswordAsync(user, changeUserDto.Password))
            {
                return BadRequest("Incorrect password");
            }

            // Set the users username to the provided new username and update the user in the user manager
            user.UserName = changeUserDto.Username;
            var result = await _userManager.UpdateAsync(user);

            // If successful return Ok, otherwise return bad request
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        // Change users password
        [Authorize]
        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword(ChangeUserDto changeUserDto)
        {
            // Find user based on email claim from token. If user not found returns bad request
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            if (user == null) return BadRequest("User not found");

            // Attempt to change user password based on if provided password is correct. If successful returns Ok otherwise returns bad request
            var result = await _userManager.ChangePasswordAsync(user, changeUserDto.Password, changeUserDto.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        // Delete users account
        [Authorize]
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteUser(ChangeUserDto changeUserDto)
        {
            // Find user based on email claim from token. If user not found returns bad request
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            if (user == null) return BadRequest("User not found");

            // If the provided password matches the users password, attempts to delete the user. If successful, returns Ok() otherwise, returns bad request
            if (!await _userManager.CheckPasswordAsync(user, changeUserDto.Password))
            {
                return BadRequest("Incorrect password");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        // Helper method that creates a UserDto based on a passed in user
        private UserDto CreateUserObject(AppUser user)
        {
            return new UserDto
            {
                Username = user.UserName,
                Image = user?.Pictures?.FirstOrDefault(x => x.IsMain)?.Url,
                Token = _tokenService.CreateToken(user),
                Pictures = user.Pictures
            };
        }
    }
}