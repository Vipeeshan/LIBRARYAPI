using LibraryAPI.Models;
using LibraryAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Librarian> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<Librarian> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // Only authenticated Librarians can register new librarians
        [Authorize(Roles = "Librarian")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LibrarianRegisterDTO model)
        {
            if (model == null)
                return BadRequest("Request body cannot be empty.");

            if (string.IsNullOrWhiteSpace(model.Username))
                return BadRequest("Username is required.");

            if (string.IsNullOrWhiteSpace(model.Email))
                return BadRequest("Email is required.");

            if (string.IsNullOrWhiteSpace(model.Password))
                return BadRequest("Password is required.");

            var user = new Librarian
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Optionally assign Librarian role here if needed
                await _userManager.AddToRoleAsync(user, "Librarian");

                return Ok("Librarian registered successfully!");
            }

            return BadRequest(result.Errors);
        }

        // Anyone can login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LibrarianLoginDTO model)
        {
            if (model == null)
                return BadRequest("Request body cannot be empty.");

            if (string.IsNullOrWhiteSpace(model.Username))
                return BadRequest("Username is required.");

            if (string.IsNullOrWhiteSpace(model.Password))
                return BadRequest("Password is required.");

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Role, "Librarian"), // add role claim
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized("Invalid credentials");
        }
    }
}
