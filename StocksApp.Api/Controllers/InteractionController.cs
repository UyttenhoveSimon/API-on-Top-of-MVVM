using Microsoft.AspNetCore.Mvc;
using StocksApp.Api.Services;
using System.Threading.Tasks;

namespace StocksApp.Api.Controllers
{
    [ApiController]
    [Route("api/interaction")]
    public class InteractionController : ControllerBase
    {
        private readonly IWpfInteractionService _interactionService;

        public InteractionController(IWpfInteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshStocks()
        {
            // Simulate user clicking the refresh button
            await _interactionService.RefreshStocksAsync();
            return Ok(new { message = "Refresh command sent to WPF application" });
        }

        [HttpPost("select/{symbol}")]
        public async Task<IActionResult> SelectStock(string symbol)
        {
            // Simulate user selecting a stock from the list
            await _interactionService.SelectStockAsync(symbol);
            return Ok(new { message = $"Select stock command sent for symbol: {symbol}" });
        }

        [HttpPost("view-details/{symbol}")]
        public async Task<IActionResult> ViewStockDetails(string symbol)
        {
            // Simulate user clicking view details button
            await _interactionService.ViewStockDetailsAsync(symbol);
            return Ok(new { message = $"View details command sent for symbol: {symbol}" });
        }

        [HttpPost("close-details")]
        public async Task<IActionResult> CloseStockDetails()
        {
            // Simulate user closing the details window
            await _interactionService.CloseStockDetailsAsync();
            return Ok(new { message = "Close details window command sent" });
        }
    }
}