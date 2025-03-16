using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace StocksApp.Api.Services
{
    public class WpfInteractionService : IWpfInteractionService
    {
        private readonly IHubContext<WpfInteractionHub> _hubContext;

        public WpfInteractionService(IHubContext<WpfInteractionHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task RefreshStocksAsync()
        {
            await _hubContext.Clients.All.SendAsync("RefreshStocks");
        }

        public async Task ViewStockDetailsAsync(string symbol)
        {
            await _hubContext.Clients.All.SendAsync("ViewStockDetails", symbol);
        }

        public async Task CloseStockDetailsAsync()
        {
            await _hubContext.Clients.All.SendAsync("CloseStockDetails");
        }

        public async Task SelectStockAsync(string symbol)
        {
            await _hubContext.Clients.All.SendAsync("SelectStock", symbol);
        }
    }
}