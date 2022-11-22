using CNAApp;
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

namespace CNAApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Client m_client;
        public MainWindow( Client client)
        {
            InitializeComponent();
            m_client = client;
        }

        private void ScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        public void UpdateChatBox(string message)
        {
            chatBox.Dispatcher.Invoke(() =>
            {
                chatBox.Text += message + Environment.NewLine;
                chatBox.ScrollToEnd();
            });
        }

        private void sendMessageButton_Click(object sender, EventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            m_client.SendMessage(messageText.Text);
        }

    }
}
