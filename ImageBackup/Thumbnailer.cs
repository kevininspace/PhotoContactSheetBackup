using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using ImageMagick;

namespace ImageBackup
{
    internal class Thumbnailer : IDisposable
    {
        protected int mColCount;
        protected int mDetailHeight;
        protected string mDetailText;
        protected int mDetailWidth;
        protected int mImageHeight;
        protected int mImageWidth;
        protected MediaInfo mInfo;
        protected InterpolationMode mInterpolation;
        protected IList<string> mLabels;
        protected Options mOptions;
        protected Bitmap mResultBitmap;
        protected int mRowCount;
        protected MagickImageCollection mScreenshots;
        //protected IList<Bitmap> mScreenshots;
        protected Bitmap mShadowTemplate;
        protected int mThumbCount;
        protected int mThumbHeight;
        protected MagickImageCollection mThumbnailsList;
        //protected List<Bitmap> mThumbnailsList;
        protected int mThumbWidth;
        protected int mTileHeight;
        protected int mTileWidth;
        protected int mLabelHeight;

        public event EventHandler Update;

        //public Thumbnailer(MediaInfo Info, MagickImage Screenshot, string Label)
        //{
        //    mInfo = Info;
        //    mOptions = new Options();
        //    mOptions.LoadOptions();
        //    mOptions.ColumnCount = 1;
        //    mOptions.Details = false;
        //    mOptions.DropShadow = false;
        //    mOptions.GapHeight = 0;
        //    mOptions.GapWidth = 0;
        //    mOptions.Labels = false;
        //    mOptions.TileWidth = mInfo.Width;
        //    mOptions.TileHeight = Info.Height;
        //    mScreenshots = new MagickImageCollection();
        //    //mScreenshots = new List<Bitmap>();
        //    mLabels = new List<string>();
        //    mScreenshots.Add(Screenshot);
        //    mLabels.Add(Label);
        //    mThumbCount = 1;
        //    mInterpolation = InterpolationMode.HighQualityBicubic;
        //    UpdateTileSize();
        //    UpdateImageSize();
        //    UpdateShadowTemplate();
        //    ResizeScreenshots();
        //    UpdateDetails();
        //    UpdateResult();
        //    SetOptionEvents();
        //}

        //public Thumbnailer(MediaInfo Info, Options pOptions, MagickImageCollection Screenshots, IList<string> Labels, InterpolationMode Mode)
        //{
        //    mInfo = Info;
        //    mOptions = pOptions;
        //    mScreenshots = Screenshots;
        //    mLabels = Labels;
        //    mThumbCount = Screenshots.Count;
        //    mInterpolation = Mode;
        //    UpdateTileSize();
        //    UpdateImageSize();
        //    UpdateShadowTemplate();
        //    ResizeScreenshots();
        //    UpdateDetails();
        //    UpdateResult();
        //    SetOptionEvents();
        //}

        public Thumbnailer(MediaInfo Info, Options pOptions, MagickImageCollection Screenshots)
        {
            mInfo = Info;
            mOptions = pOptions;
            mScreenshots = Screenshots;
            mLabels = GetLabels(Screenshots);
            CalculateLabelHeight();
            mThumbCount = Screenshots.Count;
            UpdateTileSize();
            UpdateImageSize();
            UpdateShadowTemplate();
            ResizeScreenshots();
            UpdateDetails();
            UpdateResult();
            SetOptionEvents();
        }

        private IList<string> GetLabels(MagickImageCollection screenshots)
        {
            List<string> labelInfo = new List<string>();

            foreach (MagickImage magickImage in screenshots)
            {
                MediaInfo mediaInfo = new MediaInfo(magickImage);
                labelInfo.Add(mediaInfo.FileName + "\r\n" + mediaInfo.ImageDate);
            }

            return labelInfo;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (mThumbnailsList != null)
                {
                    foreach (MagickImage thumb in mThumbnailsList)
                    {
                        thumb.Dispose();
                    }
                }
                if (mResultBitmap != null)
                {
                    mResultBitmap.Dispose();
                }
                if (mShadowTemplate != null)
                {
                    mShadowTemplate.Dispose();
                }
            }
        }

        #region Events

        private void mOptions_AutoFitTilesChanged(object sender, EventArgs e)
        {
            UpdateTileSize();
            UpdateImageSize();
            UpdateResult();
        }

        private void mOptions_BackColorChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void mOptions_BackgroundImageChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void mOptions_ColumnCountChanged(object sender, EventArgs e)
        {
            UpdateImageSize();
            UpdateResult();
        }

        private void mOptions_DetailFontChanged(object sender, EventArgs e)
        {
            UpdateDetails();
            UpdateResult();
        }

        private void mOptions_DetailPositionChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void mOptions_DetailsChanged(object sender, EventArgs e)
        {
            UpdateDetails();
            UpdateResult();
        }

        private void mOptions_DetailTextChanged(object sender, EventArgs e)
        {
            UpdateDetails();
            UpdateResult();
        }

        private void mOptions_DropShadowChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void mOptions_GapHeightChanged(object sender, EventArgs e)
        {
            UpdateImageSize();
            UpdateResult();
        }

        private void mOptions_GapWidthChanged(object sender, EventArgs e)
        {
            UpdateImageSize();
            UpdateResult();
        }

        private void mOptions_InfoFontChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void mOptions_InfoPositionsChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void mOptions_LabelsChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void mOptions_OffsetXChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void mOptions_OffsetYChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void mOptions_OptionsChanged(object sender, EventArgs e)
        {
            UpdateTileSize();
            UpdateImageSize();
            UpdateShadowTemplate();
            ResizeScreenshots();
            UpdateDetails();
            UpdateResult();
        }

        private void mOptions_ShadowIntensityChanged(object sender, EventArgs e)
        {
            UpdateShadowTemplate();
            UpdateResult();
        }

        private void mOptions_TileHeightChanged(object sender, EventArgs e)
        {
            UpdateTileSize();
            ResizeScreenshots();
            UpdateImageSize();
            UpdateShadowTemplate();
            UpdateResult();
        }

        private void mOptions_TileWidthChanged(object sender, EventArgs e)
        {
            UpdateTileSize();
            ResizeScreenshots();
            UpdateImageSize();
            UpdateShadowTemplate();
            UpdateResult();
        }

        #endregion

        protected void OnUpdate()
        {
            if (Update != null)
            {
                Update(this, EventArgs.Empty);
            }
        }

        private void ResizeScreenshots()
        {
            if (mThumbnailsList != null)
            {
                foreach (MagickImage thumb in mThumbnailsList)
                {
                    thumb.Dispose();
                }
            }
            mThumbnailsList = new MagickImageCollection();
            //mThumbnailsList = new List<Bitmap>();
            for (int i = 0; i < mThumbCount; i++)
            {
                MagickImage bitmap = mScreenshots[i];
                //Bitmap bitmap = mScreenshots[i];
                if ((mThumbWidth == bitmap.Width) && (mThumbHeight == bitmap.Height))
                {
                    mThumbnailsList.Add(bitmap.Clone());
                    //mThumbnailsList.Add(bitmap.Clone(new Rectangle(Point.Empty, bitmap.Size), PixelFormat.Format32bppArgb));
                }
                else
                {
                    bitmap.Resize(mThumbWidth, mThumbHeight);
                    mThumbnailsList.Add(bitmap);
                    //Bitmap thumb = new Bitmap(mThumbWidth, mThumbHeight);
                    //Graphics graphics = Graphics.FromImage(thumb);
                    //graphics.InterpolationMode = mInterpolation;
                    //graphics.DrawImage(bitmap, 0, 0, mThumbWidth, mThumbHeight);
                    //mThumbnailsList.Add(thumb);
                    //graphics.Dispose();
                }
            }
        }

        public void Save(string FileName)
        {
            string extension = Path.GetExtension(FileName).ToUpper();
            ImageCodecInfo codecInfo = null;
            EncoderParameters encoderParams = new EncoderParameters(1);
            foreach (ImageCodecInfo ci in ImageCodecInfo.GetImageEncoders())
            {
                string[] codecExtensions = ci.FilenameExtension.ToUpper().Split(";".ToCharArray());
                bool found = false;
                codecInfo = ci;
                for (int i = 0; i < codecExtensions.Length; i++)
                {
                    if (codecExtensions[i].Contains(extension))
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    break;
                }
            }
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, mOptions.JpegQuality);
            mResultBitmap.Save(FileName, codecInfo, encoderParams);
        }

        protected void SetOptionEvents()
        {
            mOptions.BackColorChanged += new EventHandler(mOptions_BackColorChanged);
            mOptions.AutoFitTilesChanged += new EventHandler(mOptions_AutoFitTilesChanged);
            mOptions.ColumnCountChanged += new EventHandler(mOptions_ColumnCountChanged);
            mOptions.DropShadowChanged += new EventHandler(mOptions_DropShadowChanged);
            mOptions.GapHeightChanged += new EventHandler(mOptions_GapHeightChanged);
            mOptions.GapWidthChanged += new EventHandler(mOptions_GapWidthChanged);
            mOptions.ShadowIntensityChanged += new EventHandler(mOptions_ShadowIntensityChanged);
            mOptions.TileHeightChanged += new EventHandler(mOptions_TileHeightChanged);
            mOptions.TileWidthChanged += new EventHandler(mOptions_TileWidthChanged);
            mOptions.OffsetXChanged += new EventHandler(mOptions_OffsetXChanged);
            mOptions.OffsetYChanged += new EventHandler(mOptions_OffsetYChanged);
            mOptions.BackgroundImageChanged += new EventHandler(mOptions_BackgroundImageChanged);
            mOptions.DetailFontChanged += new EventHandler(mOptions_DetailFontChanged);
            mOptions.DetailPositionChanged += new EventHandler(mOptions_DetailPositionChanged);
            mOptions.DetailsChanged += new EventHandler(mOptions_DetailsChanged);
            mOptions.DetailTextChanged += new EventHandler(mOptions_DetailTextChanged);
            mOptions.InfoFontChanged += new EventHandler(mOptions_InfoFontChanged);
            mOptions.InfoPositionsChanged += new EventHandler(mOptions_InfoPositionsChanged);
            mOptions.LabelsChanged += new EventHandler(mOptions_LabelsChanged);
            mOptions.OptionsChanged += new EventHandler(mOptions_OptionsChanged);
        }

        private void UpdateDetails()
        {
            mDetailHeight = 0;
            mDetailWidth = 0;
            if (mOptions.Details)
            {
                Bitmap image = new Bitmap(1, 1);
                image.SetResolution(300,300);
                Graphics graphics = Graphics.FromImage(image);
                mDetailText = mOptions.DetailText;
                mDetailText = mDetailText.Replace("[DIRECTORYPATH]", mInfo.DirectoryPath);
                mDetailText = mDetailText.Replace("[NUMBER_OF_IMAGES]", mThumbCount.ToString());
                mDetailText = mDetailText.Replace("[DATE]", Convert.ToString(mInfo.Date));
                
                //mDetailText = mDetailText.Replace("[ASPECT_RATIO]", mInfo.AspectRatio);
                //mDetailText = mDetailText.Replace("[AUDIO_BITRATE]", mInfo.AudioBitrate);
                //mDetailText = mDetailText.Replace("[AUDIO_CHANNELS]", mInfo.AudioChannels);
                //mDetailText = mDetailText.Replace("[AUDIO_CODEC]", mInfo.AudioCodec);
                //mDetailText = mDetailText.Replace("[DURATION]", mInfo.Duration);
                //mDetailText = mDetailText.Replace("[FILE_NAME]", mInfo.FileName);
                //mDetailText = mDetailText.Replace("[FILE_SIZE]", mInfo.FileSize);
                //mDetailText = mDetailText.Replace("[FPS]", mInfo.FPS);
                //mDetailText = mDetailText.Replace("[HEIGHT]", mInfo.Height);
                //mDetailText = mDetailText.Replace("[AUDIO_SAMPLERATE]", mInfo.SampleRate);
                //mDetailText = mDetailText.Replace("[VIDEO_BITRATE]", mInfo.VideoBitrate);
                //mDetailText = mDetailText.Replace("[VIDEO_CODEC]", mInfo.VideoCodec);
                //mDetailText = mDetailText.Replace("[WIDTH]", mInfo.Width);
                SizeF ef = graphics.MeasureString(mDetailText, mOptions.DetailFont);
                mDetailWidth = (int)ef.Width;
                mDetailHeight = ((int)ef.Height) + mOptions.GapHeight;
                graphics.Dispose();
                image.Dispose();
            }
        }

        private void CalculateLabelHeight()
        {
            mLabelHeight = 0;
            if (mOptions.Labels)
            {
                Bitmap image = new Bitmap(1,1);
                image.SetResolution(300,300);
                Graphics graphics = Graphics.FromImage(image);
                string mLabelSampleText = mLabels[0];

                SizeF ef = graphics.MeasureString(mLabelSampleText, mOptions.InfoFont);
                mLabelHeight = ((int)ef.Height) + mOptions.GapHeight;
                graphics.Dispose();
                image.Dispose();


            }
        }

        private void UpdateImageSize()
        {
            mColCount = mOptions.ColumnCount;
            mRowCount = (int)Math.Ceiling((double)(((float)mThumbCount) / ((float)mColCount)));
            mImageWidth = (Math.Min(mColCount, mThumbCount) * (mTileWidth + mOptions.GapWidth)) + mOptions.GapWidth;
            mImageHeight = (mRowCount * (mTileHeight + mOptions.GapHeight + mLabelHeight)) + mOptions.GapHeight;
        }

        private void UpdateResult()
        {
            int width = Math.Max(mDetailWidth, mImageWidth);
            int height = mDetailHeight + mImageHeight;
            int detailHeight = 0;
            int gapWidth = mOptions.GapWidth;
            int gapHeight = mOptions.GapHeight;
            if (mResultBitmap != null)
            {
                mResultBitmap.Dispose();
            }
            mResultBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            mResultBitmap.SetResolution(300, 300);
            Graphics graphics = Graphics.FromImage(mResultBitmap);

            Brush brush = new SolidBrush(mOptions.BackColor);
            graphics.FillRectangle(brush, new Rectangle(0, 0, width, height));
            brush.Dispose();
            if (File.Exists(mOptions.BackgroundImage))
            {
                try
                {
                    Bitmap image = (Bitmap)Image.FromFile(mOptions.BackgroundImage);
                    image.SetResolution(300,300);
                    graphics.DrawImageUnscaled(image, 0, 0);
                    image.Dispose();
                }
                catch
                {
                }
            }
            if (mOptions.Details)
            {
                switch (mOptions.DetailPosition)
                {
                    case 0:
                        detailHeight = mDetailHeight;
                        break;

                    case 1:
                        detailHeight = mDetailHeight;
                        gapWidth = (width - mDetailWidth) - mOptions.GapWidth;
                        break;

                    case 2:
                        gapHeight = height - mDetailHeight;
                        break;

                    case 3:
                        gapWidth = (width - mDetailWidth) - mOptions.GapWidth;
                        gapHeight = height - mDetailHeight;
                        break;
                }
                brush = new SolidBrush(mOptions.DetailFontColor);
                graphics.DrawString(mDetailText, mOptions.DetailFont, brush, (float)gapWidth, (float)gapHeight);
                brush.Dispose();
            }
            if (mOptions.DropShadow)
            {
                for (int j = 0; j < mThumbCount; j++)
                {
                    int num7 = j % mColCount;
                    int num8 = j / mColCount;
                    Rectangle rect = new Rectangle((num7 * (mTileWidth + mOptions.GapWidth)) + mOptions.GapWidth, (num8 * (mTileHeight + mOptions.GapHeight)) + mOptions.GapHeight, mThumbWidth, mThumbHeight);
                    rect.Offset((((mTileWidth - mThumbWidth) / 2) - 5) + mOptions.OffsetX, ((((mTileHeight - mThumbHeight) / 2) - 5) + mOptions.OffsetY) + detailHeight);
                    graphics.DrawImageUnscaled(mShadowTemplate, rect);
                }
            }
            for (int i = 0; i < mThumbCount; i++)
            {
                int num10 = i % mColCount;
                int num11 = i / mColCount;
                graphics.DrawImageUnscaled(mThumbnailsList[i].ToBitmap(),
                    ((num10 * (mTileWidth + mOptions.GapWidth)) + mOptions.GapWidth) + ((mTileWidth - mThumbWidth) / 2),
                    (((num11 * (mTileHeight + mLabelHeight + mOptions.GapHeight)) + mOptions.GapHeight) + ((mTileHeight - mThumbHeight) / 2)) + detailHeight);
            }
            if (mOptions.Labels)
            {
                StringFormat format = new StringFormat();
                format.Trimming = StringTrimming.EllipsisCharacter;
                switch (mOptions.InfoPositions)
                {
                    case 0:
                        format.Alignment = StringAlignment.Near;
                        format.LineAlignment = StringAlignment.Near;
                        break;

                    case 1:
                        format.Alignment = StringAlignment.Far;
                        format.LineAlignment = StringAlignment.Near;
                        break;

                    case 2:
                        format.Alignment = StringAlignment.Near;
                        format.LineAlignment = StringAlignment.Far;
                        break;

                    case 3:
                        format.Alignment = StringAlignment.Far;
                        format.LineAlignment = StringAlignment.Far;
                        break;
                }
                brush = new SolidBrush(mOptions.InfoFontColor);
                for (int k = 0; k < mThumbCount; k++)
                {
                    int num13 = k % mColCount;
                    int num14 = k / mColCount;
                    Rectangle layoutRectangle = new Rectangle((num13 * (mTileWidth + mOptions.GapWidth)) + mOptions.GapWidth, (num14 * (mTileHeight + mOptions.GapHeight + mLabelHeight)) + mOptions.GapHeight, mThumbWidth, mThumbHeight);
                    layoutRectangle.Offset((mTileWidth - mThumbWidth) / 2, ((mTileHeight - mThumbHeight) / 2) + mLabelHeight + detailHeight);
                    graphics.DrawString((string)mLabels[k], mOptions.InfoFont, brush, layoutRectangle, format);
                }
                brush.Dispose();
            }
            graphics.Dispose();
            OnUpdate();
        }

        private void UpdateShadowTemplate()
        {
            if (mShadowTemplate != null)
            {
                mShadowTemplate.Dispose();
            }
            Bitmap shadowTemplate = new Bitmap(mThumbWidth + 10, mThumbHeight + 10, PixelFormat.Format32bppArgb);
            shadowTemplate.SetResolution(300,300);
            Graphics graphics = Graphics.FromImage(shadowTemplate);
            SolidBrush brush = new SolidBrush(Color.FromArgb((int)(mOptions.ShadowIntensity * 2.55f), 0, 0, 0));
            graphics.FillRectangle(brush, 5, 5, mThumbWidth, mThumbHeight);
            brush.Dispose();
            MagickImage mahd = new MagickImage(shadowTemplate);
            mahd.Blur();
            mShadowTemplate = mahd.ToBitmap();
            //IFilter filter = new GaussianBlur();
            //mShadowTemplate = filter.Apply(shadowTemplate);
            graphics.Dispose();
            shadowTemplate.Dispose();
        }

        private void UpdateTileSize()
        {
            mTileWidth = mOptions.TileWidth;
            mTileHeight = mOptions.TileHeight;
            MagickImage mScreenshot = mScreenshots[0];
            //Bitmap bitmap = (Bitmap)mScreenshots[0];
            float scale = Math.Min((float)mTileWidth / mScreenshot.Width, (float)mTileHeight / mScreenshot.Height);
            mThumbWidth = Convert.ToInt32(mScreenshot.Width * scale);
            mThumbHeight = Convert.ToInt32(mScreenshot.Height * scale);
            if (mOptions.AutoFitTiles)
            {
                mTileWidth = mThumbWidth;
                mTileHeight = mThumbHeight;
            }
        }

        public InterpolationMode Interpolation
        {
            get
            {
                return mInterpolation;
            }
            set
            {
                mInterpolation = value;
                ResizeScreenshots();
                UpdateResult();
            }
        }

        public Bitmap Result
        {
            get
            {
                return mResultBitmap;
            }
        }
    }
}
