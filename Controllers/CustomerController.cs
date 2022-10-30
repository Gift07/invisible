using Microsoft.AspNetCore.Mvc;
using MyApplicatioon.Data;
using MyApplicatioon.Interfaces;
using System;
using MyApplicatioon.Services;
using Microsoft.EntityFrameworkCore;

namespace MyApplicatioon.Controllers
{
    [ApiController]
    [Route("api/v1/customer")]
    public class CustomerController : ControllerBase
    {
        public readonly DataContext _context;

        public CustomerController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CustomerBody model)
        {
            try
            {
                var ExistingCustomer = await _context.Customer
                .FirstOrDefaultAsync(x => x.Email == model.Email);

                if (ExistingCustomer != null)
                    return BadRequest("email already exists");

                CustomerModel customer = new CustomerModel()
                {
                    Id = Guid.NewGuid(),
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Name = model.Name,
                    CustomerStage = model.CustomerStage,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _context.Customer.AddAsync(customer);
                await _context.SaveChangesAsync();

                return Ok(new Response
                {
                    Status = "Success",
                    Message = "customer Added successfull"
                });
            }
            catch (Exception e)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Status = "Failed",
                        Message = e.Message
                    });
            }
        }
        // Getting the customer
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<CustomerModel>>> Get()
        {
            try
            {
                var result = await _context.Customer.ToListAsync();

                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Status = "Failed",
                        Message = e.Message
                    });
            }
        }

    }
}