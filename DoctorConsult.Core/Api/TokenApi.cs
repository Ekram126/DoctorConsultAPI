using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace DoctorConsult.Core.Api
{
    //public class TokenApi: ITokenService
    //{
    //    private readonly IConfiguration configuration;

    //    public TokenApi(IConfiguration configuration)
    //    {
    //        this.configuration = configuration;
    //    }
    //    public async Task<string> CreateToken(ApplicationUser user, UserManager<ApplicationUser> userManager)
    //    {
    //        var authClaims = new List<Claim>()
    //     {
    //         new Claim(ClaimTypes.GivenName, user.UserName),
    //     }; 

    //        var userRoles = await userManager.GetRolesAsync(user);

    //        foreach (var role in userRoles)
    //            authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));


    //        var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

    //        var token = new JwtSecurityToken(

    //            issuer: configuration["JWT:ValidIssuer"],
    //            audience: configuration["JWT:ValidAudience"],
    //            expires: DateTime.Now.AddDays(double.Parse(configuration["JWT:DurationInDays"])),
    //            claims: authClaims,
    //            signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
    //            );

    //        return new JwtSecurityTokenHandler().WriteToken(token);
    //    }
    //}

}
