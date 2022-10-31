using System;
using System.Collections.Generic;
using MyApplicatioon.Services;
using MyApplicatioon.Interfaces;
using MyApplicatioon.Data;
using MyApplicatioon.Constants;
using Microsoft.AspNetCore.Mvc;

namespace MyApplicatioon.Controllers
{
    [ApiController]
    [Route("api/v1/stages")]
    public class StagesController : ControllerBase
    {
        private readonly DataContext _context;

        public StagesController(DataContext context)
        {
            _context = context;
        }
        // creating stages in customers stages
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] StagesBody model)
        {
            try
            {
                MyApplicatioon.Models.CustomerStages customerStages = new MyApplicatioon.Models.CustomerStages()
                {
                    Id = Guid.NewGuid(),
                    CustomerStage = 1
                };
                await _context.CustomerStage.AddAsync(customerStages);
                await _context.SaveChangesAsync();
                return Ok(new Response
                {
                    Status = "Success",
                    Message = "stage Created sucessfull"
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
    }
}