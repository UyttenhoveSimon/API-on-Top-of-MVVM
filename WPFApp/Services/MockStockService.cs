namespace StocksApp.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using StocksApp.Core.Models;
    using StocksApp.Core.Services;

    public class MockStockService : IStockService
    {
        private readonly List<Stock> _stocks;
        private readonly Random _random;

        public MockStockService()
        {
            _random = new Random();
            _stocks = new List<Stock>
            {
                new Stock { Symbol = "AAPL", Name = "Apple Inc.", Price = 150.75m },
                new Stock { Symbol = "MSFT", Name = "Microsoft Corporation", Price = 305.22m },
                new Stock { Symbol = "GOOG", Name = "Alphabet Inc.", Price = 2750.45m },
                new Stock { Symbol = "AMZN", Name = "Amazon.com Inc.", Price = 3400.10m },
                new Stock { Symbol = "TSLA", Name = "Tesla, Inc.", Price = 730.50m },
                new Stock { Symbol = "FB", Name = "Meta Platforms, Inc.", Price = 335.80m }
            };

            // Initialize changes
            foreach (var stock in _stocks)
            {
                UpdateStockChange(stock);
            }
        }

        public Task<IEnumerable<Stock>> GetStocksAsync()
        {
            // Simulate price changes
            foreach (var stock in _stocks)
            {
                UpdateStockPrice(stock);
            }

            return Task.FromResult<IEnumerable<Stock>>(_stocks);
        }

        public Task<Stock> GetStockBySymbolAsync(string symbol)
        {
            var stock = _stocks.FirstOrDefault(s => s.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
            if (stock != null)
            {
                UpdateStockPrice(stock);
            }
            return Task.FromResult(stock);
        }

        private void UpdateStockPrice(Stock stock)
        {
            // Simulate a small price change
            decimal changeAmount = _random.Next(-100, 101) / 100.0m;
            stock.Price += changeAmount;
            
            // Ensure price is positive
            if (stock.Price < 0)
                stock.Price = 0.01m;
                
            UpdateStockChange(stock);
        }

        private void UpdateStockChange(Stock stock)
        {
            // Update change amount and percentage
            stock.Change = _random.Next(-100, 101) / 10.0m;
            stock.ChangePercent = stock.Change / stock.Price * 100;
        }
    }
}
