using Microsoft.EntityFrameworkCore;
using WebApplications.Application.DTOs;
using WebApplications.Application.Interfaces.IRepositories;
using WebApplications.Domain.Models;
using WebApplications.Infrastructure.Presistance;

namespace WebApplications.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Customer>> GetAllCustomersAsync()
        {
            return _context.Customers.ToListAsync();
        }

        public Task<Customer?> GetCustomerByIdAsync(long id)
        {
            return _context.Customers.FindAsync(id).AsTask();
        }

        public async Task<object?> GetCustomerDetailsWithHistoryAsync(long id)
        {
            return await _context.Customers
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.FullName,
                    c.Phone,
                    c.Email,
                    Vehicles = _context.Vehicles.Where(v => v.CustomerId == c.Id).ToList(),
                    Invoices = _context.SalesInvoices.Where(i => i.CustomerId == c.Id).ToList(),
                    Appointments = _context.ServiceAppointments.Where(a => a.CustomerId == c.Id).ToList(),
                    PartRequests = _context.PartRequests.Where(p => p.CustomerId == c.Id).ToList(),
                    Reviews = _context.ServiceReviews.Where(r => r.CustomerId == c.Id).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Customer> RegisterCustomerWithVehicleAsync(RegisterCustomerDto dto)
        {
            var customer = new Customer
            {
                FullName = dto.FullName,
                Phone = dto.Phone,
                Email = dto.Email
            };

            var vehicle = new Vehicle
            {
                VehicleNumber = dto.VehicleNumber,
                VehicleModel = dto.VehicleModel,
                VehicleBrand = dto.VehicleBrand,
                Customer = customer
            };

            _context.Customers.Add(customer);
            _context.Vehicles.Add(vehicle);

            await _context.SaveChangesAsync();

            return customer;
        }
    }
}