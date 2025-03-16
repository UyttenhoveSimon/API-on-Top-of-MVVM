namespace StocksApp.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using StocksApp.Core.Models;

    public interface IStockService
    {
        Task<IEnumerable<Stock>> GetStocksAsync();
        Task<Stock> GetStockBySymbolAsync(string symbol);
    }
}