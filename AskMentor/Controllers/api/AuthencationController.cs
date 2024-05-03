using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace AskMentor.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthencationController : ControllerBase
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private IConfiguration _configuration;

        public AuthencationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Registeration(User model)
        {
            ApplicationUser userExists = await this.userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return Ok(new { status = false, message = "Email already exists" });
            }
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber ?? "",
                Discriminator = "",
                Name = model.Name,
                Gender = "",
                Avt = "",
                Certification = "",
            };

            var result = await this.userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, model.Role);
                return Ok(new { status = true, message = "User created successfully" });
            }
            else
            {
                return Ok(new { status = false, message = result.Errors.FirstOrDefault().Description });
            }
        }

        // Phương thức để kiểm tra định dạng email
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin model)
        {
            if (!IsValidEmail(model.Email))
                return Ok(new { status = false, message = "Invalid email format" });
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Ok(new { status = false, message = "Invalid email or password" });

            if (!await userManager.CheckPasswordAsync(user, model.Password))
                return Ok(new { status = false, message = "Invalid email or password" });

            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.Name, user.Email),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            }

            string token = GenerateToken(authClaims);


            var claimsIdentity = new ClaimsIdentity(
                authClaims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok(new { status = true, message = "", token = token, role = userRoles });
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"]));
            var _TokenExpiryTimeInHour = Convert.ToInt64(_configuration["JWTKey:TokenExpiryTimeInHour"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWTKey:ValidIssuer"],
                Audience = _configuration["JWTKey:ValidAudience"],
                //Expires = DateTime.UtcNow.AddHours(_TokenExpiryTimeInHour),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { status = false, message = "Bạn chưa đăng nhập" });
            }
            Response.Headers.Remove("Authorization");
            return Ok(new { status = true, message = "Đã đăng xuất thành công" });
        }


        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    await roleManager.CreateAsync(new IdentityRole("Admin"));
        //    await roleManager.CreateAsync(new IdentityRole("Mentor"));
        //    await roleManager.CreateAsync(new IdentityRole("Leaner"));

        //    return Ok(new { status = true, });
        //}

        //[HttpGet]
        //public async Task<IActionResult> Add()
        //{
        //    var user = new ApplicationUser
        //    {
        //        UserName = "admin@gmail.com",
        //        Email = "admin@gmail.com",
        //        PhoneNumber = "",
        //        Discriminator = "",
        //        Name = "admin",
        //        Gender = "",
        //        Avt = "",
        //        Certification = "",
        //    };

        //    var result = await this.userManager.CreateAsync(user, "123456789Admin@");

        //    if (result.Succeeded)
        //    {
        //        await userManager.AddToRoleAsync(user, "Admin");
        //        return Ok(new { status = true, message = "User created successfully" });
        //    }
        //    else
        //    {
        //        return Ok(new { status = false, message = result.Errors.FirstOrDefault().Description });
        //    }
        //}

    }
}
