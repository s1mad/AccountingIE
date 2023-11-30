using System.Windows;

namespace AccountingIE
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.NavigationService.Navigate(new LoginPage());
        }
    }
}