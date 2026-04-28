using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplications.Application.DTOs;
using WebApplications.Application.Interfaces.IServices;

namespace WebApplications.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/staff")]
    [ApiController]
    public class AdminStaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public AdminStaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStaff()
        {
            var staff = await _staffService.GetAllStaffAsync();
            return Ok(staff);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaffById(long id)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);
            if (staff == null) return NotFound("Staff member not found");
            return Ok(staff);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterStaff(RegisterUserDto dto)
        {
            var result = await _staffService.RegisterStaffAsync(dto);
            if (!result) return BadRequest("Failed to register staff. Email may already be in use.");
            return Ok("Staff registered successfully.");
        }

        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateStaffRole(long id, [FromBody] string newRole)
        {
            var result = await _staffService.UpdateStaffRoleAsync(id, newRole);
            if (!result) return BadRequest("Failed to update staff role.");
            return Ok("Staff role updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(long id)
        {
            var result = await _staffService.DeleteStaffAsync(id);
            if (!result) return BadRequest("Failed to delete staff.");
            return Ok("Staff deleted successfully.");
        }
    }
}
