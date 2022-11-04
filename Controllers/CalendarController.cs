using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApplicatioon.Models;
using MyApplicatioon.Interfaces;
using MyApplicatioon.Data;
using MyApplicatioon.Services;

namespace server.Controllers
{
    [ApiController]
    [Route("api/v1/calendar")]
    public class CalendarController : ControllerBase
    {
        private readonly DataContext _context;
        public CalendarController(DataContext context)
        {
            _context = context;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CalendarBody body)
        {
            try
            {
                CalendarModel calendar = new CalendarModel()
                {
                    Id = Guid.NewGuid(),
                    Event = body.Event,
                    Start = body.Start,
                    End = body.End,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _context.Calendar.AddAsync(calendar);
                await _context.SaveChangesAsync();
                return Ok("Calendar created succsesful");
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
        [HttpGet("all")]
        public async Task<ActionResult<List<CalendarModel>>> GetAll()
        {
            try
            {
                var results = await _context.Calendar.ToListAsync();
                return Ok(results);
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
        [HttpGet("{id}")]
        public async Task<ActionResult<CalendarModel>> GetId([FromRoute] Guid id)
        {
            try
            {
                var result = await _context.Calendar.FirstOrDefaultAsync(x => x.Id == id);
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
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] CalendarBody body)
        {
            try
            {
                var result = await _context.Calendar.FirstOrDefaultAsync(x => x.Id == id);
                result.Start = body.Start != null ? body.Start : result.Start;
                result.End = body.End != null ? body.End : result.End;
                result.Event = body.Event != null ? body.Event : result.Event;
                result.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok("Updated successfully");
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