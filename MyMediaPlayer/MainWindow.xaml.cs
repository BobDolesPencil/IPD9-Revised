using Microsoft.Win32;
using MyMediaPlayer.MockODataSetTableAdapters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace MyMediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
       // public bool areyoulogged = false;
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        LogIn log = new LogIn();
        public static LogCheck checklog = new LogCheck(false);
        private Cursor Cursor;
        MockODataSet dataset = new MockODataSet();
        //------ tree view class
        public class DummyTreeViewItem : TreeViewItem
        {
            public DummyTreeViewItem()
                : base()
            {
                base.Header = "Dummy";
                base.Tag = "Dummy";
            }
        }
        //------ tree view class END


        public MainWindow()
        {
            InitializeComponent();
            FillDataGrid();
            this.LoadDirectories();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
                sliProgress.Value = mePlayer.Position.TotalSeconds;
            }
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg, *.mp4,*.avi, *.mkv)|*.mp3;*.mpg;*.mpeg, *.mp4,*.avi, *.mkv|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                mePlayer.Source = new Uri(openFileDialog.FileName);
            Player.Focus();
            mePlayer.Play();
            mediaPlayerIsPlaying = true;
        }

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (mePlayer != null) && (mePlayer.Source != null);
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mePlayer.Play();
            mediaPlayerIsPlaying = true;
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mePlayer.Pause();
        }

        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mePlayer.Stop();
            mediaPlayerIsPlaying = false;
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mePlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mePlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }



        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            AddNewUser Newsplash = new AddNewUser();
            Newsplash.ShowDialog();
        }
        

        //private void btnBrowseMedia_Click(object sender, RoutedEventArgs e)
        //{

        //    OpenFileDialog open = new OpenFileDialog();
        //    if (open.ShowDialog() == true)
        //        tbFileName.Text = open.FileName;



        //}

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {

            log.ShowDialog();

        }

        private void isLoadLibrary(object sender, RoutedEventArgs e)
        {
            using (var context = new MockOEntities())
            {
                var LoadLibrary = context.MediaFiles.ToList();
            }

        }

        //------------------------------------------------------------- multiopen START

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            open.Filter = "Media files (*.mp3; *.mpg; *.mpeg, *.mp4,*.avi, *.mkv)| *.mp3; *.mpg; *.mpeg, *.mp4,*.avi, *.mkv | All files(*.*) | *.* ";

            //List<UploadSelection> uploadList = new List<UploadSelection>();//create class for list... check profiler---- og
            byte[] bytes;

            if (open.ShowDialog() == true)
            {

                if (checklog.Value == true)
                {



                    using (var context = new MockOEntities())
                {
                    

                        try
                        {
                            foreach (var entityToInsert in open.FileNames)
                            {
                                var upload = new MediaFile();
                                bytes = File.ReadAllBytes(entityToInsert);

                                upload.userId = log.UserLogIn;
                                upload.sourceMedia = bytes;
                                upload.title = System.IO.Path.GetFileNameWithoutExtension(open.FileName.ToString());
                                upload.mediaType = System.IO.Path.GetExtension(open.FileName.ToString());
                                MessageBox.Show(upload.title.ToString()+"  -----------------  "+upload.mediaType.ToString());
                                context.MediaFiles.Add(upload);
                            }
                            context.SaveChanges();
                            FillDataGrid();
                            MessageBox.Show("File(s) Uploaded");
                            

                        }
                        catch (DbEntityValidationException ex)
                        {
                            var err = ex.EntityValidationErrors;
                            foreach (var eee in err)
                            {
                                Console.WriteLine("eee: " + eee);
                            }
                            MessageBox.Show("error" + ex + " : " + ex.EntityValidationErrors);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("error" + ex);
                        }



                    

                    
                    }

                } //--if logged in
                else
                {
                    MessageBox.Show("Please Log in");
                    log.ShowDialog();
                    checklog.Value = true;
                }
            }//-- if show dialog = true
        }



        //------------------------------------------------------------- multiopen END
        

       




        //------------------------------------------------------- tree -------------------------------------------------
        public void LoadDirectories()
        {
            var drives = System.IO.DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                lvFileView.Items.Add(GetItem(drive));
            }
        }
        private TreeViewItem GetItem(DriveInfo drive)
        {
            var item = new TreeViewItem
            {
                Header = drive.Name,
                DataContext = drive,
                Tag = drive
            };
            this.AddDummy(item);
            item.Expanded += new RoutedEventHandler(item_Expanded);
            return item;
        }

        private TreeViewItem GetItem(DirectoryInfo directory)
        {
            var item = new TreeViewItem
            {
                Header = directory.Name,
                DataContext = directory,
                Tag = directory
            };
            this.AddDummy(item);
            item.Expanded += new RoutedEventHandler(item_Expanded);
            return item;
        }

        private TreeViewItem GetItem(FileInfo file)
        {
            var item = new TreeViewItem
            {
                Header = file.Name,
                DataContext = file,
                Tag = file
            };
            return item;
        }
        private void AddDummy(TreeViewItem item)
        {
            item.Items.Add(new DummyTreeViewItem());
        }

        private bool HasDummy(TreeViewItem item)
        {
            return item.HasItems && (item.Items.OfType<TreeViewItem>().ToList().FindAll(tvi => tvi is DummyTreeViewItem).Count > 0);
        }
        private void ExploreDirectories(TreeViewItem item)
        {
            var directoryInfo = (DirectoryInfo)null;
            if (item.Tag is DriveInfo)
            {
                directoryInfo = ((DriveInfo)item.Tag).RootDirectory;
            }
            else if (item.Tag is DirectoryInfo)
            {
                directoryInfo = (DirectoryInfo)item.Tag;
            }
            else if (item.Tag is FileInfo)
            {
                directoryInfo = ((FileInfo)item.Tag).Directory;
            }
            if (object.ReferenceEquals(directoryInfo, null)) return;
            try
            {
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    var isHidden = (directory.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                    var isSystem = (directory.Attributes & FileAttributes.System) == FileAttributes.System;
                    if (!isHidden && !isSystem)
                    {
                        item.Items.Add(this.GetItem(directory));
                    }
                }
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show("The directory not found" + ex);
            }

        }

        private void ExploreFiles(TreeViewItem item)
        {
            var directoryInfo = (DirectoryInfo)null;
            if (item.Tag is DriveInfo)
            {
                directoryInfo = ((DriveInfo)item.Tag).RootDirectory;
            }
            else if (item.Tag is DirectoryInfo)
            {
                directoryInfo = (DirectoryInfo)item.Tag;
            }
            else if (item.Tag is FileInfo)
            {
                directoryInfo = ((FileInfo)item.Tag).Directory;
            }
            if (object.ReferenceEquals(directoryInfo, null)) return;

            try
            {
                foreach (var file in directoryInfo.GetFiles())
                {
                    var isHidden = (file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                    var isSystem = (file.Attributes & FileAttributes.System) == FileAttributes.System;
                    if (!isHidden && !isSystem)
                    {
                        item.Items.Add(this.GetItem(file));
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error: " + ex);
            }

        }

        private void RemoveDummy(TreeViewItem item)
        {
            var dummies = item.Items.OfType<TreeViewItem>().ToList().FindAll(tvi => tvi is DummyTreeViewItem);
            foreach (var dummy in dummies)
            {
                item.Items.Remove(dummy);
            }
        }

        void item_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;
            if (this.HasDummy(item))
            {
                this.Cursor = Cursors.Wait;
                this.RemoveDummy(item);
                this.ExploreDirectories(item);
                this.ExploreFiles(item);
                this.Cursor = Cursors.Arrow;
            }
        }
        
        //------------------------------------------------------- tree -------------------------------------------------

        private void FillDataGrid()
        {



            MediaFilesTableAdapter pd = new MediaFilesTableAdapter();
            {

                MyMediaPlayer.MockODataSet.MediaFilesDataTable dt = new MyMediaPlayer.MockODataSet.MediaFilesDataTable();
                pd.Fill(dt);
                dataGrid.ItemsSource = dt.DefaultView;
            }
        }

        
        //------------------ context play listviw
        private void PlayMenu_Click(object sender, RoutedEventArgs e)
        {
        TreeViewItem temp = ((TreeViewItem)lvFileView.SelectedItem);

        if (temp == null)

        return;

        string path;
        path = "";
        string temp1 = "";
        string temp2 = "";

            while (true)
            {
            temp1 = temp.Header.ToString();

                if (temp1.Contains(@"\"))
            {
            temp2 = "";
            }
            path = temp1 + temp2 + path;

                if (temp.Parent.GetType().ToString() == "System.Windows.Controls.TreeView")
                    {
                        break;
                    }
            temp = ((TreeViewItem)temp.Parent);
            temp2 = @"\";
            }
            
            
            try
            {
                var uri = new Uri(path);



                mePlayer.Source = uri;

                Player.Focus();

                mePlayer.Play();
                mediaPlayerIsPlaying = true;
            }
            catch (UriFormatException ex)
            {
                MessageBox.Show("There was an excaption: " + ex);
            }


        }




        
        private void LibraryView_GotFocus(object sender, RoutedEventArgs e)
        {

            FillDataGrid();

        }


        //private void PlayMenuItem_Click(object sender, RoutedEventArgs e)
        //{

        //    using (var context = new MockOEntities())
        //    {
        //        var load = DataGrid.

        //    }
        //}


        //------------------ context upload listviw
        private void Upload_click(object sender, RoutedEventArgs e)
        {
            TreeViewItem temp = ((TreeViewItem)lvFileView.SelectedItem);

            if (temp == null)

                return;

            string path;
            path = "";
            string temp1 = "";
            string temp2 = "";

            while (true)
            {
                temp1 = temp.Header.ToString();

                if (temp1.Contains(@"\"))
                {
                    temp2 = "";
                }
                path = temp1 + temp2 + path;

                if (temp.Parent.GetType().ToString() == "System.Windows.Controls.TreeView")
                {
                    break;
                }
                temp = ((TreeViewItem)temp.Parent);
                temp2 = @"\";
            }

            byte[] bytes;

                if (checklog.Value == true)
                {



                    using (var context = new MockOEntities())
                    {


                        try
                        {
                            
                                var upload = new MediaFile();
                                bytes = File.ReadAllBytes(path);

                                upload.userId = log.UserLogIn;
                                upload.sourceMedia = bytes;
                                upload.title = System.IO.Path.GetFileNameWithoutExtension(path.ToString());
                                upload.mediaType = System.IO.Path.GetExtension(path.ToString());
                                
                                context.MediaFiles.Add(upload);
                            
                            context.SaveChanges();
                            FillDataGrid();
                            MessageBox.Show("File(s) Uploaded");


                        }
                        catch (DbEntityValidationException ex)
                        {
                            var err = ex.EntityValidationErrors;
                            foreach (var eee in err)
                            {
                                Console.WriteLine("eee: " + eee);
                            }
                            MessageBox.Show("error" + ex + " : " + ex.EntityValidationErrors);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("error" + ex);
                        }






                    }

                } //--if logged in
                else
                {
                    MessageBox.Show("Please Log in");
                    log.ShowDialog();
                    checklog.Value = true;
                }
            }//-- if show dialog = true

        
    }
}
