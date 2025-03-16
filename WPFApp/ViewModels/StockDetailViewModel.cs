namespace StocksApp.WPF.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using StocksApp.Core.Models;
    using StocksApp.Core.Services;

    public class StockDetailViewModel : BaseViewModel
    {
        private readonly IStockService _stockService;
        private Stock _stock;
        private bool _isLoading;

        public StockDetailViewModel(IStockService stockService)
        {
            _stockService = stockService;
            RefreshCommand = new RelayCommand(async _ => await LoadStockAsync());
            CloseCommand = new RelayCommand(_ => RequestClose?.Invoke());
        }

        public Stock Stock
        {
            get => _stock;
            set => SetProperty(ref _stock, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand CloseCommand { get; }
        
        public event Action RequestClose;

        public async void LoadStock(string symbol)
        {
            await LoadStockAsync(symbol);
        }

        private async Task LoadStockAsync(string symbol = null)
        {
            if (string.IsNullOrEmpty(symbol) && Stock == null)
                return;

            symbol = symbol ?? Stock?.Symbol;
            if (string.IsNullOrEmpty(symbol))
                return;

            IsLoading = true;
            try
            {
                Stock = await _stockService.GetStockBySymbolAsync(symbol);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading stock details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}