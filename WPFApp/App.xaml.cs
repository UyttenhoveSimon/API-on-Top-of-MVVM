namespace StocksApp.WPF
{
    using System.Windows;
    using Ninject;
    using StocksApp.Core.Services;
    using StocksApp.WPF.ViewModels;
    using StocksApp.WPF.Views;

    public partial class App : Application
    {
        private IKernel _kernel;
        private WpfInteractionClient _interactionClient;
        private MainWindow _mainWindow;
        private StockDetailWindow _detailWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            _kernel = new StandardKernel();
            ConfigureServices();
            
            // Initialize interaction client
            _interactionClient = _kernel.Get<WpfInteractionClient>();
            SetupInteractionHandlers();
            _ = _interactionClient.ConnectAsync();
            
            var mainViewModel = _kernel.Get<MainViewModel>();
            _mainWindow = new MainWindow { DataContext = mainViewModel };
            _mainWindow.Show();
        }

        private void SetupInteractionHandlers()
        {
            // Handle refresh requests
            _interactionClient.RefreshRequested += async (s, e) =>
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    if (_mainWindow?.DataContext is MainViewModel vm)
                    {
                        vm.RefreshCommand.Execute(null);
                    }
                });
            };
            
            // Handle stock selection requests
            _interactionClient.StockSelectionRequested += async (s, symbol) =>
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    if (_mainWindow?.DataContext is MainViewModel vm)
                    {
                        var stockToSelect = vm.Stocks.FirstOrDefault(stock => stock.Symbol == symbol);
                        if (stockToSelect != null)
                        {
                            vm.SelectedStock = stockToSelect;
                        }
                    }
                });
            };
            
            // Handle view details requests
            _interactionClient.ViewDetailsRequested += async (s, symbol) =>
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    if (_mainWindow?.DataContext is MainViewModel vm)
                    {
                        // First select the stock
                        var stockToSelect = vm.Stocks.FirstOrDefault(stock => stock.Symbol == symbol);
                        if (stockToSelect != null)
                        {
                            vm.SelectedStock = stockToSelect;
                            
                            // Then execute the view details command
                            if (vm.ViewStockDetailsCommand.CanExecute(null))
                            {
                                vm.ViewStockDetailsCommand.Execute(null);
                            }
                        }
                    }
                });
            };
            
            // Handle close details requests
            _interactionClient.CloseDetailsRequested += async (s, e) =>
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    _detailWindow?.Close();
                });
            };
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Dispose interaction client
            _interactionClient?.Dispose();
            
            base.OnExit(e);
        }

        private void ConfigureServices()
        {
            // Register services
            _kernel.Bind<IStockService>().To<MockStockService>().InSingletonScope();
            _kernel.Bind<WpfInteractionClient>().ToSelf().InSingletonScope();
            
            // Register view models
            _kernel.Bind<MainViewModel>().ToSelf().InSingletonScope();
            _kernel.Bind<StockDetailViewModel>().ToSelf();
        }

        // Helper method to track detail window
        public void RegisterDetailWindow(StockDetailWindow window)
        {
            _detailWindow = window;
            window.Closed += (s, e) => _detailWindow = null;
        }
    }
}