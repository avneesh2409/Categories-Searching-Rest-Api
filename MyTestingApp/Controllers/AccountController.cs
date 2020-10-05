using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyTestingApp.Models;
using MyTestingApp.Utilities;
using MyTestingApp.ViewModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyTestingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly AppSetting jwtBearerTokenSettings;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<IdentityUser> loginManager;

        public AccountController(IOptions<AppSetting> jwtTokenOptions, UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager,SignInManager<IdentityUser> loginManager) { 
             this.jwtBearerTokenSettings = jwtTokenOptions.Value;
             this.userManager = userManager;
            this.roleManager = roleManager;
            this.loginManager = loginManager;
        }
        [HttpGet]
        [Route("role")]
        public async Task<IActionResult> GetRoles() {

            return Ok(await roleManager.Roles.ToListAsync());
        }
        [HttpPost]
        [Route("role")]
        public async Task<IActionResult> CreateRole(Role user)
        {
            if (!ModelState.IsValid || user == null) { return BadRequest(); }
            var roleCheck = await roleManager.RoleExistsAsync(user.Name);
            if (!roleCheck)
            {
                //create the roles and seed them to the database  
                IdentityRole role = new IdentityRole(user.Name);
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Ok(new { Message = "Role Created" });
                }
                return BadRequest(new { Message = "Role already Exists" });
            }
           
            return BadRequest();
        }
        [HttpPost]
        [Route("user-role")]
        public async Task<IActionResult> CreateUserRole(UserRole userRole)
        {
            var roleCheck = await roleManager.RoleExistsAsync(userRole.RoleName);
            if (!roleCheck)
            { 
                await roleManager.CreateAsync(new IdentityRole(userRole.RoleName));
            }
            IdentityUser user = await userManager.FindByEmailAsync(userRole.Email);
            if (user != null) {
                var IdentityResult = await userManager.AddToRoleAsync(user, userRole.RoleName);
                return Ok(IdentityResult);
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("identity-login")]
        public async Task<bool> IdentityLogin(IdentityUser user) {
            await loginManager.SignInAsync(user,false);
            return true;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid || user == null) { return new BadRequestObjectResult(new { Message = "User Registration Failed" }); }
            var identityUser = new IdentityUser() { UserName = user.UserName, Email = user.Email };
            var result = await userManager.CreateAsync(identityUser, user.Password);
            if (!result.Succeeded)
            {
                var dictionary = new ModelStateDictionary(); 
                foreach (IdentityError error in result.Errors) {
                     dictionary.AddModelError(error.Code, error.Description);
                }
                return new BadRequestObjectResult(new { Message = "User Registration Failed", Errors = dictionary });
            }
            return Ok(new { Message = "User Reigstration Successful" });
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(Login credentials)
        {
            IdentityUser identityUser;
            if (credentials == null) 
            { 
                return new BadRequestObjectResult(new { Message = "Login failed" });
            }
            identityUser = await ValidateUser(credentials);
            if (identityUser != null) {
                var token = GenerateJSONWebToken(identityUser);
                return Ok(new { Token = token, Message = "Success" });
            }
            return BadRequest(new { message = "user not found !!"});
        }
      private async Task<IdentityUser> ValidateUser(Login credentials)
            {
                var identityUser = await userManager.FindByEmailAsync(credentials.Email);
                if (identityUser != null) {
                     var result = userManager.PasswordHasher.VerifyHashedPassword(identityUser,
                      identityUser.PasswordHash,
                       credentials.Password
                    );
                    return result == PasswordVerificationResult.Failed ? null : identityUser;
                }
                return null;
            }

        #region Token Authentication

        public ObjectResult GenerateJSONWebToken(IdentityUser userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtBearerTokenSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            Claim[] claims = new Claim[] {
                new Claim("email", userInfo.Email),
                new Claim("username",userInfo.UserName)
            };
            var token = new JwtSecurityToken(jwtBearerTokenSettings.Issuer,
              jwtBearerTokenSettings.Issuer,
              claims,
              expires: DateTime.Now.AddMinutes(60),
              signingCredentials: credentials);

            return new ObjectResult(new { status = true, token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        #endregion
    }
}
