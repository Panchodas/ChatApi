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
using System.Windows.Threading;
using TestOurApi.AppData;

namespace TestOurApi.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChatRoomWindow.xaml
    /// </summary>
    public partial class ChatRoomWindow : Window
    {
        DispatcherTimer timer;
        public List<ChatMessage> chatMessages = new List<ChatMessage>();
        public ChatRoomWindow()
        {
            InitializeComponent();
            Title = Window1.chatRoom.Topic;
            GetMessage();
            Update(); //ежесекундное обновление чата
        }
        private void Update()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1); //установка секундного интервала
            timer.Tick += Timer_Tick; //событие
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            GetMessage();
        }

        private async void GetMessage()
        {
            HttpResponseMessage httpResponseMessage = await AuthWindow.httpClient
                .GetAsync("http://localhost:50203/api/ChatMessages");
            var content = await httpResponseMessage.Content
                .ReadAsStringAsync();
            chatMessages = JsonConvert
                .DeserializeObject<List<ChatMessage>>(content);
            ChatRoomList.ItemsSource = chatMessages
                .Where(i => i.IdChatRoom == Window1.chatRoom.id);
            ChatRoomList.ScrollIntoView(chatMessages
                .Where(i => i.IdChatRoom == Window1.chatRoom.id)
                .LastOrDefault());
        }
        private async void SendMessage(object sender, RoutedEventArgs e)
        {
            var message = new ChatMessage
            {
                IdChatRoom = Window1.chatRoom.id,
                DateTime = DateTime.Now,
                TextMessage = MessageTb.Text,
                idEmployee = AuthWindow.employee.id
            };
            HttpContent httpContent = new StringContent(JsonConvert
                .SerializeObject(message), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await AuthWindow
                .httpClient.PostAsync("http://localhost:50203/api/ChatMessages", httpContent);
            MessageTb.Text = string.Empty;
            GetMessage();
        }
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();
            Close();
            timer.Stop();
        }
    }
}
