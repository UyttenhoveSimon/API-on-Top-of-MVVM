using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace StocksApp.Core.Services
{
    public class WpfInteractionClient : IDisposable
    {
        private readonly HubConnection _hubConnection;
        
        // Events to notify the UI of user interaction simulations
        public event EventHandler RefreshRequested;
        public event EventHandler<string> StockSelectionRequested;
        public event EventHandler<string> ViewDetailsRequested;
        public event EventHandler CloseDetailsRequested;

        public WpfInteractionClient(string hubUrl = "https://localhost:7001/interactionhub")
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                .Build();

            // Set up event handlers
            _hubConnection.On("RefreshStocks", () =>
            {
                RefreshRequested?.Invoke(this, EventArgs.Empty);
            });

            _hubConnection.On<string>("SelectStock", (symbol) =>
            {
                StockSelectionRequested?.Invoke(this, symbol);
            });

            _hubConnection.On<string>("ViewStockDetails", (symbol) =>
            {
                ViewDetailsRequested?.Invoke(this, symbol);
            });

            _hubConnection.On("CloseStockDetails", () =>
            {
                CloseDetailsRequested?.Invoke(this, EventArgs.Empty);
            });
        }

        public async Task ConnectAsync()
        {
            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine("Connected to WPF interaction hub");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to WPF interaction hub: {ex.Message}");
                // Consider retrying or notifying the user
            }
        }

        public bool IsConnected => _hubConnection.State == HubConnectionState.Connected;

        public void Dispose()
        {
            _hubConnection?.DisposeAsync().GetAwaiter().GetResult();
        }
    }
}
