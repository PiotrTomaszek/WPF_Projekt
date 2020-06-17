using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace P4_Projekt_2
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private theBankVievModel _theBankVievModel;

        public LoginWindow(theBankVievModel theBankVievModel)
        {
            InitializeComponent();
            _theBankVievModel = theBankVievModel;
            DataContext = _theBankVievModel;
        }

        // Logowanie do aplikacji
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Login.Text.Equals(_theBankVievModel.Login) && Password.Password.Equals(_theBankVievModel.Alias))
            {
                var mainWindow = new MainWindow(_theBankVievModel);
                mainWindow.Show();
                Close();
            }
            else
                MessageBox.Show("Błędne dane logowania.", "theBank - błąd");
        }

        // Zamknięcie aplikacji
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
