using CarSalesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarSalesApp
{
    /// <summary>
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        public DetailWindow(DetailWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
