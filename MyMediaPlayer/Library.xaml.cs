using MyMediaPlayer.MockODataSetTableAdapters;
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
    /// Interaction logic for Library.xaml
    /// </summary>
    public partial class Library : Window
    {
        public Library()
        {
            
            MediaFilesTableAdapter pd = new MediaFilesTableAdapter();
            DataContext = pd;
            dataGrid.DataContext = pd.GetData();
            
            
            InitializeComponent();
            
            
            



        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
