using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager; 
    private readonly ITokenService _tokenService;
    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            // Validate the incoming request model
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Create a new AppUser object from the registration data
            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            // Attempt to create the user with the specified password
            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (!createdUser.Succeeded)
                return StatusCode(500, createdUser.Errors); // Return error if creation fails

            // Assign the new user to the "User" role
            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            if (!roleResult.Succeeded)
                return StatusCode(500, roleResult.Errors); // Return error if role assignment fails

            // Success: user is created and assigned a role
            return Ok(
                new NewUserDto
                {
                    UserName = appUser.UserName,
                    Email = appUser.Email,
                    Token = _tokenService.CreateToken(appUser)
                }
            );
        }
        catch (Exception e)
        {
            return StatusCode(500, e); 
        }
    }
}