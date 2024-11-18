using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;
using System.Security.Authentication;
using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static async Task<AppUser> GetUserByEmail(this UserManager<AppUser> userManager,
            ClaimsPrincipal user)
        {
            var userToReturn = await
                userManager.
                Users.
                FirstOrDefaultAsync(x => x.Email == user.GetEmail());

            return userToReturn ?? throw new AuthenticationException("User not found");

        }

        public static async Task<AppUser> GetUserByEmailWithAddress(this UserManager<AppUser> userManager,
          ClaimsPrincipal user)
        {
            var userToReturn = await
                userManager.
                Users
               .Include(x => x.Address)
               .FirstOrDefaultAsync(x => x.Email == user.GetEmail());

            return userToReturn ?? throw new AuthenticationException("User not found");

        }

        public static string GetEmail(this ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            return email ?? throw new AuthenticationException("Email claim not found");
        }

    }

    public static class ClaimsPrincipalExtensionsv2
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst("Id")?.Value;
        }

        public static string GetCountry(this ClaimsPrincipal user)
        {
            return user.FindFirst("Country")?.Value;
        }

        public static int GetAge(this ClaimsPrincipal user)
        {
            return int.TryParse(user.FindFirst("Age")?.Value, out var age) ? age : 0;
        }

        public static string GetEditRole(this ClaimsPrincipal user)
        {
            return user.FindFirst("EditRole")?.Value;
        }
    }
}

