using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace Fenrir.Src.Helper
{
    /// <summary>
    /// Manages all loaded Assets
    /// </summary>
    class ContentManager
    {
        /// <summary>
        /// Datastorage for fonts
        /// </summary>
        private Dictionary<String, SpriteFont> fontDatabase;

        /// <summary>
        /// Datastorage for textures
        /// </summary>
        private Dictionary<String, Texture2D> textureDatabase;

        /// <summary>
        /// Datastorage for models
        /// </summary>
        private Dictionary<String, Model> modelDatabase;

        private List<String> languages;
        /// <summary>
        /// List of supported languages
        /// </summary>
        public List<String> Languages
        {
            get { return languages; }
            private set { languages = value; }
        }
        private Dictionary<String, String> localizationDatabase;
        
        /// <summary>
        /// Random number generator
        /// </summary>
        private Random randomizer;

        /// <summary>
        /// XNA content manager
        /// </summary>
        Microsoft.Xna.Framework.Content.ContentManager xnaContentManager;

        /// <summary>
        /// Maganger to store all sorts of contents
        /// </summary>
        /// <param name="xnaContentManager">the xna content manager</param>
        public ContentManager(Microsoft.Xna.Framework.Content.ContentManager xnaContentManager)
        {
            this.fontDatabase           = new Dictionary<string, SpriteFont>();
            this.textureDatabase        = new Dictionary<string, Texture2D>();
            this.modelDatabase          = new Dictionary<string, Model>();
            this.languages              = new List<string>();
            this.localizationDatabase   = new Dictionary<string, string>();
            this.randomizer             = new Random();

            this.xnaContentManager      = xnaContentManager;
            
            System.Xml.XmlDocument languages = new System.Xml.XmlDocument();
            foreach (String language in System.IO.Directory.GetDirectories(@"Content/Locale"))
            {
                this.languages.Add(language.Substring(15, language.Length - 15));   // 15 = Content/Locale/
            }
        }

        /// <summary>
        /// Reloads all localizations for the currently selected language
        /// </summary>
        public void ReloadLanguageFiles()
        {
            this.localizationDatabase.Clear();

            if (this.languages.Contains(FenrirGame.Instance.Properties.SelectedLanguage))
            {
                System.Xml.XmlDocument xmlFile = new System.Xml.XmlDocument();
                try
                {
                    xmlFile.Load(@"Content/Locale/" + FenrirGame.Instance.Properties.SelectedLanguage + "/main_menu.xml");
                    foreach (System.Xml.XmlNode text in xmlFile.DocumentElement)
                        this.localizationDatabase.Add(text["key"].InnerText, text["value"].InnerText);

                    xmlFile.Load(@"Content/Locale/" + FenrirGame.Instance.Properties.SelectedLanguage + "/ingame.xml");
                    foreach (System.Xml.XmlNode text in xmlFile.DocumentElement)
                        this.localizationDatabase.Add(text["key"].InnerText, text["value"].InnerText);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("WARING: failed to load localization data for: " + FenrirGame.Instance.Properties.SelectedLanguage);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                }
            }
        }

        /// <summary>
        /// Add a font to the library
        /// </summary>
        /// <param name="name">name of the font</param>
        /// <param name="font">the font name</param>
        public void AddFontToLibrary(String name, String font)
        {
            this.fontDatabase.Add(name, this.xnaContentManager.Load<SpriteFont>(@font));
        }

        /// <summary>
        /// load a sprritefont
        /// </summary>
        /// <param name="name">name of the font to be loaded</param>
        /// <returns>the requested font or the default one if the font isn't loaded yet</returns>
        public Microsoft.Xna.Framework.Graphics.SpriteFont GetFont(String name)
        {
            return (this.fontDatabase.ContainsKey(name)) ? this.fontDatabase[name] : this.fontDatabase[DataIdentifier.defaultFont];
        }

        /// <summary>
        /// add a texture to the database
        /// </summary>
        /// <param name="name">the name of the texture</param>
        /// <param name="file">path to the texture</param>
        public void AddTextureToLibrary(String name, String file)
        {
            this.textureDatabase.Add(name, this.xnaContentManager.Load<Texture2D>(@file));
        }

        /// <summary>
        /// load a texture
        /// </summary>
        /// <param name="name">the name of the texture to be loaded</param>
        /// <returns>the requested texture or null</returns>
        public Microsoft.Xna.Framework.Graphics.Texture2D GetTexture(String name)
        {
            return (this.textureDatabase.ContainsKey(name)) ? this.textureDatabase[name] : null;
        }

        /// <summary>
        /// get the text for the givven ressource
        /// </summary>
        /// <param name="text">the resource</param>
        /// <returns>the string or the ressource if not found</returns>
        public String getLocalization(String text)
        {
            return (this.localizationDatabase.ContainsKey(text)) ? localizationDatabase[text] : text;
        }

        /// <summary>
        /// adds a model to the library
        /// </summary>
        /// <param name="name"></param>
        /// <param name="model"></param>
        public void AddModelToLibrary(String name, String model)
        {
            this.modelDatabase.Add(name, this.xnaContentManager.Load<Model>(@model));
        }

        /// <summary>
        /// get a model from the Database
        /// </summary>
        /// <param name="name">the model to be loaded</param>
        /// <returns>the model or null if not found</returns>
        public Microsoft.Xna.Framework.Graphics.Model getModel(String name)
        {
            return (this.modelDatabase.ContainsKey(name)) ? this.modelDatabase[name] : null;
        }

        /// <summary>
        /// generates a new random number
        /// </summary>
        /// <param name="small">min value</param>
        /// <param name="large">max value</param>
        /// <returns>random value</returns>
        public int randomize(int small, int large)
        {
            return this.randomizer.Next(small, large);
        }
    }
}
