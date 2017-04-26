using MyMediaPlayer.MockODataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using static MyMediaPlayer.MockODataSet;

namespace MyMediaPlayer
{
    /// <summary>
    /// Interaction logic for Library.xaml
    /// </summary>
    public partial class Library : Window
    {
        public Library()
        {
            
           
            
            
            
            InitializeComponent();
            FillDataGrid();







        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void FillDataGrid()
        {
            //string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            string CmdString = string.Empty;
            //using (SqlConnection con = new SqlConnection(ConString))
            MediaFilesTableAdapter pd = new MediaFilesTableAdapter();
            {
                CmdString = "SELECT emp_id, fname, lname, hire_date FROM Employee";
                //SqlCommand cmd = new SqlCommand(CmdString, con);
                //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                MediaFilesDataTable dt = new MediaFilesDataTable();
                pd.Fill(dt);
                dataGrid.ItemsSource = dt.DefaultView;
            }
        }
    }
}
