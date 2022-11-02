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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

   
        private void Button_Click(object sender, RoutedEventArgs e)
        {
      if (MessageText.Text == "")
            {
                MessageBox.Show("No message in text box!", "Warning");
            }
            else
            {
                string message = MessageText.Text;
                MessageText.Text = "";
                if(NameInput.Text == "")
                {
                    MessageBox.Show("Please enter a name in Local Name Textbox!", "Warning");
                    MessageText.Text = message;
                }
                else
                {
                    string name = NameInput.Text;
                    MessagePanel.Text += name + ": " + message;
                }
            }
        }
    }
}
