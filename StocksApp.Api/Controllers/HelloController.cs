using Microsoft.AspNetCore.Mvc;
using StocksApp.Api.Services;
using System.Threading.Tasks;

namespace StocksApp.Api.Controllers
{
    public class HelloController : ControllerBase
{
        [HttpGet("")]
                public IActionResult GetHello()
                {
                    // Simulate user clicking the refresh button
                
                    return Ok(new { message = "Hello from API" });
                }
}}