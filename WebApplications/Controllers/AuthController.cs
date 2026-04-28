using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplications.Application.DTOs;
using WebApplications.Domain.Models;
using WebApplications.Infrastructure.Helpers;

namespace WebApplications.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<Roles> _roleManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly JwtHelper _jwtHelper;

        public AuthController(
            UserManager<Users> userManager,
            RoleManager<Roles> roleManager,
            SignInManager<Users> signInManager,
            JwtHelper jwtHelper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("Email is required.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Password is required.");

            if (string.IsNullOrWhiteSpace(dto.FullName))
                return BadRequest("Full name is required.");

            var allowedRoles = new[] { "Admin", "Staff", "Customer" };

            if (!allowedRoles.Contains(dto.Role))
                return BadRequest("Invalid role.");

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser != null)
                return BadRequest("User already exists with this email.");

            var user = new Users
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (!await _roleManager.RoleExistsAsync(dto.Role))
            {
                await _roleManager.CreateAsync(new Roles
                {
                    Name = dto.Role
                });
            }

            await _userManager.AddToRoleAsync(user, dto.Role);

            return Ok(new
            {
                message = "User registered successfully.",
                userId = user.Id,
                user.FullName,
                user.Email,
                role = dto.Role
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return Unauthorized("Invalid email or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded)
                return Unauthorized("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Customer";

            var token = _jwtHelper.GenerateToken(user, role);

            return Ok(new
            {
                message = "Login successful.",
                token,
                user = new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    role
                }
            });
        }
    }
}