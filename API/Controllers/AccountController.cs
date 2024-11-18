using API.DTOs;
using API.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace API.Controllers;

public class AccountController(SignInManager<AppUser> signInManager): BaseApiController
{

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var user = new AppUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {

            foreach (var error in result.Errors)
            {

                ModelState.AddModelError(error.Code, error.Description);

            }

            return ValidationProblem();

        }

        return Ok();

    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto loginDto)
    {
        var user = await signInManager.UserManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return Unauthorized("Invalid credentials.");
        }

        var result = await signInManager.PasswordSignInAsync(user, loginDto.Password, isPersistent: false, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            return Unauthorized("Invalid credentials.");
        }

        // Add claims to the user for the session
        var claims = new List<Claim>
    {
        new Claim("Id", user.Id),
        new Claim("Country", user.Address?.Country ?? "DefaultCountry"), 
        new Claim("Age", 26.ToString()),
        new Claim("EditRole",  "EditRole")
    };

        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
 
        // Replace existing authentication with new claims (re-sign in user)
        await HttpContext.SignInAsync("Cookies", claimsPrincipal);

        return Ok("Logged in successfully.");
    }
    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return NoContent();

    }

    [HttpGet("user-info")]
    public async Task<ActionResult> GetUserInfo() 
    {

        if (User.Identity?.IsAuthenticated==false) 
        {
            return NoContent();
        
        }

        AppUser user=await signInManager.UserManager.GetUserByEmailWithAddress(User);
        
        return Ok(new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            Address=user.Address?.ToDto()
        });

    }

    [HttpGet("auth-status")]
    public ActionResult GetAuthState()
    {
        return Ok(new
        {
            IsAuthenticated=User.Identity?.IsAuthenticated??false
        });
    }

    [Authorize]
    [HttpPost("address")]
    public async Task<ActionResult<Address>>CreateOrUpdateAddress(AddressDto addressDto) 
    {

        var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

        if (user.Address==null)
        {

            user.Address = 
                addressDto.ToEntity();
 
        }
        else
        {
            user.Address.UpdateFromDto(addressDto);        
        }
     
        var result = await signInManager.UserManager.UpdateAsync(user);

        if (!result.Succeeded) {

            return BadRequest("Problem updating user address");
        }

        return Ok(user.Address.ToDto());

    }

}