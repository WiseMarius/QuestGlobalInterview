using Client.ViewModels;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowsViewModel _viewModel = new MainWindowsViewModel();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = _viewModel;
        }
    }
}
