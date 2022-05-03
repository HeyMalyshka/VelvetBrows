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
using System.Data.SqlClient;
using System.Data;
using VelvetBrows.Models;
using System.Data.Entity;


namespace VelvetBrows
{
    /// <summary>
    /// Логика взаимодействия для BlizZapis.xaml
    /// </summary>
    public partial class BlizZapis : Page
    {
        //подключение к бд и объявление переменных
        static String connectionString = "server=LAPTOP-GFFVEP2R; Initial Catalog=VelvetBrows;Integrated Security=SSPI";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;

        public BlizZapis()
        {
            InitializeComponent();

            System.Timers.Timer timer = new System.Timers.Timer(30000);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timerTick);
            timer.Start();
            FillList();
        }

        private void MenuItemHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainWindow.xaml", UriKind.Relative));

        }

        private void MenuItemAdmin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FormAdmin.xaml", UriKind.Relative));

        }

        private void Programm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {

        }

        //вывод данных из базы
        public void FillList()
        {
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT ClientService.ID,ClientService.ClientID, ClientService.ServiceID, ClientService.StartTime, Client.FirstName, Service.Title FROM ClientService, Client, Service WHERE Client.ID = ClientService.ClientID AND Service.ID = ClientService.ServiceID", con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "ClientService");
                Services co = new Services();
                IList<Services> co1 = new List<Services>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    co1.Add(new Services
                    {
                        ID = Convert.ToInt32(dr[0].ToString()),
                        ClientID = Convert.ToInt32(dr[1].ToString()),
                        ServiceID = Convert.ToInt32(dr[2].ToString()),
                        StartTime = Convert.ToDateTime(dr[3].ToString()),
                        Name = dr[4].ToString(),
                        Title = dr[5].ToString()
                        
                    });

                }
                lstBox.ItemsSource = co1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ds = null;
                adapter.Dispose();
                con.Close();
                con.Dispose();
            }
        }

        //обновление страницы
        private void timerTick(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                lstBox.Visibility = Visibility.Visible;
                if (lstBox.SelectedItems != null)
                    lstBox.Items.Refresh();
            });
        }

        public class Services
        {
            public int ID { get; set; }
            public int ClientID { get; set; }
            public int ServiceID { get; set; }
            public System.DateTime StartTime { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
            
        }
    }
}
