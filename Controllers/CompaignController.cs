using MyApplicatioon.Data;
using Microsoft.AspNetCore.Mvc;
using MyApplicatioon.Services;
using MyApplicatioon.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace MyApplicatioon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaignController : ControllerBase
    {
        private readonly DataContext _context;
        public CompaignController(DataContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CompaignBody body)
        {
            try
            {
                var ExistingCompaign = await _context.Compaign.FirstOrDefaultAsync(
                    x => x.Name == body.Name);
                if (ExistingCompaign != null)
                    return StatusCode(
                   StatusCodes.Status400BadRequest,
                   new Response
                   {
                       Status = "Failed",
                       Message = "Compaign with that name already exists"
                   });
                // compaign constructor
                CompainModel compain = new CompainModel()
                {
                    Id = Guid.NewGuid(),
                    Name = body.Name,
                    StartDate = body.StartDate,
                    EndDate = body.EndDate,
                    Messages = body.Messages,
                    TotalCustomers = body.TotalCustomers,
                    HasEnded = false,
                    ClickThroughRate = 0.0,
                    ClickToOpenRate = 0.0,
                    ComplaintRate = 0.0,
                    ConversionRate = 0.00,
                    DeliveredEmail = new List<Guid> { },
                    OpenRates = 0.00,
                    UnsubscribeRate = 0.00,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _context.Compaign.AddAsync(compain);
                await _context.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Compaign created Successfull" });
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
        // getting all compaings
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<CompainModel>>> GetAll()
        {
            try
            {
                var result = await _context.Compaign.ToListAsync();
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
        // Getting single Company
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CompainModel>> Get([FromRoute] Guid Id)
        {
            try
            {
                var result = await _context.Compaign.FirstOrDefaultAsync(x => x.Id == Id);
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
        // addding email openers
        [HttpPatch]
        [Route("email/{compain}/{id}")]
        public async Task<IActionResult> EmailReaders([FromRoute] Guid Id, [FromQuery] Guid compain)
        {
            try
            {
                var result = await _context.Compaign.FindAsync(compain);
                if (result != null)
                    return BadRequest("compain doesnot exist");

                result.DeliveredEmail.Add(Id);
                await _context.SaveChangesAsync();

                return Ok(new Response
                {
                    Status = "Success",
                    Message = "Email viewer added successfull"
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
        // total clicks
        // addding email openers
        [HttpPatch]
        [Route("clicks/{compain}/{id}")]
        public async Task<IActionResult> EmailClicks([FromRoute] Guid Id, [FromQuery] Guid compain)
        {
            try
            {
                var result = await _context.Compaign.FindAsync(compain);
                if (result != null)
                    return BadRequest("compain doesnot exist");

                result.TotalClicks.Add(Id);
                await _context.SaveChangesAsync();

                return Ok(new Response
                {
                    Status = "Success",
                    Message = "Email viewer added successfull"
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
        // click rate
        // addding email openers
        [HttpPatch]
        [Route("clickc/rate")]
        public async Task<IActionResult> Click([FromRoute] Guid Id, [FromQuery] Guid compain)
        {
            try
            {
                var result = await _context.Compaign.FindAsync(compain);
                if (result != null)
                    return BadRequest("compain doesnot exist");

                int clickRate = (result.TotalClicks.Count / result.DeliveredEmail.Count);
                result.ClickThroughRate = clickRate;

                await _context.SaveChangesAsync();

                return Ok(new Response
                {
                    Status = "Success",
                    Message = "Email viewer added successfull"
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