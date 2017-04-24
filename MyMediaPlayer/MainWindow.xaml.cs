﻿using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        LogIn log = new LogIn();
        public static LogCheck checklog = new LogCheck(false);

        public MainWindow()
        {
            InitializeComponent();
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
            openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                mePlayer.Source = new Uri(openFileDialog.FileName);
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

        //private void btnUpload_Click(object sender, RoutedEventArgs e)
        //{


        //    OpenFileDialog open = new OpenFileDialog();
        //    open.Multiselect = true;
        //    open.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*";

        //    if (open.ShowDialog() == true)

        //        tbFileName.Text = open.FileName;
        //    string filename = tbFileName.Text.ToString();

        //    byte[] bytes = File.ReadAllBytes(filename);

        //    // if (LoggedIn != "")
        //    if (checklog.Value == true)
        //    {

        //        using (var context = new MockOEntities())
        //        {

        //            var upload = new MediaFile
        //            {


        //                userId = log.UserLogIn,
        //                sourceMedia = bytes,
        //                mediaType = "mp4"




        //            };


        //            context.MediaFiles.Add(upload);
        //            context.SaveChanges();
        //            MessageBox.Show("File Uploaded");

        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please Log in");

        //        log.ShowDialog();
        //        checklog.Value = true;


        //    }
        //}





        //}

        private void btnBrowseMedia_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == true)
                tbFileName.Text = open.FileName;



        }

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

        //----------------------------- TRYING MULTI UPLOAD -------------------------------------------------------------------------------

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            open.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*";



            List<UploadSelection> uploadList = new List<UploadSelection>();//create class for list... check profiler
            byte[] bytes;                                                              




            if (open.ShowDialog() == true)



                if (checklog.Value == true)
                {

                foreach (string file in open.FileNames)
                {
                    
                    string filename = open.FileName;
                    bytes = File.ReadAllBytes(filename);

                    uploadList.Add(new UploadSelection() { newfile = bytes, mediatype = "MP4", mediatitle = "newTest", userId = log.UserLogIn });
                }



            
                using (var context = new MockOEntities())
                {
                    if (open.ShowDialog() == true)
                    {

                        try
                        {



                            foreach (var entityToInsert in uploadList)
                            {
                                var upload = new MediaFile();

                                entityToInsert.userId = log.UserLogIn;
                                entityToInsert.newfile = upload.sourceMedia;
                                entityToInsert.mediatitle = upload.title;
                                entityToInsert.mediatype = upload.mediaType;


                                context.MediaFiles.Add(upload);
                                }
                                context.SaveChanges();

                            
                            MessageBox.Show("File(s) Uploaded");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("error" + ex);
                        }



                    }
                    else
                    {
                        MessageBox.Show("Please Log in");
                        log.ShowDialog();
                        checklog.Value = true;
                    }




                }
            }
        }

//--------------------------- -------------------------------------------------------

    }
}
    