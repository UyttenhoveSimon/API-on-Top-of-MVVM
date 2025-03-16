namespace StocksApp.WPF.Views
{
    using System.Windows;

    public partial class StockDetailWindow : Window
    {
        public StockDetailWindow()
        {
            InitializeComponent();
            ((App)Application.Current).RegisterDetailWindow(this);
        }
    }
}