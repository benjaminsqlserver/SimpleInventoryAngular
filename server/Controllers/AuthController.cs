using System;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

using InventoryManager.Authentication;
using InventoryManager.Data;
using InventoryManager.Models;

namespace InventoryManager.Controllers
{
    [Route("/auth/[action]")]
    public partial class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        
        private readonly IWebHostEnvironment env;

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.env = env;
        }

        private IActionResult Error(string message)
        {
            return BadRequest(new { error = new { message } });
        }

        private IActionResult Jwt(IEnumerable<Claim> claims)
        {
            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = TokenProviderOptions.Issuer,
                Audience = TokenProviderOptions.Audience,
                SigningCredentials = TokenProviderOptions.SigningCredentials,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenProviderOptions.Expiration)
            });

            return Json(new { access_token = handler.WriteToken(token), expires_in = TokenProviderOptions.Expiration.TotalSeconds });
        }

        partial void OnChangePassword(ApplicationUser user, string newPassword);

        [Authorize(AuthenticationSchemes="Bearer")]
        [HttpPost("/auth/change-password")]
        public async Task<IActionResult> ChangePassword([FromBody]JObject data)
        {
            var oldPassword = data.GetValue("OldPassword", StringComparison.OrdinalIgnoreCase);
            var newPassword = data.GetValue("NewPassword", StringComparison.OrdinalIgnoreCase);

            if (oldPassword == null || newPassword == null)
            {
                return Error("Invalid old or new password.");
            }

            var id = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await userManager.FindByIdAsync(id);

            OnChangePassword(user, newPassword.ToObject<string>());

            var result = await userManager.ChangePasswordAsync(user, oldPassword.ToObject<string>(), newPassword.ToObject<string>());

            if (!result.Succeeded)
            {
                var message = string.Join(", ", result.Errors.Select(error => error.Description));

                return Error(message);
            }

            return new NoContentResult();
        }

        internal static async System.Threading.Tasks.Task AddTokenToHeader(Microsoft.AspNetCore.Http.HttpContext context)
        {
            if(context.User.Identity.IsAuthenticated)
            {
                var userManager = (Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>)context.RequestServices
                    .GetService(typeof(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>));

                var signInManager = (Microsoft.AspNetCore.Identity.SignInManager<ApplicationUser>)context.RequestServices
                    .GetService(typeof(Microsoft.AspNetCore.Identity.SignInManager<ApplicationUser>));

                var nameIdentifier = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if(nameIdentifier != null)
                {
                    var id = nameIdentifier.Value;
                    var user = await userManager.FindByIdAsync(id);

                    var principal = await signInManager.CreateUserPrincipalAsync(user);
                    
                    var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

                    var token = handler.CreateToken(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
                    {
                        Issuer = Authentication.TokenProviderOptions.Issuer,
                        Audience = Authentication.TokenProviderOptions.Audience,
                        SigningCredentials = Authentication.TokenProviderOptions.SigningCredentials,
                        Subject = new System.Security.Claims.ClaimsIdentity(principal.Claims),
                        Expires = DateTime.UtcNow.Add(Authentication.TokenProviderOptions.Expiration)
                    });

                    context.Response.Headers.Add("Access-Control-Expose-Headers", "access_token");
                    context.Response.Headers.Add("access_token", handler.WriteToken(token));
                }
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]JObject data)
        {
            var username = data.GetValue("UserName", StringComparison.OrdinalIgnoreCase);
            var password = data.GetValue("Password", StringComparison.OrdinalIgnoreCase);

            if (username == null || password == null)
            {
                return Error("Invalid user name or password.");
            }

            if (env.EnvironmentName == "Development" && username.ToObject<string>() == "admin" && password.ToObject<string>() == "admin")
            {
                var claims = new List<Claim>() {
                        new Claim(ClaimTypes.Name, "admin"),
                        new Claim(ClaimTypes.Email, "admin")
                      };

                this.roleManager.Roles.ToList().ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r.Name)));

                return Jwt(claims);
            }

            var user = await userManager.FindByNameAsync(username.ToObject<string>());

            if (user == null)
            {
                return Error("Invalid user name or password.");
            }

            var validPassword = await userManager.CheckPasswordAsync(user, password.ToObject<string>());

            if (!validPassword)
            {
                return Error("Invalid user name or password.");
            }
            var principal = await signInManager.CreateUserPrincipalAsync(user);

            return Jwt(principal.Claims);
        }

        partial void OnUserCreated(ApplicationUser user);

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]JObject data)
        {
            var email = data.GetValue("Email", StringComparison.OrdinalIgnoreCase);
            var password = data.GetValue("Password", StringComparison.OrdinalIgnoreCase);

            if (email == null || password == null)
            {
                return Error("Invalid email or password.");
            }

            var user = new ApplicationUser { UserName = email.ToObject<string>(), Email = email.ToObject<string>() };

            EntityPatch.Apply(user, data);

            OnUserCreated(user);

            var result = await userManager.CreateAsync(user, password.ToObject<string>());

            if (result.Succeeded)
            {
                return Created($"auth/Users('{user.Id}')", user);
            }
            else
            {
                return IdentityError(result);
            }
        }

        private IActionResult IdentityError(IdentityResult result)
        {
            var message = string.Join(", ", result.Errors.Select(error => error.Description));

            return BadRequest(new { error = new { message } });
        }

    }
}
