using System.Windows;
using System.Windows.Controls;

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
            this.viewModel = new ViewModel(this.Dispatcher);
            this.DataContext = this.viewModel;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await this.viewModel.Initialize();
        }

        private void TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter)
            {
                return;
            }
            
            TextBox commandTextBox = (TextBox)sender;
            this.viewModel.ClientRequestCommand.Execute(commandTextBox.Text);
            commandTextBox.Clear();
        }
    }
}
