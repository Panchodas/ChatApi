using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using TestOurApi.AppData;

namespace TestOurApi.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public static HttpClient httpClient = new HttpClient();
        public static Employee employee;
        public AuthWindow()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Login) && !string.IsNullOrEmpty(Properties.Settings.Default.Password))
            {
                Enter();
                SaveChb.IsChecked = true;
            }
        }
        //подстановка запомненных логина и пароля
        private void Enter()
        {
            LoginTb.Text = Properties.Settings.Default.Login;
            PasswordPb.Password = Properties.Settings.Default.Password;
        }
        private async void SignIn(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(LoginTb.Text) && !string.IsNullOrEmpty(PasswordPb.Password))
            {
                //учим работать с json
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var content = new UserData { username = LoginTb.Text, password = PasswordPb.Password };
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
                HttpResponseMessage message = await httpClient.PostAsync("http://localhost:50203/api/Auth", httpContent);
                if (message.IsSuccessStatusCode)
                {
                    var curContent = await message.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<Employee>(curContent);
                    if ((bool)SaveChb.IsChecked)
                    {
                        Properties.Settings.Default.Login = LoginTb.Text;
                        Properties.Settings.Default.Password = PasswordPb.Password;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Properties.Settings.Default.Login = string.Empty;
                        Properties.Settings.Default.Password = string.Empty;
                        Properties.Settings.Default.Save();
                    }
                    Window1 w = new Window1();
                    w.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Пользователь не найден");
                }
            }
            else
            {
                MessageBox.Show("Впишите данные");
            }
        }
        public class UserData
        {
            public string username { get; set; }
            public string password { get; set; }
        }
    }
}
