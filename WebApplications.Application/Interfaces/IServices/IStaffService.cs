using WebApplications.Application.DTOs;

namespace WebApplications.Application.Interfaces.IServices
{
    public interface IStaffService
    {
        Task<List<StaffDto>> GetAllStaffAsync();
        Task<StaffDto?> GetStaffByIdAsync(long id);
        Task<bool> RegisterStaffAsync(RegisterUserDto dto);
        Task<bool> UpdateStaffRoleAsync(long id, string newRole);
        Task<bool> DeleteStaffAsync(long id);
    }
}
