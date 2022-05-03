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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VelvetBrows
{
    /// <summary>
    /// Логика взаимодействия для FormAdmin.xaml
    /// </summary>
    public partial class FormAdmin : Page
    {
        public FormAdmin()
        {
            InitializeComponent();
            
        }

        private void ClickBack(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainWindow.xaml", UriKind.Relative));
        }

        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AdminPanel.xaml", UriKind.Relative));
        }

        //проверка пароля на правильность
        private void password_TextChanged(object sender, RoutedEventArgs e)
        {
            if (password.Password == "0000") Admin.IsEnabled = true;
            else
            {
                Admin.IsEnabled = false;
                //MessageBox.Show("Неверный пароль! Попробуйте еще раз!", "Уведомление");
            }
        }
    }
}
