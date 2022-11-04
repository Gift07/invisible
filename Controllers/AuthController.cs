using Microsoft.AspNetCore.Identity;
using MyApplicatioon.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
            if (model.Password != model.ConfirmPassword)
                return StatusCode(
                    StatusCodes.Status400BadRequest,
                    new Response
                    {
                        Status = "Error",
                        Message = "Passwords do not match"
                    });

            var userExist = await userManager.FindByEmailAsync(model.Email);
            if (userExist != null)
                return StatusCode(
                    StatusCodes.Status409Conflict,
                    new Response
                    {
                        Status = "Error",
                        Message = "User already exists"
                    });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.CompanyName
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Status = "Error",
                        Message = "User creation failed"
                    });
            }
            return Ok(
                new FirstResponse
                {
                    Status = "Success",
                    Message = "User created successfull",
                    Email = model.Email,
                    Password = model.Password
                });

        }

        [HttpPost]
        [AllowAnonymous]
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
                HttpContext.Response.Cookies.Append("X-Access-Token",
                 new JwtSecurityTokenHandler().WriteToken(access),
                  new CookieOptions()
                  {
                      HttpOnly = true,
                      Expires = DateTime.UtcNow.AddMinutes(30),
                      IsEssential = true,
                      SameSite = SameSiteMode.None,
                  });
                HttpContext.Response.Cookies.Append("X-Username",
                 user.Email,
                 new CookieOptions()
                 {
                     HttpOnly = true,
                     Expires = DateTime.UtcNow.AddDays(7),
                     IsEssential = true,
                     SameSite = SameSiteMode.None,
                 });
                HttpContext.Response.Cookies.Append("X-Refresh-Token",
                 new JwtSecurityTokenHandler().WriteToken(refresh),
                  new CookieOptions()
                  {
                      HttpOnly = true,
                      Expires = DateTime.UtcNow.AddDays(7),
                      IsEssential = true,
                      SameSite = SameSiteMode.None,
                  });
                return Ok(
                new Response
                {
                    Status = "Success",
                    Message = "Login successfull"
                });
            }
            return Unauthorized();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("ping")]
        public string Ping() => "pong";

        // obtaining access new pair of tokens
        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                var token = "";
                if (Request.Cookies.TryGetValue("X-Username", out var userName) && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken))
                    token = refreshToken;
                return Unauthorized();

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true, //you might want to validate the audience and issuer depending on your use case
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                    ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var user = await userManager.FindByEmailAsync(userName);

                if (user != null)
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
                    Response.Cookies.Append("X-Access-Token",
                    new JwtSecurityTokenHandler().WriteToken(access),
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    });
                    Response.Cookies.Append("X-Username", user.Email,
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    });
                    Response.Cookies.Append("X-Refresh-Token",
                    new JwtSecurityTokenHandler().WriteToken(refresh),
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    });
                    return Ok();
                }
                return Unauthorized();

            }
            catch (Exception e)
            {
                return StatusCode(
                StatusCodes.Status500InternalServerError,
                new Response
                {
                    Status = "Failed",
                    Message = e.Message
                });
            }
        }
    }
}
