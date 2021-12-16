using MangaCMS.DAL;
using MangaCMS.Models;
using MangaCMS.Models.JWT;
using MangaCMS.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MangaCMS.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly MangaCMSContext _mangaContext;

        public AuthenticationController(MangaCMSContext context, UserManager<CustomUser> userManager)
        {
            _mangaContext = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("GetToken")]
        public IActionResult Authenticate(
            [FromBody] AuthRequest Model,
            [FromServices] IJwtSigningEncodingKey signingEncodingKey
            )
        {
            var current_user = _userManager.FindByNameAsync(Model.Login);
            var user_roles = _userManager.GetRolesAsync(current_user.Result);

            if (current_user.Result != null)
            {
                
                PasswordVerificationResult verified = _userManager.PasswordHasher.VerifyHashedPassword(current_user.Result, current_user.Result.PasswordHash, Model.Password);

                if (verified == PasswordVerificationResult.Success)
                {
                    var claims = new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, current_user.Result.UserName),
                        //new Claim(ClaimTypes.Role, user_roles.Result.First()),
                    };
                    //foreach (var r in user_roles.Result)
                    //{
                    //    claims.Append(new Claim(ClaimTypes.Role, r.ToString()));
                    //};
                    var token = new JwtSecurityToken(
                        issuer: SigningSymmetricKey.ISSUER,
                        audience: SigningSymmetricKey.AUDIENCE,
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(SigningSymmetricKey.LIFETIME),
                        signingCredentials: new SigningCredentials(
                                signingEncodingKey.GetKey(),
                                signingEncodingKey.SigningAlgorithm)
                    );

                    string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(
                        new JwtResponse(jwtToken, "NotExist")
                    );

                }
            }

            ModelState.AddModelError("Auth", "Invalid username or password.");
            return BadRequest(ModelState);
        }

        // Refresh Token
        // Revoke Token

    }
}
