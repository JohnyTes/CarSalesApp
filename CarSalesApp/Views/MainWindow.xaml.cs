using CarSalesApp.ViewModels;
using System.Windows;

namespace CarSalesApp
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}