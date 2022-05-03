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
    /// Логика взаимодействия для Edit.xaml
    /// </summary>
    public partial class Edit : Page
    {
        //подключение к бд и объявление переменных
        static String connectionString = "server=LAPTOP-GFFVEP2R; Initial Catalog=VelvetBrows;Integrated Security=SSPI";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        Service srv;

        //конструктор который берет id услуги
        public int id { get; set; }
        
        public Edit(int id = 0)
        {
            this.id = id;
            FillList();
            //this.Resources.Add(srv, "service");
            InitializeComponent();

            //вывод услуги в текстовые поля для редактирования из базы
            Title1.Text = srv.Title;
            Cost1.Text = Convert.ToString(srv.Cost);
            Time.Text = Convert.ToString(srv.DurationInSeconds);
            Discont.Text = Convert.ToString(srv.Discount);
            Image1.Text = srv.MainImagePath;

            
            //MessageBox.Show(this.id.ToString());
        }

        //вывод услуги из бд
        public void FillList()
        {
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT * FROM Service WHERE ID = " + id + "", con);
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
                    srv = new Service
                    {
                        ID = Convert.ToInt32(dr[0].ToString()),
                        Title = dr[1].ToString(),
                        Cost = Convert.ToDecimal(dr[2].ToString()),
                        DurationInSeconds = Convert.ToInt32(dr[3].ToString()),
                        Description = dr[4].ToString(),
                        Discount = Convert.ToDouble(dr[5].ToString()),
                        MainImagePath = Convert.ToString(dr[6].ToString())
                    };
                }
                //lstBox.ItemsSource = co1;
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

        //редактирование услуги
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //берем измененные значения из текстбоксов
                Service cr = new Service();
                cr.Title = Convert.ToString(Title1.Text.ToString());
                cr.Cost = Convert.ToDecimal(Cost1.Text.ToString());
                cr.DurationInSeconds = Convert.ToInt32(Time.Text.ToString());
                cr.Discount = Convert.ToDouble(Discont.Text.ToString());
                cr.MainImagePath = Convert.ToString(Image1.Text.ToString());

                //открываем соединение, создаем новую команду, прописываем запрос и с помощью адаптера обновляем бд
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("UPDATE Service SET Title = '" + cr.Title + "', Cost = " + cr.Cost + ", DurationInSeconds = " + cr.DurationInSeconds + ", Discount= 0." + cr.Discount + ", MainImagePath = '" + cr.MainImagePath + "' WHERE ID= " + id + "", con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "Service");
                ds = null;
                adapter.Dispose();
                con.Close();
                con.Dispose();


                MessageBox.Show("Услуга успешно обновлена!", "Уведомление");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MenuItemHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainWindow.xaml", UriKind.Relative));

        }

        private void MenuItemAdmin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FormAdmin.xaml", UriKind.Relative));

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainWindow.xaml", UriKind.Relative));

        }

        private void Programm_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Данная программа предназначена для салона красоты. В которой вы можете просматривать все услуги салона, добавлять, удалять и изменять их. Также здесь имеется фильрт по скидкам.");

        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Чтобы добавить новую услугу войдите как администратор!");

        }

        //запрет на ввод символов кроме цифр
        private void Cost1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string goodText = "";
            TextBox tb = sender as TextBox;
            if (double.TryParse(tb.Text, out double value)
                              && value >= 0 )
                goodText = tb.Text;
            else
            {
                tb.Text = goodText;
                tb.CaretIndex = tb.Text.Length;
            }
        }

        //запрет на ввод символов кроме цифр
        private void Time_TextChanged(object sender, TextChangedEventArgs e)
        {
            string goodText = "";
            TextBox tb = sender as TextBox;
            if (double.TryParse(tb.Text, out double value)
                              && value >= 0 )
                goodText = tb.Text;
            else
            {
                tb.Text = goodText;
                tb.CaretIndex = tb.Text.Length;
            }
        }

        //запрет на ввод символов кроме цифр
        private void Discont_TextChanged(object sender, TextChangedEventArgs e)
        {
            string goodText = "";
            TextBox tb = sender as TextBox;
            if (double.TryParse(tb.Text, out double value)
                              && value >= 0)
                goodText = tb.Text;
            else
            {
                tb.Text = goodText;
                tb.CaretIndex = tb.Text.Length;
            }
        }
    }
}
