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
using System.Windows.Shapes;

namespace MyMediaPlayer
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        public LogIn()
        {
            InitializeComponent();
        }

     



        public string UserLogIn { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserLogIn = tbLogIn.Text;
            string msg = "Logged in as: " + UserLogIn;
            this.Close();
            MessageBox.Show(msg);
            Title = "MMP " + msg;
            
        }
    }
}
