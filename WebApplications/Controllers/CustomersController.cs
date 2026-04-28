using Microsoft.AspNetCore.Mvc;
using WebApplications.Application.DTOs;
using WebApplications.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;

namespace WebApplications.Controllers
{
    [Authorize(Roles = "Staff")]
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCustomer(RegisterCustomerDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                return BadRequest("Customer name is required.");

            if (string.IsNullOrWhiteSpace(dto.Phone))
                return BadRequest("Phone number is required.");

            if (string.IsNullOrWhiteSpace(dto.VehicleNumber))
                return BadRequest("Vehicle number is required.");

            var customer = await _customerService.RegisterCustomerWithVehicleAsync(dto);

            return Ok(new
            {
                message = "Customer and vehicle registered successfully.",
                customerId = customer.Id,
                customer.FullName,
                customer.Phone,
                customer.Email
            });
        }
    }
}