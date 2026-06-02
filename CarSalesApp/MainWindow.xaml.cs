using CarSalesApp.Models;
using CarSalesApp.ViewModels;
using Microsoft.Win32;
using System.Windows;
using System.Xml.Linq;

namespace CarSalesApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}