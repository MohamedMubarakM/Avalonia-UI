using Avalonia.Controls;
using Avalonia1.ViewModels;

namespace Avalonia1.Views
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
