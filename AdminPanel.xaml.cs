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
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Page
    {
        public AdminPanel()
        {
            InitializeComponent();
        }


        private void MenuItemHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainWindow.xaml", UriKind.Relative));

        }

        private void ZapisClient_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewClient.xaml", UriKind.Relative));

        }

        private void BlizZapis_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BlizZapis.xaml", UriKind.Relative));

        }

        private void Programm_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Данная программа предназначена для салона красоты. В которой вы можете просматривать все услуги салона, добавлять, удалять и изменять их. " +
                "Также здесь имеется фильрт по скидкам и поиск.");

        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Чтобы добавить новую услугу войдите как администратор!");

        }

        private void MenuItemAdmin_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы находитесь на странице администратора!", "Уведомление");
        }
    }
}
