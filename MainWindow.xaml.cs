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
using System.Collections;
using System.Collections.ObjectModel;

namespace VelvetBrows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Page
    {
        //подключение к бд и объявление переменных
        static String connectionString = "server=LAPTOP-GFFVEP2R; Initial Catalog=VelvetBrows;Integrated Security=SSPI";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;

        public MainWindow()
        {
            InitializeComponent();
            FillList();
            Count.IsEnabled = false;
            Count2.IsEnabled = false;
        }

        //запрос на вывод бд в листбокс
        public void FillList()
        {
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT * FROM Service", con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "Service");
                Service co = new Service();
                IList<Service> co1 = new List<Service>();


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    co1.Add(new Service
                    {
                        ID = Convert.ToInt32(dr[0].ToString()),
                        Title = dr[1].ToString(),
                        Cost = Convert.ToDecimal(dr[2].ToString()),
                        DurationInSeconds = Convert.ToInt32(dr[3].ToString()),
                        Description = dr[4].ToString(),
                        Discount = Convert.ToDouble(dr[5].ToString()),
                        MainImagePath = Convert.ToString(dr[6].ToString())
                    });

                }
                lstBox.ItemsSource = co1;

                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT COUNT (*) FROM Service", con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "Service");
                string count = cmd.ExecuteScalar().ToString();
                Count.Text = count;
                /*Service co = new Service();
                IList<Service> co1 = new List<Service>();*/
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

        public class Service
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public decimal Cost { get; set; }
            public int DurationInSeconds { get; set; }
            public string Description { get; set; }
            public double Discount { get; set; }
            public string MainImagePath { get; set; }
        }


        private void MenuItemAdmin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FormAdmin.xaml", UriKind.Relative));

        }

        //поиск услуг по базе
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string intext = text.Text.ToString();
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT * FROM Service WHERE Title LIKE '" + intext.ToString() + "%' OR Cost LIKE '" + intext.ToString() + "%' ", con);
                adapter = new SqlDataAdapter(cmd);

                ds = new DataSet();
                adapter.Fill(ds, "Service");
                Service co = new Service();
                IList<Service> co1 = new List<Service>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    co1.Add(new Service
                    {
                        ID = Convert.ToInt32(dr[0].ToString()),
                        Title = dr[1].ToString(),
                        Cost = Convert.ToDecimal(dr[2].ToString()),
                        DurationInSeconds = Convert.ToInt32(dr[3].ToString()),
                        Description = dr[4].ToString(),
                        Discount = Convert.ToDouble(dr[5].ToString()),
                        MainImagePath = Convert.ToString(dr[6].ToString())
                    });

                }
                lstBox.ItemsSource = co1;

                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT COUNT (*) FROM Service WHERE Title LIKE '" + intext.ToString() + "%' OR Cost LIKE '" + intext.ToString() + "%' ", con);
                adapter = new SqlDataAdapter(cmd);
                string count = cmd.ExecuteScalar().ToString();
                Count2.Text = count;
                ds = new DataSet();
                adapter.Fill(ds, "Service");
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

            /*if (cmBox.Text == "до 5%")
            {
                ComboBox cmBox = (ComboBox)sender;
                ComboBoxItem selectedItem = (ComboBoxItem)cmBox.SelectedItem;
                intext = selectedItem.Content.ToString();
                intext = "0.05";
            }*/
        }

        //фильтр по скидкам
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //берем значение из комбобокса
            ComboBox cmBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)cmBox.SelectedItem;
            string intext = selectedItem.Content.ToString();


            try
            {
                if (intext == "до 5%")
                    intext = "0.05";
                else if (intext == "от 10% до 15%")
                    intext = "0.1";
                else if (intext == "от 20% до 25%")
                    intext = "0.2";
                else if (intext == "до 30%")
                     intext = "0.3";
                else if (intext == "Все услуги")
                    intext = "0";


                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT * FROM Service WHERE Discount LIKE '" + intext.ToString() + "%'", con);
                adapter = new SqlDataAdapter(cmd);

                ds = new DataSet();
                adapter.Fill(ds, "Service");
                Service co = new Service();
                IList<Service> co1 = new List<Service>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    co1.Add(new Service
                    {
                        ID = Convert.ToInt32(dr[0].ToString()),
                        Title = dr[1].ToString(),
                        Cost = Convert.ToDecimal(dr[2].ToString()),
                        DurationInSeconds = Convert.ToInt32(dr[3].ToString()),
                        Description = dr[4].ToString(),
                        Discount = Convert.ToDouble(dr[5].ToString()),
                        MainImagePath = Convert.ToString(dr[6].ToString())
                    });

                }
                lstBox.ItemsSource = co1;

                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT COUNT (*) FROM Service WHERE Discount LIKE '" + intext.ToString() + "%'", con);
                adapter = new SqlDataAdapter(cmd);
                string count = cmd.ExecuteScalar().ToString();
                Count2.Text = count;
                ds = new DataSet();
                adapter.Fill(ds, "Service");
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

        
        //удаление услуги из бд 
        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //подтверждение удаления
                MessageBoxResult result = MessageBox.Show("Вы уверены что хотите удалить услугу?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    //получаем id и удаляем услугу из бд
                    int a = Convert.ToInt32((sender as Button).Uid);
                    con = new SqlConnection(connectionString);
                    con.Open();
                    cmd = new SqlCommand("DELETE FROM Service WHERE ID = " + a + "", con);
                    adapter = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    adapter.Fill(ds, "Service");
                    ds = null;
                    adapter.Dispose();
                    con.Close();
                    con.Dispose();

                    MessageBox.Show("Услуга успешно удалена из базы данных", "Уведомление");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            //получаем id и переходим на стр редактирования 
            int a = Convert.ToInt32((sender as Button).Uid);

            Edit edit = new Edit(a);
            NavigationService.Navigate(edit);
            
        }

        //обновление листбокса по бд
        private void Sbros_Click(object sender, RoutedEventArgs e)
        {
            FillList();

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
    }
}
