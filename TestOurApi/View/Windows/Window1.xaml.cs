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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public List<ChatRoomEmployee> chatRoomEmployee = new List<ChatRoomEmployee>();
        //public List<ChatRoom> chatRoom = new List<ChatRoom>();
        public static ChatRoom chatRoom;
        public Window1()
        {
            InitializeComponent();
            hellogrid.DataContext = AuthWindow.employee;
            GetRooms();
        }
        public async void GetRooms()
        {
            //берём инфо о чатах
            HttpResponseMessage httpResponseMessage = await AuthWindow.httpClient.GetAsync("http://localhost:50203/api/Chatrooms");
            var rooms = await httpResponseMessage.Content.ReadAsStringAsync();
            //берём инфо о пользователях в чатах
            HttpResponseMessage responseMessage = await AuthWindow.httpClient.GetAsync("http://localhost:50203/api/ChatroomEmploees");
            var emp = await responseMessage.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<ChatRoomEmployee>>(emp)
                .Where(i => i.IdEmployee == AuthWindow.employee.id).ToList();
            if (result == null)
            {
                
            }
            else
            {
                var temp = JsonConvert.DeserializeObject<List<ChatRoom>>(rooms).ToList();
                ChatRoomList.ItemsSource = from t in temp
                                           join r in result on t.id equals r.IdChatRoom
                                           select t;
            }
        }
        private void BACK_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow m = new AuthWindow();
            m.Show();
            Close();
        }
        
        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            chatRoom = ChatRoomList.SelectedItem as ChatRoom;
            ChatRoomWindow chatRoomWindow = new ChatRoomWindow();
            chatRoomWindow.Show();
            Close();
        }
    }
}
