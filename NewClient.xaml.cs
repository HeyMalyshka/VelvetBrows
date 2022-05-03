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
using Microsoft.Win32;
using System.IO;
using System.Collections.ObjectModel;

namespace VelvetBrows
{
    public class Test
    {
        public string Img { get; set; }
        public string Text { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для NewClient.xaml
    /// </summary>
    public partial class NewClient : Page
    {
        //подключение к бд и объявление переменных
        static String connectionString = "server=LAPTOP-GFFVEP2R; Initial Catalog=VelvetBrows;Integrated Security=SSPI";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        Service srv;

        private string imgName;


        public NewClient()
        {
            InitializeComponent();
            FillList();

            Image1.DataContext = new ObservableCollection<Test>()
            {
                new Test() {Img = @"\images\96.png", Text="images/96.png"},
                new Test() {Img = @"\images\face.png", Text="images/face.png"},
                new Test() {Img = @"\images\man.png", Text="images/man.png"},
                new Test() {Img = @"\images\брови.png", Text="images/брови.png"},

            };

        }

        //вывод услуг из бд
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
                //Image.ItemsSource = co1;
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

        private void MenuItemHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainWindow.xaml", UriKind.Relative));

        }

        private void MenuItemAdmin_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы находитесь на странице администратора!", "Уведомление");

        }

        public void Image_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var comboItem = (Test)Image1.SelectedValue;

            /*ComboBox Image = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)Image.SelectedItem;*/
            /*string intext = comboItem.Text.ToString();

            Service cr = new Service();
            cr.MainImagePath = Convert.ToString(intext);

            con = new SqlConnection(connectionString);
            con.Open();
            cmd = new SqlCommand("INSERT INTO Service (MainImagePath) VALUES ('" + cr.MainImagePath + "')", con);
            adapter = new SqlDataAdapter(cmd);
            ds = new DataSet();
            adapter.Fill(ds, "Service");
            ds = null;
            adapter.Dispose();
            con.Close();
            con.Dispose();*/

        }

        //добавление новой усулги в бд
        private void Zapis_Click(object sender, RoutedEventArgs e)
        {
            var comboItem = (Test)Image1.SelectedValue;
            string intext = comboItem.Text.ToString();
            try
            {
                //проверка на заполнение обязательных полей
                if (Title.Text != null && Cost.Text != null && Time.Text != null)
                {
                    Service cr = new Service();
                    cr.Title = Convert.ToString(Title.Text.ToString());
                    cr.Cost = Convert.ToDecimal(Cost.Text.ToString());
                    cr.DurationInSeconds = Convert.ToInt32(Time.Text.ToString());
                    //cr.Description = Convert.ToString(DescText.Text.ToString());
                    cr.Discount = Convert.ToDouble(Discont.Text.ToString());
                    cr.MainImagePath = Convert.ToString(intext);

                    con = new SqlConnection(connectionString);
                    con.Open();
                    cmd = new SqlCommand("INSERT INTO Service (Title, Cost, DurationInSeconds, Discount, MainImagePath) VALUES ('" + cr.Title + "', " + cr.Cost + " , " + cr.DurationInSeconds + ", 0." + cr.Discount + ", '" + cr.MainImagePath + "')", con);
                    adapter = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    adapter.Fill(ds, "Service");
                    ds = null;
                    adapter.Dispose();
                    con.Close();
                    con.Dispose();

                    MessageBox.Show("Услуга успешно добавлена в базу данных", "Уведомление");
                }
                else
                    MessageBox.Show("Обязательные поля для заполнения пусты! Заполните первые 3 поля", "Внимание!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void Programm_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Данная программа предназначена для салона красоты. В которой вы можете просматривать все услуги салона, добавлять, удалять и изменять их. " +
                "Также здесь имеется фильрт по скидкам и поиск.");

        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Чтобы добавить новую услугу войдите как администратор!");

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AdminPanel.xaml", UriKind.Relative));

        }

        //запрет на ввод символов кроме цифр
        private void Cost_TextChanged(object sender, TextChangedEventArgs e)
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

        /*private void openImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*jpg|All Files (*.*)|*.*";
            ofd.ShowDialog();
            string imgPath = ofd.FileName;

            string[] splitter = imgPath.Split('\\');

            imgName = @"CC:\Users\Пользователь\Desktop\VelvetBrows\VelvetBrows\Images" + splitter[splitter.Length - 1];

            var di = new DirectoryInfo(@"C:\Users\Пользователь\Desktop\VelvetBrows\VelvetBrows\Images");


            System.IO.File.Copy(imgPath, imgName, true);

            MessageBox.Show("ok!");

        }*/
    }
}
