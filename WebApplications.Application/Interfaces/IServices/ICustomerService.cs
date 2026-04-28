using WebApplications.Application.DTOs;
using WebApplications.Domain.Models;

namespace WebApplications.Application.Interfaces.IServices
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(long id);
        Task<object?> GetCustomerDetailsWithHistoryAsync(long id);
        Task<Customer> RegisterCustomerWithVehicleAsync(RegisterCustomerDto dto);
    }
}