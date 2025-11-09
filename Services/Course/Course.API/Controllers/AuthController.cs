using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Course.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IConfiguration cfg) : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, req.UserId.ToString()),
            new Claim(ClaimTypes.Role, req.Role)
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!));
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return Ok(new { access_token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
    public record LoginRequest(int UserId, string Role);
}
