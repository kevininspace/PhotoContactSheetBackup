using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using ImageMagick;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows;

namespace ImageBackup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string dirName;
        private List<string> lstFilesFound = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //var dialog = new System.Windows.Forms.FolderBrowserDialog();
            //System.Windows.Forms.DialogResult result = dialog.ShowDialog();


            //// Configure open file dialog box
            //Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.FileName = "Document"; // Default file name
            //dlg.DefaultExt = ".txt"; // Default file extension
            //dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension 

            //// Show open file dialog box
            //Nullable<bool> result = dlg.ShowDialog();

            //// Process open file dialog box results 
            //if (result == true)
            //{
            //    // Open document 
            //    string filename = dlg.FileName;
            //}

            //Check to see if the user is >Vista
            if (CommonFileDialog.IsPlatformSupported)
            {
                var dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                CommonFileDialogResult result = dialog.ShowDialog();
                if (result == CommonFileDialogResult.Ok)
                {
                    dirName = dialog.FileName;
                    textbox.Text = dirName;
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DirSearch(textbox.Text);
        }

        private void MakeMosaic(List<string> list)
        {
            using (MagickImageCollection images = new MagickImageCollection())
            {
                foreach (string imagePath in lstFilesFound)
                {
                    try
                    {
                        MagickImage first = new MagickImage(imagePath);
                        images.Add(first);
                    }
                    catch (Exception ex)
                    {
                        if (ex is MagickCorruptImageErrorException || ex is MagickImageErrorException ||
                            ex is MagickDelegateErrorException || ex is MagickMissingDelegateErrorException ||
                            ex is MagickFileOpenErrorException)
                        {
                            //Not an image, or improper image
                            Console.WriteLine(ex.Message);
                        }
                    }

                }

                MediaInfo info = new MediaInfo(lstFilesFound);
                Options options = new Options();
                options.DropShadow = false;
                options.AutoFitTiles = true;
                options.Details = true;
                options.TileHeight = 180;
                options.TileWidth = 240;
                //options.DetailText = Path.GetDirectoryName(lstFilesFound[0]);

                //MagickImageCollection imagesbmp = new MagickImageCollection();
                //foreach (MagickImage magickImage in images)
                //{
                //    imagesbmp.Add(magickImage.ToBitmap());
                //}
                Thumbnailer thumbnailer = new Thumbnailer(info, options, images);

                // path is your file path
                //string directory = Path.GetDirectoryName(info.DirectoryPath);
                string directoryString = CreateDirectoryString(new DirectoryInfo(info.DirectoryPath));

                string path = Path.Combine(info.DirectoryPath, "contact-sheet");
                thumbnailer.Result.Save(path, ImageFormat.Jpeg);
            }
        }

        public string CreateDirectoryString(DirectoryInfo directory)
        {
            string concatenatedPath = "";
            string directoryRoot = Directory.GetDirectoryRoot(directory.Name);
            if (directory.Parent != null && Directory.GetDirectoryRoot(directory.Name) == directory.Parent.Name)
            {
                concatenatedPath = directory.Name + "-";
            }
            else if (directory.Parent != null && directory.Parent.Exists)
            {
                concatenatedPath = CreateDirectoryString(directory.Parent) + "-";
            }
            concatenatedPath = concatenatedPath + "-" + directory.Name;
            return concatenatedPath;
        }

        void DirSearch(string sDir)
        {
            lstFilesFound.Clear();

            try
            {
                if (Directory.GetDirectories(sDir).Length > 0)
                {

                    foreach (string d in Directory.GetDirectories(sDir))
                    {
                        foreach (string f in Directory.GetFiles(d))
                        {
                            lstFilesFound.Add(f);
                        }
                        MakeMosaic(lstFilesFound);
                        lstFilesFound.Clear();
                        DirSearch(d);
                    }
                }
                else
                {
                    foreach (string f in Directory.GetFiles(sDir))
                    {
                        lstFilesFound.Add(f);
                    }
                    MakeMosaic(lstFilesFound);
                    lstFilesFound.Clear();
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
    }
}
