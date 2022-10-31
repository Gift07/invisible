using System;
using MyApplicatioon.Data;
using MyApplicatioon.Services;
using MyApplicatioon.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyApplicatioon.Controllers
{
    [ApiController]
    [Route("api/v1/company")]
    public class CompanyController : ControllerBase
    {
        private readonly DataContext _context;
        public CompanyController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> AddBusiness([FromBody] CompanyBody model)
        {
            try
            {
                var existingCompany = await _context.Company.FirstOrDefaultAsync(
                    x => x.CompanyName == model.CompanyName
                    );
                if (existingCompany != null)
                    return StatusCode(
                        StatusCodes.Status400BadRequest,
                        new Response
                        {
                            Status = "Failed",
                            Message = "Company  already exists"
                        });

                CompanyModel company = new CompanyModel
                {
                    Id = Guid.NewGuid(),
                    CompanyName = model.CompanyName,
                    CompanyLogo = model.CompanyLogo,
                    RegistrationNumber = model.RegistrationNumber
                };

                await _context.Company.AddAsync(company);
                await _context.SaveChangesAsync();

                return Ok(new Response
                {
                    Status = "Success",
                    Message = "Company created successfull"
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
        // Getting the company
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<CompanyModel>>> Getall()
        {
            try
            {
                var result = await _context.Company.ToListAsync();
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