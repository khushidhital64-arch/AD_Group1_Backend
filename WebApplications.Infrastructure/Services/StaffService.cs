using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplications.Application.DTOs;
using WebApplications.Application.Interfaces.IServices;
using WebApplications.Domain.Models;

namespace WebApplications.Infrastructure.Services
{
    public class StaffService : IStaffService
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<Roles> _roleManager;

        public StaffService(UserManager<Users> userManager, RoleManager<Roles> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<StaffDto>> GetAllStaffAsync()
        {
            var staffUsers = await _userManager.GetUsersInRoleAsync("Staff");
            return staffUsers.Select(u => new StaffDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email!,
                Role = "Staff"
            }).ToList();
        }

        public async Task<StaffDto?> GetStaffByIdAsync(long id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Staff")) return null;

            return new StaffDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                Role = "Staff"
            };
        }

        public async Task<bool> RegisterStaffAsync(RegisterUserDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null) return false;

            var user = new Users
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return false;

            if (!await _roleManager.RoleExistsAsync("Staff"))
            {
                await _roleManager.CreateAsync(new Roles { Name = "Staff" });
            }

            await _userManager.AddToRoleAsync(user, "Staff");
            return true;
        }

        public async Task<bool> UpdateStaffRoleAsync(long id, string newRole)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!await _roleManager.RoleExistsAsync(newRole))
            {
                await _roleManager.CreateAsync(new Roles { Name = newRole });
            }

            var result = await _userManager.AddToRoleAsync(user, newRole);
            return result.Succeeded;
        }

        public async Task<bool> DeleteStaffAsync(long id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}
