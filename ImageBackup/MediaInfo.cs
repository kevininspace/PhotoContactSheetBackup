using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using ImageMagick;

namespace ImageBackup
{
    internal class MediaInfo
    {
        public MagickFormat MagickFormat { get; private set; }
        public double ResolutionX { get; private set; }
        public double ResolutionY { get; private set; }
        //#region MediaInfo.dll Imports
        //[DllImport("MediaInfo.dll")]
        //private static extern IntPtr MediaInfo_New();
        //[DllImport("MediaInfo.dll")]
        //private static extern IntPtr MediaInfo_Open(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string FileName);
        //[DllImport("MediaInfo.dll")]
        //private static extern void MediaInfo_Close(IntPtr Handle);
        //[DllImport("MediaInfo.dll")]
        //private static extern void MediaInfo_Delete(IntPtr Handle);
        //[DllImport("MediaInfo.dll")]
        //private static extern IntPtr MediaInfo_Get(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber, [MarshalAs(UnmanagedType.LPWStr)] string Parameter, IntPtr KindOfInfo, IntPtr KindOfSearch);
        //#endregion

        //public string Duration { get; private set; }
        public string FileName { get; private set; }
        public int FileSize { get; private set; }
        //public string VideoCodec { get; private set; }
        //public string VideoBitrate { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        //public string AspectRatio { get; private set; }
        //public string FPS { get; private set; }
        //public string AudioCodec { get; private set; }
        //public string AudioBitrate { get; private set; }
        //public string AudioChannels { get; private set; }
        //public string SampleRate { get; private set; }

        public string DirectoryPath { get; private set; }
        public int NumberOfFiles { get; private set; }
        public DateTime Date { get; private set; }

        public DateTime? ImageDate { get; set; }

        public ColorSpace ColorSpace { get; private set; }

        public string Comment { get; private set; }


        public MediaInfo(string path)
        {
            //IntPtr handle = MediaInfo_New();
            //MediaInfo_Open(handle, path);
            //Duration = Get(handle, StreamKind.General, 0, "Duration/String3", InfoKind.Text, InfoKind.Name);
            //FileName = Get(handle, StreamKind.General, 0, "FileName", InfoKind.Text, InfoKind.Name);
            //FileSize = Get(handle, StreamKind.General, 0, "FileSize/String4", InfoKind.Text, InfoKind.Name);
            //VideoCodec = Get(handle, StreamKind.Video, 0, "Codec/String", InfoKind.Text, InfoKind.Name);
            //VideoBitrate = Get(handle, StreamKind.Video, 0, "BitRate/String", InfoKind.Text, InfoKind.Name);
            //Width = Get(handle, StreamKind.Video, 0, "Width", InfoKind.Text, InfoKind.Name);
            //Height = Get(handle, StreamKind.Video, 0, "Height", InfoKind.Text, InfoKind.Name);
            //AspectRatio = Get(handle, StreamKind.Video, 0, "DisplayAspectRatio/String", InfoKind.Text, InfoKind.Name);
            //FPS = Get(handle, StreamKind.Video, 0, "FrameRate", InfoKind.Text, InfoKind.Name);
            //AudioCodec = Get(handle, StreamKind.Audio, 0, "Codec/String", InfoKind.Text, InfoKind.Name);
            //AudioBitrate = Get(handle, StreamKind.Audio, 0, "BitRate/String", InfoKind.Text, InfoKind.Name);
            //AudioChannels = Get(handle, StreamKind.Audio, 0, "Channel(s)/String", InfoKind.Text, InfoKind.Name);
            //SampleRate = Get(handle, StreamKind.Audio, 0, "SamplingRate/String", InfoKind.Text, InfoKind.Name);
            //MediaInfo_Close(handle);
            //MediaInfo_Delete(handle);
        }

        public MediaInfo(MagickImage image)
        {
            ColorSpace = image.ColorSpace;
            Comment = image.Comment;
            FileName = image.FileName;
            FileSize = image.FileSize;
            MagickFormat = image.Format;
            foreach (ExifValue exifValue in image.GetExifProfile().Values)
            {
                if (exifValue.Tag == ExifTag.DateTimeOriginal)
                {
                    //YYYY:MM:DD HH:MM:SS
                    //DateTime dtresult;

                    //IFormatProvider formatp = d;
                    //DateTime.TryParse(exifValue.Value.ToString(), formatp, DateTimeStyles.None, out dtresult);
                    ImageDate = DateTaken(exifValue.Value.ToString());
                }
            }
            Height = image.Height;
            Width = image.Width;
            ResolutionX = image.ResolutionX;
            ResolutionY = image.ResolutionY;
        }

        public MediaInfo(List<string> lstFilesFound)
        {
            DirectoryPath = Path.GetDirectoryName(lstFilesFound[0]);
            NumberOfFiles = lstFilesFound.Count;
            Date = Directory.GetCreationTime(lstFilesFound[0]);
        }

        private String Get(IntPtr Handle, StreamKind StreamKind, int StreamNumber, string Parameter, InfoKind KindOfInfo, InfoKind KindOfSearch)
        {
            return "MediaInfo";
            //return Marshal.PtrToStringUni(MediaInfo_Get(Handle, (IntPtr)StreamKind, (IntPtr)StreamNumber, Parameter, (IntPtr)KindOfInfo, (IntPtr)KindOfSearch));
        }

        /// <summary>
        /// Returns the EXIF Image Data of the Date Taken.
        /// </summary>
        /// <param name="getImage">Image (If based on a file use Image.FromFile(f);)</param>
        /// <returns>Date Taken or Null if Unavailable</returns>
        public static DateTime? DateTaken(string dateTakenTag)
        {
            //int DateTakenValue = 0x9003; //36867;

            //if (!getImage.PropertyIdList.Contains(DateTakenValue))
            //    return null;

            //string dateTakenTag = System.Text.Encoding.ASCII.GetString(getImage.GetPropertyItem(DateTakenValue).Value);
            string[] parts = dateTakenTag.Split(':', ' ');
            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);
            int hour = int.Parse(parts[3]);
            int minute = int.Parse(parts[4]);
            int second = int.Parse(parts[5]);

            return new DateTime(year, month, day, hour, minute, second);
        }
    }

    enum StreamKind
    {
        General,
        Video,
        Audio,
        Text,
        Chapters,
        Image
    }

    enum InfoKind
    {
        Name,
        Text,
        Measure,
        Options,
        NameText,
        MeasureText,
        Info,
        HowTo
    }

    enum InfoOptions
    {
        ShowInInform,
        Support,
        ShowInSupported,
        TypeOfValue
    }
}
