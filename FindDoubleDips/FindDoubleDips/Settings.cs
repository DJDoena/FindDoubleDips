using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace DoenaSoft.DVDProfiler.FindDoubleDips
{
    [ComVisible(false)]
    public class Settings
    {
        public SizableForm MainForm;

        public String CurrentVersion;

        public DefaultValues DefaultValues;

        private static XmlSerializer s_XmlSerializer;

        [XmlIgnore]
        public static XmlSerializer XmlSerializer
        {
            get
            {
                if (s_XmlSerializer == null)
                {
                    s_XmlSerializer = new XmlSerializer(typeof(Settings));
                }
                return (s_XmlSerializer);
            }
        }

        public static void Serialize(String fileName, Settings instance)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (XmlTextWriter xtw = new XmlTextWriter(fs, Encoding.UTF8))
                {
                    xtw.Formatting = Formatting.Indented;
                    XmlSerializer.Serialize(xtw, instance);
                }
            }
        }

        public void Serialize(String fileName)
        {
            Serialize(fileName, this);
        }

        public static Settings Deserialize(String fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (XmlTextReader xtr = new XmlTextReader(fs))
                {
                    Settings instance = (Settings)(XmlSerializer.Deserialize(xtr));

                    return (instance);
                }
            }
        }
    }

    [ComVisible(false)]
    public class BaseForm
    {
        public Int32 Top = 50;

        public Int32 Left = 50;
    }

    [ComVisible(false)]
    public class SizableForm : BaseForm
    {
        public Int32 Height = 500;

        public Int32 Width = 900;

        public FormWindowState WindowState = FormWindowState.Normal;

        public Rectangle RestoreBounds;
    }
}