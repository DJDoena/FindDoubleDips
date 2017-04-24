using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using DoenaSoft.DVDProfiler.DVDProfilerHelper;
using DoenaSoft.DVDProfiler.DVDProfilerXML.Version390;
using Invelos.DVDProfilerPlugin;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace DoenaSoft.DVDProfiler.FindDoubleDips
{
    internal partial class MainForm : Form
    {
        private const String XslResource = "DoenaSoft.DVDProfiler.FindDoubleDips.DoubleDips.xsl";

        #region Fields

        private IDVDProfilerAPI Api;

        private ProgressWindow ProgressWindow;

        private Boolean CanClose;

        private Collection Collection;

        private static XmlSerializer s_XmlSerializer;

        private static XmlSerializer XmlSerializer
        {
            get
            {
                if (s_XmlSerializer == null)
                {
                    s_XmlSerializer = new XmlSerializer(typeof(Collection), new[] { typeof(DoubleDipDVD) });
                }

                return (s_XmlSerializer);
            }
        }

        #endregion

        #region Constructor
        public MainForm(IDVDProfilerAPI api)
        {
            Api = api;

            CanClose = true;

            InitializeComponent();
        }

        #endregion

        #region Form Events

        private void OnCheckForUpdateToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            CheckForNewVersion();
        }

        private void OnEnglishToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            Plugin.Settings.DefaultValues.UiCulture = CultureInfo.GetCultureInfo("en-US").LCID;

            MessageBox.Show(MessageBoxTexts.LanguageChange, MessageBoxTexts.InformationHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnGermamToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            Plugin.Settings.DefaultValues.UiCulture = CultureInfo.GetCultureInfo("de-DE").LCID;

            MessageBox.Show(MessageBoxTexts.LanguageChange, MessageBoxTexts.InformationHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnReadMeToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            OpenReadme();
        }

        private void OnAboutToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            using (AboutBox aboutBox = new AboutBox(GetType().Assembly))
            {
                aboutBox.ShowDialog();
            }
        }

        private void OnMainFormLoad(Object sender
            , EventArgs e)
        {
            SuspendLayout();

            LayoutForm();

            AddColumns(PurchasesDataGridView);

            ResumeLayout();

            String currentVersion = GetType().Assembly.GetName().Version.ToString();

            if (Plugin.Settings.CurrentVersion != currentVersion)
            {
                OpenReadme();

                Plugin.Settings.CurrentVersion = currentVersion;
            }
        }

        private void OnMainFormClosing(Object sender
            , FormClosingEventArgs e)
        {
            if (CanClose == false)
            {
                e.Cancel = true;

                return;
            }

            Plugin.Settings.MainForm.Left = Left;
            Plugin.Settings.MainForm.Top = Top;
            Plugin.Settings.MainForm.Width = Width;
            Plugin.Settings.MainForm.Height = Height;
            Plugin.Settings.MainForm.WindowState = WindowState;
            Plugin.Settings.MainForm.RestoreBounds = RestoreBounds;

            Plugin.Settings.DefaultValues.CheckOnlyTitles = CheckTitlesRadioButton.Checked;
            Plugin.Settings.DefaultValues.IgnoreProductionYear = IgnoreProductionYearCheckBox.Checked;
            Plugin.Settings.DefaultValues.IgnoreWishListTitles = IgnoreWishListTitlesCheckBox.Checked;
            Plugin.Settings.DefaultValues.IgnoreTelevisonTitles = IgnoreTelevisonTitlesCheckBox.Checked;
            Plugin.Settings.DefaultValues.IgnoreSameDatePurchases = IgnoreSameDatePurchasesCheckBox.Checked;
            Plugin.Settings.DefaultValues.IgnoreBoxSetContents = IgnoreBoxSetContentsCheckBox.Checked;
        }

        private void OnMarkProfilesToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            Api.FlagAllDVDs(false);

            foreach (DataGridViewRow row in PurchasesDataGridView.Rows)
            {
                String id = ((DVD)(row.Tag)).ID;

                Api.FlagDVDByProfileID(id, true);
            }
        }

        private void OnMarkSelectedProfilesToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            Api.FlagAllDVDs(false);

            foreach (DataGridViewRow row in PurchasesDataGridView.Rows)
            {
                if (RowIsSelected(row))
                {
                    String id = ((DVD)(row.Tag)).ID;

                    Api.FlagDVDByProfileID(id, true);
                }
            }
        }

        private static Boolean RowIsSelected(DataGridViewRow row)
        {
            Object value = row.Cells[0].Value;

            Boolean isSelected = ((value is Boolean) && ((Boolean)value));

            return (isSelected);
        }

        private IEnumerable<DataGridViewRow> GetRowsForSave(Boolean mustBeSelected)
        {
            foreach (DataGridViewRow row in PurchasesDataGridView.Rows)
            {
                if ((mustBeSelected == false) || (RowIsSelected(row)))
                {
                    yield return (row);
                }
            }
        }

        private void OnSaveDoubleDipsFileToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            List<DataGridViewRow> rows = GetRowsForSave(false).ToList();

            SaveToXml(rows);
        }

        private void SaveToXml(List<DataGridViewRow> rows)
        {
            if (rows.Count == 0)
            {
                MessageBox.Show(MessageBoxTexts.NothingToSave, MessageBoxTexts.WarningHeader, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = "DoubleDips.xml";
                sfd.OverwritePrompt = true;
                sfd.RestoreDirectory = true;
                sfd.Filter = "Double Dips file|*.xml";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Collection collection = GetCollectionForExport(rows);

                    try
                    {
                        SaveToXml(sfd.FileName, collection);

                        MessageBox.Show(MessageBoxTexts.Done, MessageBoxTexts.InformationHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeWritten, sfd.FileName, ex.Message), MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private Collection GetCollectionForExport(List<DataGridViewRow> rows)
        {
            Collection collection = new Collection();

            List<DVD> dvds = new List<DVD>(rows.Count);

            Int32 currentGroup = (Int32)(rows.First().Cells[1].Tag);

            Boolean odd = true;

            foreach (DataGridViewRow row in rows)
            {
                DoubleDipDVD export = new DoubleDipDVD();

                DVD dvd = (DVD)(row.Tag);

                export.ID = dvd.ID;
                export.Title = dvd.Title;
                export.OriginalTitle = dvd.OriginalTitle;
                export.SortTitle = dvd.SortTitle;
                export.UPC = dvd.UPC;
                export.Edition = dvd.Edition;
                export.ProductionYear = dvd.ProductionYear;
                export.CollectionType = dvd.CollectionType;
                export.PurchaseInfo = dvd.PurchaseInfo;
                export.MediaTypes = dvd.MediaTypes;
                export.AudioList = dvd.AudioList;
                export.SubtitleList = dvd.SubtitleList;
                export.RegionList = dvd.RegionList;
                export.ID_LocalityDesc = dvd.ID_LocalityDesc;
                export.BoxSet = dvd.BoxSet;

                Int32 rowGroup = (Int32)(row.Cells[1].Tag);

                if (rowGroup != currentGroup)
                {
                    currentGroup = rowGroup;

                    odd = (odd == false);
                }

                export.Colour = ConvertToHex(odd ? Color.LightBlue : Color.Empty);

                dvds.Add(export);
            }

            collection.DVDList = dvds.ToArray();

            return (collection);
        }

        private void OnSaveSelectedDoubleDipsFileToolStripMenuItemClick(Object sender
                , EventArgs e)
        {
            List<DataGridViewRow> rows = GetRowsForSave(true).ToList();

            SaveToXml(rows);
        }

        private static void SaveToXml(String fileName
            , Collection collection)
        {
            FileInfo xmlfileName = new FileInfo(fileName);

            String xslFileName = Path.GetFileNameWithoutExtension(xmlfileName.FullName) + ".xsl";

            using (FileStream fs = new FileStream(xmlfileName.FullName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                Encoding encoding = Encoding.GetEncoding(1252);

                using (XmlTextWriter xmlWriter = new XmlTextWriter(fs, encoding))
                {
                    xmlWriter.Formatting = Formatting.Indented;
                    xmlWriter.WriteProcessingInstruction("xml", $"version=\"1.0\" encoding=\"{encoding.WebName}\"");
                    xmlWriter.WriteProcessingInstruction("xml-stylesheet", $"type=\"text/xsl\" href=\"{xslFileName}\"");

                    XmlSerializer.Serialize(xmlWriter, collection);
                }
            }

            xslFileName = Path.Combine(xmlfileName.DirectoryName, xslFileName);

            using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(XslResource))
            {
                using (StreamReader resourceReader = new StreamReader(resourceStream))
                {
                    using (FileStream fs = new FileStream(xslFileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                        {
                            sw.Write(resourceReader.ReadToEnd());
                        }
                    }
                }
            }
        }

        private void OnOpenDoubleDipsFileToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.CheckFileExists = true;
                ofd.Filter = "DoubleDips.xml|*.xml";
                ofd.Multiselect = false;
                ofd.RestoreDirectory = true;
                ofd.FileName = "DoubleDips.xml";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ReadCollectionFile(ofd.FileName, false);
                }
            }
        }

        private void OnCurrentCellDirtyStateChanged(Object sender
            , EventArgs e)
        {
            if (PurchasesDataGridView.IsCurrentCellDirty)
            {
                PurchasesDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void OnStartSearchingButtonClick(Object sender
            , EventArgs e)
        {
            CanClose = false;

            UseWaitCursor = true;

            Cursor = Cursors.WaitCursor;

            Enabled = false;

            PurchasesDataGridView.Rows.Clear();

            if (Collection == null)
            {
                ProgressWindow = new ProgressWindow();
                ProgressWindow.ProgressBar.Minimum = 0;
                ProgressWindow.ProgressBar.Step = 1;
                ProgressWindow.CanClose = false;

                Object[] allIds = (Object[])(Api.GetAllProfileIDs());

                ProgressWindow.ProgressBar.Maximum = allIds.Length;
                ProgressWindow.Show();

                if (TaskbarManager.IsPlatformSupported)
                {
                    TaskbarManager.Instance.OwnerHandle = Handle;
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                    TaskbarManager.Instance.SetProgressValue(0, ProgressWindow.ProgressBar.Maximum);
                }

                Thread thread = new Thread(new ParameterizedThreadStart(ThreadRun));

                thread.IsBackground = false;
                thread.Start(new[] { allIds });
            }
            else
            {
                ThreadFinished(Collection);
            }
        }

        private void ThreadRun(Object param)
        {
            Collection collection = null;

            try
            {
                Object[] allIds = (Object[])(((Object[])param)[0]);

                List<DVD> dvdList = new List<DVD>(allIds.Length);

                for (Int32 i = 0; i < allIds.Length; i++)
                {
                    Func<String> getProfileData = () => GetProfileData(allIds[i]);

                    String xml = (String)(Invoke(getProfileData));

                    DVD dvd = Serializer<DVD>.FromString(xml, DVD.DefaultEncoding);

                    dvdList.Add(dvd);

                    Action updateProgressBar = () => UpdateProgressBar();

                    Invoke(updateProgressBar);
                }

                collection = new Collection();

                collection.DVDList = dvdList.ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Action threadFinished = () => ThreadFinished(collection);

                Invoke(threadFinished);
            }
        }

        private void OnSaveDoupleDipsAsHTMLToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            List<DataGridViewRow> rows = GetRowsForSave(false).ToList();

            SaveToHtml(rows);
        }

        private void OnSaveDoubleDipsOfSelectedRowsAsHTMLToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            List<DataGridViewRow> rows = GetRowsForSave(true).ToList();

            SaveToHtml(rows);
        }

        private void SaveToHtml(List<DataGridViewRow> rows)
        {
            if (rows.Count == 0)
            {
                MessageBox.Show(MessageBoxTexts.NothingToSave, MessageBoxTexts.WarningHeader, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = "DoubleDips.html";
                sfd.OverwritePrompt = true;
                sfd.RestoreDirectory = true;
                sfd.Filter = "HTML file|*.html";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Collection collection = GetCollectionForExport(rows);

                    try
                    {
                        SaveToHtml(sfd.FileName, collection);

                        MessageBox.Show(MessageBoxTexts.Done, MessageBoxTexts.InformationHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeWritten, sfd.FileName, ex.Message), MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SaveToHtml(String fileName
            , Collection collection)
        {
            XslCompiledTransform transform = new XslCompiledTransform();

            using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(XslResource))
            {
                using (XmlReader xmlReader = XmlReader.Create(resourceStream))
                {
                    transform.Load(xmlReader);
                }
            }

            Encoding encoding = Encoding.UTF8;

            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlTextWriter xtw = new XmlTextWriter(ms, encoding))
                {
                    XmlSerializer.Serialize(xtw, collection);

                    ms.Seek(0, SeekOrigin.Begin);

                    using (XmlReader reader = XmlReader.Create(ms))
                    {
                        using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                        {
                            using (StreamWriter sw = new StreamWriter(fs, encoding))
                            {
                                transform.Transform(reader, null, sw);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Helper Functions

        private static String ConvertToHex(Color colour)
            => ((colour.IsEmpty == false) ? ("#" + colour.R.ToString("X2") + colour.G.ToString("X2") + colour.B.ToString("X2")) : "#FFFFFF");

        private String GetProfileData(Object id)
        {
            IDVDInfo dvdInfo;
            Api.DVDByProfileID(out dvdInfo, (id).ToString(), -1, 0);

            String xml = dvdInfo.GetXML(true);

            return (xml);
        }

        private void ThreadFinished(Collection collection)
        {
            if (TaskbarManager.IsPlatformSupported)
            {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                TaskbarManager.Instance.OwnerHandle = IntPtr.Zero;
            }

            if (ProgressWindow != null)
            {
                ProgressWindow.CanClose = true;
                ProgressWindow.Close();
                ProgressWindow.Dispose();

                ProgressWindow = null;
            }

            Collection = collection;

            if (collection != null)
            {
                ReadCollection(collection, true);
            }

            Enabled = true;

            Cursor = Cursors.Default;

            UseWaitCursor = false;

            CanClose = true;
        }

        private void UpdateProgressBar()
        {
            ProgressWindow.ProgressBar.PerformStep();

            if (TaskbarManager.IsPlatformSupported)
            {
                TaskbarManager.Instance.SetProgressValue(ProgressWindow.ProgressBar.Value, ProgressWindow.ProgressBar.Maximum);
            }
        }

        private DataGridViewRow CreateRow()
        {
            DataGridViewRow row = new DataGridViewRow();

            for (Int32 i = 0; i < PurchasesDataGridView.Columns.Count; i++)
            {
                DataGridViewColumn column = PurchasesDataGridView.Columns[i];

                DataGridViewCell cell = (DataGridViewCell)(column.CellTemplate.Clone());

                row.Cells.Add(cell);
            }

            return (row);
        }

        private static DateTime GetPurchaseDateForSorting(DVD dvd)
            => ((dvd.PurchaseInfo?.DateSpecified == true) ? (dvd.PurchaseInfo.Date) : (new DateTime(9999, 12, 31)));

        private static void AddColumns(DataGridView dataGridView)
        {
            DataGridViewCheckBoxColumn selectionColumn = new DataGridViewCheckBoxColumn();

            selectionColumn.HeaderText = Texts.Selection;
            selectionColumn.Name = "Selection";
            selectionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            selectionColumn.ReadOnly = false;

            DataGridViewTextBoxColumn titleColumn = new DataGridViewTextBoxColumn();

            titleColumn.HeaderText = Texts.Title;
            titleColumn.Name = "Title";
            titleColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            titleColumn.ReadOnly = true;

            DataGridViewTextBoxColumn editionColumn = new DataGridViewTextBoxColumn();

            editionColumn.HeaderText = Texts.Edition;
            editionColumn.Name = "Edition";
            editionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            editionColumn.ReadOnly = true;

            DataGridViewTextBoxColumn yearColumn = new DataGridViewTextBoxColumn();

            yearColumn.HeaderText = Texts.ProductionYear;
            yearColumn.Name = "ProductionYear";
            yearColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            yearColumn.ReadOnly = true;

            DataGridViewTextBoxColumn mediaTypeColumn = new DataGridViewTextBoxColumn();

            mediaTypeColumn.HeaderText = Texts.MediaType;
            mediaTypeColumn.Name = "MediaType";
            mediaTypeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            mediaTypeColumn.ReadOnly = true;

            DataGridViewTextBoxColumn purchaseDateColumn = new DataGridViewTextBoxColumn();

            purchaseDateColumn.HeaderText = Texts.PurchaseDate;
            purchaseDateColumn.Name = "PurchaseDate";
            purchaseDateColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            purchaseDateColumn.ReadOnly = true;

            DataGridViewTextBoxColumn collectionTypeColumn = new DataGridViewTextBoxColumn();

            collectionTypeColumn.HeaderText = Texts.CollectionType;
            collectionTypeColumn.Name = "CollectionType";
            collectionTypeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            collectionTypeColumn.ReadOnly = true;

            DataGridViewCheckBoxColumn boxSetColumn = new DataGridViewCheckBoxColumn();

            boxSetColumn.HeaderText = Texts.BoxSet;
            boxSetColumn.Name = "BoxSet";
            boxSetColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            boxSetColumn.ReadOnly = true;

            DataGridViewTextBoxColumn originalTitleColumn = new DataGridViewTextBoxColumn();

            originalTitleColumn.HeaderText = Texts.OriginalTitle;
            originalTitleColumn.Name = "OriginalTitle";
            originalTitleColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            originalTitleColumn.ReadOnly = true;

            DataGridViewTextBoxColumn sortTitleColumn = new DataGridViewTextBoxColumn();

            sortTitleColumn.HeaderText = Texts.SortTitle;
            sortTitleColumn.Name = "SortTitle";
            sortTitleColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            sortTitleColumn.ReadOnly = true;

            DataGridViewTextBoxColumn upcColumn = new DataGridViewTextBoxColumn();

            upcColumn.HeaderText = Texts.UPC;
            upcColumn.Name = "UPC";
            upcColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            upcColumn.ReadOnly = true;

            DataGridViewTextBoxColumn localityColumn = new DataGridViewTextBoxColumn();

            localityColumn.HeaderText = Texts.Locality;
            localityColumn.Name = "Locality";
            localityColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            localityColumn.ReadOnly = true;

            DataGridViewTextBoxColumn regionColumn = new DataGridViewTextBoxColumn();

            regionColumn.HeaderText = Texts.Regions;
            regionColumn.Name = "Regions";
            regionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            regionColumn.ReadOnly = true;

            DataGridViewTextBoxColumn audioColumn = new DataGridViewTextBoxColumn();

            audioColumn.HeaderText = Texts.Audio;
            audioColumn.Name = "Audio";
            audioColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            audioColumn.ReadOnly = true;

            DataGridViewTextBoxColumn subtitleColumn = new DataGridViewTextBoxColumn();

            subtitleColumn.HeaderText = Texts.Subtitles;
            subtitleColumn.Name = "Subtitles";
            subtitleColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            subtitleColumn.ReadOnly = true;

            dataGridView.Columns.AddRange(selectionColumn
                , titleColumn
                , editionColumn
                , yearColumn
                , mediaTypeColumn
                , purchaseDateColumn
                , collectionTypeColumn
                , boxSetColumn
                , originalTitleColumn
                , sortTitleColumn
                , upcColumn
                , localityColumn
                , regionColumn
                , audioColumn
                , subtitleColumn);
        }

        private void CheckForNewVersion()
        {
            OnlineAccess.Init("Doena Soft.", "FindDoubleDips");
            OnlineAccess.CheckForNewVersion("http://doena-soft.de/dvdprofiler/3.9.0/versions.xml", this, "FindDoubleDips", GetType().Assembly);
        }

        private void LayoutForm()
        {
            if (Plugin.Settings.MainForm.WindowState == FormWindowState.Normal)
            {
                Left = Plugin.Settings.MainForm.Left;

                Top = Plugin.Settings.MainForm.Top;

                if (Plugin.Settings.MainForm.Width > MinimumSize.Width)
                {
                    Width = Plugin.Settings.MainForm.Width;
                }
                else
                {
                    Width = MinimumSize.Width;
                }

                if (Plugin.Settings.MainForm.Height > MinimumSize.Height)
                {
                    Height = Plugin.Settings.MainForm.Height;
                }
                else
                {
                    Height = MinimumSize.Height;
                }
            }
            else
            {
                Left = Plugin.Settings.MainForm.RestoreBounds.X;

                Top = Plugin.Settings.MainForm.RestoreBounds.Y;

                if (Plugin.Settings.MainForm.RestoreBounds.Width > MinimumSize.Width)
                {
                    Width = Plugin.Settings.MainForm.RestoreBounds.Width;
                }
                else
                {
                    Width = MinimumSize.Width;
                }

                if (Plugin.Settings.MainForm.RestoreBounds.Height > MinimumSize.Height)
                {
                    Height = Plugin.Settings.MainForm.RestoreBounds.Height;
                }
                else
                {
                    Height = MinimumSize.Height;
                }
            }

            if (Plugin.Settings.MainForm.WindowState != FormWindowState.Minimized)
            {
                WindowState = Plugin.Settings.MainForm.WindowState;
            }

            CheckTitlesRadioButton.Checked = Plugin.Settings.DefaultValues.CheckOnlyTitles;

            IgnoreProductionYearCheckBox.Checked = Plugin.Settings.DefaultValues.IgnoreProductionYear;

            IgnoreWishListTitlesCheckBox.Checked = Plugin.Settings.DefaultValues.IgnoreWishListTitles;

            IgnoreTelevisonTitlesCheckBox.Checked = Plugin.Settings.DefaultValues.IgnoreTelevisonTitles;

            IgnoreSameDatePurchasesCheckBox.Checked = Plugin.Settings.DefaultValues.IgnoreSameDatePurchases;

            IgnoreBoxSetContentsCheckBox.Checked = Plugin.Settings.DefaultValues.IgnoreBoxSetContents;
        }

        private void OpenReadme()
        {
            String helpFile = (new FileInfo(GetType().Assembly.Location)).DirectoryName + @"\Readme\readme.html";

            if (File.Exists(helpFile))
            {
                using (HelpForm helpForm = new HelpForm(helpFile))
                {
                    helpForm.Text = "Read Me";
                    helpForm.ShowDialog(this);
                }
            }
        }

        private void ReadCollectionFile(String fileName
            , Boolean filter)
        {
            UseWaitCursor = true;

            Cursor = Cursors.WaitCursor;

            try
            {
                PurchasesDataGridView.Rows.Clear();

                Collection collection;
                try
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        collection = (Collection)(XmlSerializer.Deserialize(fs));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeRead, fileName, ex.Message), MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                ReadCollection(collection, filter);
            }
            finally
            {
                Cursor = Cursors.Default;

                UseWaitCursor = false;
            }
        }

        private void ReadCollection(Collection collection
            , Boolean filter)
        {
            if (collection.DVDList?.Length > 0)
            {
                #region Phase 1: Add all relevant profiles using the filters

                Dictionary<String, List<DVD>> duplicates = new Dictionary<String, List<DVD>>(collection.DVDList.Length);

                foreach (DVD dvd in collection.DVDList)
                {
                    if (filter)
                    {
                        #region Ignore Wish List Titles

                        if ((IgnoreWishListTitlesCheckBox.Checked) && (dvd.CollectionType.Value == "Wish List"))
                        {
                            continue;
                        }

                        #endregion

                        #region Ignore Television Genre

                        if ((IgnoreTelevisonTitlesCheckBox.Checked) && (dvd.GenreList != null))
                        {
                            Boolean found = false;

                            foreach (String genre in dvd.GenreList)
                            {
                                if (genre == "Television")
                                {
                                    found = true;

                                    break;
                                }
                            }

                            if (found)
                            {
                                continue;
                            }
                        }

                        #endregion
                    }

                    #region Create Title Key

                    String key;
                    if ((CheckOriginalTitlesRadioButton.Checked) && (String.IsNullOrEmpty(dvd.OriginalTitle) == false))
                    {
                        key = dvd.OriginalTitle;
                    }
                    else
                    {
                        key = dvd.Title;
                    }

                    if (filter)
                    {
                        if (IgnoreProductionYearCheckBox.Checked == false)
                        {
                            key += "_" + dvd.ProductionYear;
                        }
                    }

                    key = key.ToLower();

                    #endregion

                    if (duplicates.ContainsKey(key))
                    {
                        duplicates[key].Add(dvd);
                    }
                    else
                    {
                        List<DVD> list = new List<DVD>(2);

                        list.Add(dvd);

                        duplicates.Add(key, list);
                    }
                }

                #endregion

                #region Phase 2: Sort the entries and use same date purchase filter

                List<List<DVD>> sortedList = new List<List<DVD>>(duplicates.Count);

                List<DataGridViewRow> rows = new List<DataGridViewRow>();

                foreach (KeyValuePair<String, List<DVD>> kvp in duplicates)
                {
                    if ((filter) && (kvp.Value.Count < 2))
                    {
                        continue;
                    }

                    #region Ignore Same Date Purchases

                    if (filter)
                    {
                        if (IgnoreSameDatePurchasesCheckBox.Checked)
                        {
                            Boolean purchaseDatesIdentical = true;

                            DateTime initialPurchaseDate = GetPurchaseDateForSorting(kvp.Value[0]);

                            for (Int32 i = 1; i < kvp.Value.Count; i++)
                            {
                                if (initialPurchaseDate != GetPurchaseDateForSorting(kvp.Value[i]))
                                {
                                    purchaseDatesIdentical = false;

                                    break;
                                }
                            }

                            if (purchaseDatesIdentical)
                            {
                                continue;
                            }
                        }
                    }

                    #endregion

                    #region Ignore Box Set Contents

                    if (filter)
                    {
                        if (IgnoreBoxSetContentsCheckBox.Checked)
                        {
                            Boolean allBoxSetContents = true;

                            for (Int32 i = 0; i < kvp.Value.Count; i++)
                            {
                                if (String.IsNullOrEmpty(kvp.Value[i].BoxSet.Parent))
                                {
                                    allBoxSetContents = false;

                                    break;
                                }
                            }

                            if (allBoxSetContents)
                            {
                                continue;
                            }
                        }
                    }

                    #endregion

                    #region Sort Inner List

                    kvp.Value.Sort(CompareForSorting);

                    sortedList.Add(kvp.Value);

                    #endregion
                }

                #region Sort Outer List

                sortedList.Sort((left, right) => left[0].SortTitle.CompareTo(right[0].SortTitle));

                #endregion

                #endregion

                #region Phase 3: Add entries to grid

                Boolean odd = true;

                for (Int32 i = 0; i < sortedList.Count; i++)
                {
                    List<DVD> list = sortedList[i];

                    foreach (DVD dvd in list)
                    {
                        DataGridViewRow row = CreateRow();

                        if (odd)
                        {
                            row.DefaultCellStyle.BackColor = Color.LightBlue;
                        }

                        row.Cells[1].Tag = i;

                        row.Cells[1].Value = dvd.Title;
                        row.Cells[2].Value = dvd.Edition;
                        row.Cells[3].Value = GetProductionYear(dvd);
                        row.Cells[4].Value = GetMediaTypes(dvd);
                        row.Cells[5].Value = GetPurchaseDateString(dvd);
                        row.Cells[6].Value = GetCollectionType(dvd);
                        row.Cells[7].Value = GetBoxSetInfo(dvd);
                        row.Cells[8].Value = dvd.OriginalTitle;
                        row.Cells[9].Value = dvd.SortTitle;
                        row.Cells[10].Value = dvd.UPC;
                        row.Cells[11].Value = dvd.ID_LocalityDesc;
                        row.Cells[12].Value = GetRegion(dvd);
                        row.Cells[13].Value = GetAudio(dvd);
                        row.Cells[14].Value = GetSubtitles(dvd);

                        row.Tag = dvd;

                        rows.Add(row);
                    }

                    odd = (odd == false);
                }

                PurchasesDataGridView.Rows.AddRange(rows.ToArray());

                #endregion
            }

            MessageBox.Show(MessageBoxTexts.Done, MessageBoxTexts.InformationHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static Int32 CompareForSorting(DVD left
            , DVD right)
        {
            Int32 compare = GetPurchaseDateForSorting(left).CompareTo(GetPurchaseDateForSorting(right));

            if (compare == 0)
            {
                compare = left.SortTitle.CompareTo(right.SortTitle);
            }

            if (compare == 0)
            {
                compare = left.CollectionType.Value.CompareTo(right.CollectionType.Value);
            }

            if (compare == 0)
            {
                compare = left.Edition.CompareTo(right.Edition);
            }

            return (compare);
        }

        private static String GetProductionYear(DVD dvd)
            => ((dvd.ProductionYear != 0) ? (dvd.ProductionYear.ToString()) : (String.Empty));

        private static String GetMediaTypes(DVD dvd)
        {
            List<String> mediaTypes = new List<String>(4);

            if (dvd.MediaTypes.DVD)
            {
                mediaTypes.Add("DVD");
            }

            if (dvd.MediaTypes.BluRay)
            {
                mediaTypes.Add("Blu-ray");
            }

            if (dvd.MediaTypes.HDDVD)
            {
                mediaTypes.Add("HD-DVD");
            }

            if (String.IsNullOrEmpty(dvd.MediaTypes.CustomMediaType) == false)
            {
                mediaTypes.Add(dvd.MediaTypes.CustomMediaType);
            }

            String result = String.Join(",", mediaTypes.ToArray());

            return (result);
        }

        private static String GetPurchaseDateString(DVD dvd)
            => ((dvd.PurchaseInfo?.DateSpecified == true) ? (dvd.PurchaseInfo.Date.ToString("d", CultureInfo.CurrentCulture)) : (String.Empty));

        private String GetCollectionType(DVD dvd)
        {
            switch (dvd.CollectionType.Value)
            {
                case ("Owned"):
                    {
                        return (Texts.Owned);
                    }
                case ("Ordered"):
                    {
                        return (Texts.Ordered);
                    }
                case ("Wish List"):
                    {
                        return (Texts.WishList);
                    }
                default:
                    {
                        return (dvd.CollectionType.Value);
                    }
            }
        }

        private Boolean GetBoxSetInfo(DVD dvd)
            => (String.IsNullOrEmpty(dvd.BoxSet.Parent) == false);

        private String GetRegion(DVD dvd)
        {
            IEnumerable<String> list = dvd.RegionList.Select(item => item);

            String regions = String.Join(", ", list.ToArray());

            return (regions);
        }

        private String GetAudio(DVD dvd)
        {
            IEnumerable<String> list = dvd.AudioList.Select(item => item.Content);

            String audio = String.Join(", ", list.ToArray());

            return (audio);
        }

        private String GetSubtitles(DVD dvd)
        {
            IEnumerable<String> list = dvd.SubtitleList.Select(item => item);

            String subtitles = String.Join(", ", list.ToArray());

            return (subtitles);
        }

        #endregion
    }
}