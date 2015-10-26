using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fenrir.Src.Helper
{
    /// <summary>
    /// Configuration file
    /// </summary>
    class Config
    {
        private System.Xml.XmlDocument configDoc;
        private String configPath = @"Content/config.xml";

        /// <summary>
        /// The language name
        /// </summary>
        public String Language
        {
            get { return this.configDoc.GetElementsByTagName("language")[0].InnerText; }
            set { this.configDoc.GetElementsByTagName("language")[0].InnerText = value; configDoc.Save(configPath); FenrirGame.Instance.Properties.ContentManager.ReloadLanguageFiles(); }
        }

        /// <summary>
        /// The width of the window
        /// </summary>
        public int ResolutionX
        {
            get { return Convert.ToInt32(configDoc.GetElementsByTagName("resolutionX")[0].InnerText); }
            set { this.configDoc.GetElementsByTagName("resolutionX")[0].InnerText = value.ToString(); configDoc.Save(configPath); }
        }

        /// <summary>
        /// The height of the window
        /// </summary>
        public int ResolutionY
        {
            get { return Convert.ToInt32(configDoc.GetElementsByTagName("resolutionY")[0].InnerText); }
            set { this.configDoc.GetElementsByTagName("resolutionY")[0].InnerText = value.ToString(); configDoc.Save(configPath); }
        }

        public int WindowMode
        {
            get { return Convert.ToInt32(configDoc.GetElementsByTagName("windowmode")[0].InnerText); }
            set { this.configDoc.GetElementsByTagName("windowmode")[0].InnerText = value.ToString(); configDoc.Save(configPath); }
        }

        public Config()
        {
            this.configDoc = new System.Xml.XmlDocument();
            this.configDoc.Load(configPath);
        }
    }
}
