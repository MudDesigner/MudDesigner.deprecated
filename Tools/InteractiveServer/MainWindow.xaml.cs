using System.Windows;

namespace InteractiveServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.viewModel = new ViewModel();
            this.DataContext = this.viewModel;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await this.viewModel.Initialize();
        }
    }
}
