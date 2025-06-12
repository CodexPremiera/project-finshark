using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager; 
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var user = await _userManager.Users
            .FirstOrDefaultAsync(user => user.UserName == loginDto.Username.ToLower());
        
        if (user == null) 
            return Unauthorized("Invalid username!");
        
        if (string.IsNullOrWhiteSpace(loginDto.Password))
            return BadRequest("Password is required.");
        
        var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
        
        if (!result.Succeeded) 
            return Unauthorized("Username or password is incorrect!");
        
        // Success: user is logged in
        return Ok(
            new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            }
        );
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

            // Success: user is created
            return Ok(
                new NewUserDto
                {
                    UserName = appUser.UserName!,
                    Email = appUser.Email!,
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