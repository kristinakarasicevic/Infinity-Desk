using InfinityDesk.Api.Data;
using InfinityDesk.Api.DTOs;
using InfinityDesk.Api.Models;
using InfinityDesk.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InfinityDesk.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly TokenService tokenService;

        public AuthController(AppDbContext context, TokenService tokenService)
        {
            this.context = context;
            this.tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
        {
            var existingUser = await this.context.Users
                .FirstOrDefaultAsync(u => u.Username == registerDto.Username);
            if (existingUser != null) //ako je pronadjen korisnik sa istim usernameom, vrati error
            {
                return BadRequest("Username already exists");
            }

            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = registerDto.Password //ZA SAD BEZ HESOVANJA
            };

            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();

            return Ok("User created");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            var user = await this.context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || user.PasswordHash != request.Password)
            {
                return BadRequest("Invalid username or password");
            }

            var token = this.tokenService.CreateToken(user);

            return Ok(new
            {
                token
            });
        }
    }
}