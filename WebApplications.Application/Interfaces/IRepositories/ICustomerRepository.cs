using WebApplications.Application.DTOs;
using WebApplications.Domain.Models;

namespace WebApplications.Application.Interfaces.IRepositories
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(long id);
        Task<object?> GetCustomerDetailsWithHistoryAsync(long id);
        Task<Customer> RegisterCustomerWithVehicleAsync(RegisterCustomerDto dto);
    }
}