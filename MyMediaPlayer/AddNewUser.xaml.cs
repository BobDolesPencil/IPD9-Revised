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
    /// Interaction logic for AddNewUser.xaml
    /// </summary>
    public partial class AddNewUser : Window
    {
        public AddNewUser()
        {
            InitializeComponent();
        }

        private void NewUserSubmit_CLick(object sender, RoutedEventArgs e)
        {
            using (var context = new MockOEntities())
            {


                //UserProfile user = new UserProfile()
                var addedUser = new UserProfile
                {
                    FirstName = tbFname.Text,
                    LastNAme = tbLname.Text,
                    userId = tbUserName.Text,
                    pass = tbPassWrd.Text
                };

                //try
                //{
                    context.UserProfiles.Add(addedUser);
                    context.SaveChanges();
                    this.Close();

                //}
                //catch (Exception ex) { }
                




            }
        }
    }
}
