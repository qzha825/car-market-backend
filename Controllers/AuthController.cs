using Microsoft.AspNetCore.Mvc;
using CarMarketBackend.Data;
using CarMarketBackend.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarMarketBackend.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == login.Username);
        if (user == null) return Unauthorized("User not found");

        if (!BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            return Unauthorized("Password incorrect");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
           claims: new[] {
               new Claim("id", user.Id.ToString()),
               new Claim("role", user.Role)
           },
           expires: DateTime.Now.AddHours(2),
           signingCredentials: creds
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            username = user.Username,
            role = user.Role
        });
    }


    [HttpPost("test")]
    public async Task<IActionResult> Test([FromBody] User login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == login.Username);
        if (user == null) return Unauthorized("User not found");

        var hashed = BCrypt.Net.BCrypt.HashPassword("admin123");
        Console.WriteLine(hashed);


        return Ok(new
        {
            username = user.Username,
            role = user.Role
        });
    }
    
    [HttpPost("health")]
    public async Task<IActionResult> Health([FromBody] User login)
    {

        return Ok("ok");
    }
}
