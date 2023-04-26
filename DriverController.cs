using Hangfire;
using HangFireWebApi.Models;
using HangFireWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HangFireWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly ILogger<DriverController> _logger;
        private static List<Driver> _drivers = new List<Driver>();

        public DriverController(ILogger<DriverController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult AddDriver(Driver driver)
        {
            if (ModelState.IsValid)
            {
                _drivers.Add(driver);
                var jobId = BackgroundJob.Enqueue<IServiceManagement>(x=>x.SendEmail());
                return CreatedAtAction("GetDriver",new {driver.Id}, driver);
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult GetDriver(Guid id)
        {
            var driver = _drivers.FirstOrDefault(x=>x.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return Ok(driver);
        }


        [HttpDelete]
        public IActionResult DeleteDriver(Guid id)
        {
            var driver = _drivers.FirstOrDefault(x => x.Id == id);
            if (driver == null)
            {
                return NotFound();
            }
            driver.Status = 0;
            RecurringJob.AddOrUpdate<IServiceManagement>(x => x.UpdateDatabase(),Cron.Hourly);
            return NoContent();
        }
    }
}
