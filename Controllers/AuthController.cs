using Microsoft.AspNetCore.Identity;
using MyApplicatioon.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using MyApplicatioon.Services;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace MyApplicatioon.Controllers
{
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExist = await userManager.FindByEmailAsync(model.Email);
            if (userExist != null)
                return StatusCode(StatusCodes.Status409Conflict, new Response { Status = "Error", Message = "User already exists" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed" });
            }

            return Ok(new Response { Status = "Success", Message = "User created successfull" });

        }

        [HttpPost]
        [Route("jwt/tokens")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                var access = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(30),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
                var refresh = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(7),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
                return Ok(new
                {
                    access = new JwtSecurityTokenHandler().WriteToken(access),
                    refresh = new JwtSecurityTokenHandler().WriteToken(refresh)
                });
            }
            return Unauthorized();
        }
    }
}