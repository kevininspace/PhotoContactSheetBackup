using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ImageBackup.Properties;

namespace ImageBackup
{
    [Serializable]
    internal sealed class Options : ICloneable
    {
        private readonly string mGlobalOptionsPath = Path.Combine("Options.grb"); //Settings.SettingsDirectory, "Options.grb");

        private bool mAutoFitTiles = true;
        private Color mBackColor = Color.WhiteSmoke;
        private string mBackgroundImage = String.Empty;
        private int mColumnCount = 5;
        private Font mDetailFont = new Font("Microsoft Sans Serif", 12.25f, GraphicsUnit.Document);
        private Color mDetailFontColor = Color.Black;
        private int mDetailPosition;
        private bool mDetails = true;
        private string mDetailText = "Path: [DIRECTORYPATH]\r\nNumber of images: [NUMBER_OF_IMAGES]\r\nDate: [DATE]";
        private bool mDropShadow = true;
        private int mGapHeight = 10;
        private int mGapWidth = 10;
        private Font mInfoFont = new Font("Microsoft Sans Serif", 9.00f, GraphicsUnit.Document);
        private Color mInfoFontColor = Color.Black;
        private int mInfoPosition = 2;
        private long mJpegQuality = 90L;
        private bool mLabels = true;
        private int mOffsetX = 3;
        private int mOffsetY = 3;
        private int mShadowIntensity = 50;
        private int mTileHeight = 120;
        private int mTileWidth = 160;

        [field: NonSerialized]
        public event EventHandler AutoFitTilesChanged;

        [field: NonSerialized]
        public event EventHandler BackColorChanged;

        [field: NonSerialized]
        public event EventHandler BackgroundImageChanged;

        [field: NonSerialized]
        public event EventHandler ColumnCountChanged;

        [field: NonSerialized]
        public event EventHandler DetailFontChanged;

        [field: NonSerialized]
        public event EventHandler DetailPositionChanged;

        [field: NonSerialized]
        public event EventHandler DetailsChanged;

        [field: NonSerialized]
        public event EventHandler DetailTextChanged;

        [field: NonSerialized]
        public event EventHandler DropShadowChanged;

        [field: NonSerialized]
        public event EventHandler GapHeightChanged;

        [field: NonSerialized]
        public event EventHandler GapWidthChanged;

        [field: NonSerialized]
        public event EventHandler InfoFontChanged;

        [field: NonSerialized]
        public event EventHandler InfoPositionsChanged;

        [field: NonSerialized]
        public event EventHandler LabelsChanged;

        [field: NonSerialized]
        public event EventHandler OffsetXChanged;

        [field: NonSerialized]
        public event EventHandler OffsetYChanged;

        [field: NonSerialized]
        public event EventHandler OptionsChanged;

        [field: NonSerialized]
        public event EventHandler ShadowIntensityChanged;

        [field: NonSerialized]
        public event EventHandler TileHeightChanged;

        [field: NonSerialized]
        public event EventHandler TileWidthChanged;

        public object Clone()
        {
            Options options = (Options)MemberwiseClone();
            options.DetailFont = (Font)mDetailFont.Clone();
            options.InfoFont = (Font)mInfoFont.Clone();
            return options;
        }

        public void CopyFrom(Options mOptions)
        {
            mColumnCount = mOptions.ColumnCount;
            mTileWidth = mOptions.TileWidth;
            mTileHeight = mOptions.TileHeight;
            mGapWidth = mOptions.GapWidth;
            mGapHeight = mOptions.GapHeight;
            mOffsetX = mOptions.OffsetX;
            mOffsetY = mOptions.OffsetY;
            mShadowIntensity = mOptions.ShadowIntensity;
            mInfoPosition = mOptions.InfoPositions;
            mDetailPosition = mOptions.DetailPosition;
            mJpegQuality = mOptions.JpegQuality;
            mAutoFitTiles = mOptions.AutoFitTiles;
            mDropShadow = mOptions.DropShadow;
            mLabels = mOptions.Labels;
            mDetails = mOptions.Details;
            mBackColor = mOptions.BackColor;
            mInfoFontColor = mOptions.InfoFontColor;
            mDetailFontColor = mOptions.DetailFontColor;
            mBackgroundImage = mOptions.BackgroundImage;
            mDetailText = mOptions.DetailText;
            mDetailFont = mOptions.DetailFont;
            mInfoFont = mOptions.InfoFont;
            OnOptionsChanged();
        }

        public void LoadOptions(string path)
        {
            Options options;
            IFormatter formatter = new BinaryFormatter();
            try
            {
                Stream serializationStream = File.Open(path, FileMode.Open);
                options = (Options)formatter.Deserialize(serializationStream);
                serializationStream.Close();
            }
            catch
            {
                options = new Options();
            }
            CopyFrom(options);
        }

        public void LoadOptions()
        {
            LoadOptions(mGlobalOptionsPath);
        }

        private void OnAutoFitTilesChanged()
        {
            if (AutoFitTilesChanged != null)
            {
                AutoFitTilesChanged(this, EventArgs.Empty);
            }
        }

        private void OnBackColorChanged()
        {
            if (BackColorChanged != null)
            {
                BackColorChanged(this, EventArgs.Empty);
            }
        }

        private void OnBackgroundImageChanged()
        {
            if (BackgroundImageChanged != null)
            {
                BackgroundImageChanged(this, EventArgs.Empty);
            }
        }

        private void OnColumnCountChanged()
        {
            if (ColumnCountChanged != null)
            {
                ColumnCountChanged(this, EventArgs.Empty);
            }
        }

        private void OnDetailFontChanged()
        {
            if (DetailFontChanged != null)
            {
                DetailFontChanged(this, EventArgs.Empty);
            }
        }

        private void OnDetailPositionChanged()
        {
            if (DetailPositionChanged != null)
            {
                DetailPositionChanged(this, EventArgs.Empty);
            }
        }

        private void OnDetailsChanged()
        {
            if (DetailsChanged != null)
            {
                DetailsChanged(this, EventArgs.Empty);
            }
        }

        private void OnDetailTextChanged()
        {
            if (DetailTextChanged != null)
            {
                DetailTextChanged(this, EventArgs.Empty);
            }
        }

        private void OnDropShadowChanged()
        {
            if (DropShadowChanged != null)
            {
                DropShadowChanged(this, EventArgs.Empty);
            }
        }

        private void OnGapHeightChanged()
        {
            if (GapHeightChanged != null)
            {
                GapHeightChanged(this, EventArgs.Empty);
            }
        }

        private void OnGapWidthChanged()
        {
            if (GapWidthChanged != null)
            {
                GapWidthChanged(this, EventArgs.Empty);
            }
        }

        private void OnInfoFontChanged()
        {
            if (InfoFontChanged != null)
            {
                InfoFontChanged(this, EventArgs.Empty);
            }
        }

        private void OnInfoPositionsChanged()
        {
            if (InfoPositionsChanged != null)
            {
                InfoPositionsChanged(this, EventArgs.Empty);
            }
        }

        private void OnLabelsChanged()
        {
            if (LabelsChanged != null)
            {
                LabelsChanged(this, EventArgs.Empty);
            }
        }

        private void OnOffsetXChanged()
        {
            if (OffsetXChanged != null)
            {
                OffsetXChanged(this, EventArgs.Empty);
            }
        }

        private void OnOffsetYChanged()
        {
            if (OffsetYChanged != null)
            {
                OffsetYChanged(this, EventArgs.Empty);
            }
        }

        private void OnOptionsChanged()
        {
            if (OptionsChanged != null)
            {
                OptionsChanged(this, EventArgs.Empty);
            }
        }

        private void OnShadowIntensityChanged()
        {
            if (ShadowIntensityChanged != null)
            {
                ShadowIntensityChanged(this, EventArgs.Empty);
            }
        }

        private void OnTileHeightChanged()
        {
            if (TileHeightChanged != null)
            {
                TileHeightChanged(this, EventArgs.Empty);
            }
        }

        private void OnTileWidthChanged()
        {
            if (TileWidthChanged != null)
            {
                TileWidthChanged(this, EventArgs.Empty);
            }
        }

        public void SaveOptions(string path)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream serializationStream = File.Open(path, FileMode.OpenOrCreate);
            formatter.Serialize(serializationStream, this);
            serializationStream.Close();
        }

        public void SaveOptions()
        {
            SaveOptions(mGlobalOptionsPath);
        }

        public void setDefaults()
        {
            mColumnCount = 5;
            mTileWidth = 160;
            mTileHeight = 120;
            mGapWidth = 10;
            mGapHeight = 10;
            mOffsetX = 3;
            mOffsetY = 3;
            mShadowIntensity = 50;
            mInfoPosition = 2;
            mDetailPosition = 0;
            mJpegQuality = 90L;
            mAutoFitTiles = true;
            mDropShadow = true;
            mLabels = true;
            mDetails = true;
            mBackColor = Color.FromArgb(0xee, 0xf2, 0xf7);
            mInfoFontColor = Color.White;
            mDetailFontColor = Color.Black;
            mBackgroundImage = String.Empty;
            mDetailText = "File Name: [FILE_NAME]\r\nFile Size: [FILE_SIZE]\r\nResolution: [WIDTH]x[HEIGHT]\r\nDuration: [DURATION]";
            mDetailFont = new Font("Microsoft Sans Serif", 8.25f);
            mInfoFont = new Font("Microsoft Sans Serif", 8.25f);
            OnOptionsChanged();
        }

        public bool AutoFitTiles
        {
            get
            {
                return mAutoFitTiles;
            }
            set
            {
                mAutoFitTiles = value;
                OnAutoFitTilesChanged();
            }
        }

        public Color BackColor
        {
            get
            {
                return mBackColor;
            }
            set
            {
                mBackColor = value;
                OnBackColorChanged();
            }
        }

        public string BackgroundImage
        {
            get
            {
                return mBackgroundImage;
            }
            set
            {
                mBackgroundImage = value;
                OnBackgroundImageChanged();
            }
        }

        public int ColumnCount
        {
            get
            {
                return mColumnCount;
            }
            set
            {
                mColumnCount = value;
                OnColumnCountChanged();
            }
        }

        public Font DetailFont
        {
            get
            {
                return mDetailFont;
            }
            set
            {
                mDetailFont = value;
                OnDetailFontChanged();
            }
        }

        public Color DetailFontColor
        {
            get
            {
                return mDetailFontColor;
            }
            set
            {
                mDetailFontColor = value;
            }
        }

        public int DetailPosition
        {
            get
            {
                return mDetailPosition;
            }
            set
            {
                mDetailPosition = value;
                OnDetailPositionChanged();
            }
        }

        public bool Details
        {
            get
            {
                return mDetails;
            }
            set
            {
                mDetails = value;
                OnDetailsChanged();
            }
        }

        public string DetailText
        {
            get
            {
                return mDetailText;
            }
            set
            {
                mDetailText = value;
                OnDetailTextChanged();
            }
        }

        public bool DropShadow
        {
            get
            {
                return mDropShadow;
            }
            set
            {
                mDropShadow = value;
                OnDropShadowChanged();
            }
        }

        public int GapHeight
        {
            get
            {
                return mGapHeight;
            }
            set
            {
                mGapHeight = value;
                OnGapHeightChanged();
            }
        }

        public int GapWidth
        {
            get
            {
                return mGapWidth;
            }
            set
            {
                mGapWidth = value;
                OnGapWidthChanged();
            }
        }

        public Font InfoFont
        {
            get
            {
                return mInfoFont;
            }
            set
            {
                mInfoFont = value;
                OnInfoFontChanged();
            }
        }

        public Color InfoFontColor
        {
            get
            {
                return mInfoFontColor;
            }
            set
            {
                mInfoFontColor = value;
            }
        }

        public int InfoPositions
        {
            get
            {
                return mInfoPosition;
            }
            set
            {
                mInfoPosition = value;
                OnInfoPositionsChanged();
            }
        }

        public long JpegQuality
        {
            get
            {
                return mJpegQuality;
            }
            set
            {
                mJpegQuality = value;
            }
        }

        public bool Labels
        {
            get
            {
                return mLabels;
            }
            set
            {
                mLabels = value;
                OnLabelsChanged();
            }
        }

        public int OffsetX
        {
            get
            {
                return mOffsetX;
            }
            set
            {
                mOffsetX = value;
                OnOffsetXChanged();
            }
        }

        public int OffsetY
        {
            get
            {
                return mOffsetY;
            }
            set
            {
                mOffsetY = value;
                OnOffsetYChanged();
            }
        }

        public int ShadowIntensity
        {
            get
            {
                return mShadowIntensity;
            }
            set
            {
                mShadowIntensity = value;
                OnShadowIntensityChanged();
            }
        }

        public int TileHeight
        {
            get
            {
                return mTileHeight;
            }
            set
            {
                mTileHeight = value;
                OnTileHeightChanged();
            }
        }

        public int TileWidth
        {
            get
            {
                return mTileWidth;
            }
            set
            {
                mTileWidth = value;
                OnTileWidthChanged();
            }
        }
    }
}
