using WebApplications.Application.DTOs;
using WebApplications.Application.Interfaces.IRepositories;
using WebApplications.Application.Interfaces.IServices;
using WebApplications.Domain.Models;

namespace WebApplications.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Task<List<Customer>> GetAllCustomersAsync()
        {
            return _customerRepository.GetAllCustomersAsync();
        }

        public Task<Customer?> GetCustomerByIdAsync(long id)
        {
            return _customerRepository.GetCustomerByIdAsync(id);
        }

        public Task<object?> GetCustomerDetailsWithHistoryAsync(long id)
        {
            return _customerRepository.GetCustomerDetailsWithHistoryAsync(id);
        }

        public Task<Customer> RegisterCustomerWithVehicleAsync(RegisterCustomerDto dto)
        {
            return _customerRepository.RegisterCustomerWithVehicleAsync(dto);
        }
    }
}