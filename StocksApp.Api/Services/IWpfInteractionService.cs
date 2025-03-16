using System.Threading.Tasks;

namespace StocksApp.Api.Services
{
    public interface IWpfInteractionService
    {
        Task RefreshStocksAsync();
        Task ViewStockDetailsAsync(string symbol);
        Task CloseStockDetailsAsync();
        Task SelectStockAsync(string symbol);
    }
}