namespace StocksApp.WPF.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using StocksApp.Core.Models;
    using StocksApp.Core.Services;
    using StocksApp.WPF.Views;
    using Ninject;


    public class MainViewModel : BaseViewModel
    {
        private readonly IStockService _stockService;
        private readonly IKernel _kernel;
        private Stock _selectedStock;
        private bool _isLoading;

        public MainViewModel(IStockService stockService, IKernel kernel)
        {
            _stockService = stockService;
            _kernel = kernel;
            Stocks = new ObservableCollection<Stock>();
            RefreshCommand = new RelayCommand(async _ => await RefreshStocksAsync());
            ViewStockDetailsCommand = new RelayCommand(ViewStockDetails, _ => SelectedStock != null);
            
            // Load stocks on startup
            Task.Run(async () => await RefreshStocksAsync());

            // ApiEvents.RefreshRequested += async (sender, args) => 
            // {
            //     await Application.Current.Dispatcher.InvokeAsync(async () => 
            //     {
            //         await RefreshStocksAsync();
            //     });
            // };
        }

        public ObservableCollection<Stock> Stocks { get; private set; }
        
        public Stock SelectedStock
        {
            get => _selectedStock;
            set
            {
                SetProperty(ref _selectedStock, value);
                (ViewStockDetailsCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand ViewStockDetailsCommand { get; }

        private async Task RefreshStocksAsync()
        {
            IsLoading = true;
            try
            {
                var stocks = await _stockService.GetStocksAsync();
                
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Stocks.Clear();
                    foreach (var stock in stocks)
                    {
                        Stocks.Add(stock);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading stocks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ViewStockDetails(object parameter)
        {
            if (SelectedStock == null) return;

            var detailViewModel = _kernel.Get<StockDetailViewModel>();
            detailViewModel.LoadStock(SelectedStock.Symbol);
            
            var detailWindow = new StockDetailWindow { DataContext = detailViewModel };
            detailWindow.Owner = Application.Current.MainWindow;
            detailWindow.ShowDialog();
        }
    }

    // Simple Command implementation
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}