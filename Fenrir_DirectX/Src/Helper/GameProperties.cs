using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fenrir.Src.Helper
{
    /// <summary>
    /// current state of the game
    /// </summary>
    class GameProperties
    {
        #region variables
        private GameState currentGameState;
        /// <summary>
        /// the current game state
        /// </summary>
        public GameState CurrentGameState
        {
            get { return currentGameState; }
            private set { currentGameState = value; }
        }

        private InputHandler input;
        /// <summary>
        /// All the input
        /// </summary>
        internal InputHandler Input
        {
            get { return input; }
            private set { input = value; }
        }

        private Microsoft.Xna.Framework.GameTime currentGameTime;
        /// <summary>
        /// the current game time
        /// </summary>
        public Microsoft.Xna.Framework.GameTime CurrentGameTime
        {
            get { return currentGameTime; }
            private set { currentGameTime = value; }
        }

        private ContentManager contentManager;
        /// <summary>
        /// Asset handler
        /// </summary>
        internal ContentManager ContentManager
        {
            get { return contentManager; }
            private set { contentManager = value; }
        }

        /// <summary>
        /// the screen height
        /// </summary>
        public int ScreenHeight
        {
            get { return graphicDeviceManager.PreferredBackBufferHeight; }
            private set { }
        }

        /// <summary>
        /// the screen width
        /// </summary>
        public int ScreenWidth
        {
            get { return graphicDeviceManager.PreferredBackBufferWidth; }
            private set { }
        }

        private Boolean isFullscreen;
        /// <summary>
        /// is the game in fullscreenmode or not?
        /// </summary>
        public Boolean IsFullscreen
        {
            get { return isFullscreen; }
            private set { isFullscreen = value; }
        }

        private Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch;
        /// <summary>
        /// the batch to draw sprites with
        /// </summary>
        public Microsoft.Xna.Framework.Graphics.SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            private set { spriteBatch = value; }
        }

        private Microsoft.Xna.Framework.GraphicsDeviceManager graphicDeviceManager;
        /// <summary>
        /// access to the graphics device
        /// </summary>
        public Microsoft.Xna.Framework.GraphicsDeviceManager GraphicDeviceManager
        {
            get { return graphicDeviceManager; }
            private set { graphicDeviceManager = value; }
        }

        private String selectedLanguage;
        /// <summary>
        /// the language to be loaded
        /// </summary>
        public String SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                selectedLanguage = value;
                this.contentManager.ReloadLanguageFiles();
            }
        }

        private Microsoft.Xna.Framework.Graphics.Effect baseShader;
        /// <summary>
        /// the basic shader for rendering
        /// </summary>
        public Microsoft.Xna.Framework.Graphics.Effect BaseShader
        {
            get { return baseShader; }
            set { baseShader = value; }
        }

        private Microsoft.Xna.Framework.GameWindow gameWindow;

        #endregion

        #region events

        public event EventHandler onResolutionChanged;

        #endregion

        /// <summary>
        /// Setup the game properties
        /// </summary>
        /// <param name="contentManager">the XNA content manager to be used</param>
        /// <param name="graphicDeviceManager">the XNA graphic manager to be used</param>
        public GameProperties(Microsoft.Xna.Framework.Content.ContentManager contentManager, Microsoft.Xna.Framework.GraphicsDeviceManager graphicDeviceManager, Microsoft.Xna.Framework.GameWindow window)
        {
            // load game properties
            System.Xml.XmlDocument config = new System.Xml.XmlDocument();
            config.Load(@"Content/config.xml");

            // try resolution
            try
            {
                graphicDeviceManager.PreferredBackBufferWidth = Convert.ToInt32(config.GetElementsByTagName("resolutionX")[0].InnerText);
                graphicDeviceManager.PreferredBackBufferHeight = Convert.ToInt32(config.GetElementsByTagName("resolutionY")[0].InnerText);
                Screen screen = Screen.PrimaryScreen;
                window.Position = new Microsoft.Xna.Framework.Point(screen.Bounds.Width / 2 - graphicDeviceManager.PreferredBackBufferWidth / 2, screen.Bounds.Height / 2 - graphicDeviceManager.PreferredBackBufferHeight / 2);
            }
            catch (Exception)
            {
                graphicDeviceManager.PreferredBackBufferWidth = 1280;
                graphicDeviceManager.PreferredBackBufferHeight = 720;
            }


            // try fullscreen
            try
            {
                int fullscreen = Convert.ToInt32(config.GetElementsByTagName("fullscreen")[0].InnerText);
                if (fullscreen == 1)
                {
                    Screen screen = Screen.PrimaryScreen;
                    window.IsBorderless = true;
                    window.Position = new Microsoft.Xna.Framework.Point(screen.Bounds.X, screen.Bounds.Y);
                    graphicDeviceManager.PreferredBackBufferWidth = screen.Bounds.Width;
                    graphicDeviceManager.PreferredBackBufferHeight = screen.Bounds.Height;
                    this.isFullscreen = true;
                }
                else
                {
                    this.isFullscreen = false;
                }
            }
            catch (Exception) { }

            graphicDeviceManager.ApplyChanges();

            // try language settings
            try
            {
                this.SelectedLanguage = config.GetElementsByTagName("language")[0].InnerText;
            }
            catch(Exception){ }


            this.currentGameState = GameState.LoadMenu;
            this.input = new InputHandler();
            
            this.contentManager = new Helper.ContentManager(contentManager);
            this.spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(graphicDeviceManager.GraphicsDevice);
            this.graphicDeviceManager = graphicDeviceManager;
            this.gameWindow = window;

            System.IO.BinaryReader Reader = new System.IO.BinaryReader(System.IO.File.Open(@"Content\Shader\main_shader.mgfxo", System.IO.FileMode.Open));
            this.baseShader = new Microsoft.Xna.Framework.Graphics.Effect(this.graphicDeviceManager.GraphicsDevice, Reader.ReadBytes((int)Reader.BaseStream.Length)); 
        }

        /// <summary>
        /// switch the displaymode to fullscreen or windows
        /// </summary>
        /// <param name="toFullscreen">true for fullscreen, false for windowed</param>
        public void toggleFullscreen(bool toFullscreen)
        {
            System.Xml.XmlDocument config = new System.Xml.XmlDocument();
            config.Load(@"Content/config.xml");

            if (toFullscreen && !this.isFullscreen)
            {
                Screen screen = Screen.PrimaryScreen;
                this.gameWindow.IsBorderless = true;
                this.gameWindow.Position = new Microsoft.Xna.Framework.Point(screen.Bounds.X, screen.Bounds.Y);
                this.graphicDeviceManager.PreferredBackBufferWidth = screen.Bounds.Width;
                this.graphicDeviceManager.PreferredBackBufferHeight = screen.Bounds.Height;
                this.graphicDeviceManager.ApplyChanges();

                try
                {
                    config.GetElementsByTagName("fullscreen")[0].InnerText = "1";
                }
                catch (Exception) 
                {
                    FenrirGame.Instance.Log(LogLevel.Warn, "Failed to save fullscreen setting");
                }
                
                this.isFullscreen = true;
                this.ResolutionChanged();
            }
            else if (!toFullscreen && this.isFullscreen)
            {
                Screen screen = Screen.PrimaryScreen;
                this.gameWindow.IsBorderless = false;

                try
                {
                    graphicDeviceManager.PreferredBackBufferWidth = Convert.ToInt32(config.GetElementsByTagName("resolutionX")[0].InnerText);
                    graphicDeviceManager.PreferredBackBufferHeight = Convert.ToInt32(config.GetElementsByTagName("resolutionY")[0].InnerText);
                }
                catch (Exception)
                {
                    graphicDeviceManager.PreferredBackBufferWidth = 1280;
                    graphicDeviceManager.PreferredBackBufferHeight = 720;
                }
                this.graphicDeviceManager.ApplyChanges();

                this.gameWindow.Position = new Microsoft.Xna.Framework.Point(screen.Bounds.Width / 2 - graphicDeviceManager.PreferredBackBufferWidth / 2, screen.Bounds.Height / 2 - graphicDeviceManager.PreferredBackBufferHeight / 2);

                try
                {
                    config.GetElementsByTagName("fullscreen")[0].InnerText = "0";
                }
                catch (Exception) 
                {
                    FenrirGame.Instance.Log(LogLevel.Warn, "Failed to save fullscreen setting");
                }
                this.isFullscreen = false;
                this.ResolutionChanged();
            }

            config.Save(@"Content/config.xml");
        }

        /// <summary>
        /// changes the resolution
        /// </summary>
        /// <param name="width">new width</param>
        /// <param name="height">new height</param>
        public void changeResolution(int width, int height)
        {
            if (!this.isFullscreen)
            {
                System.Xml.XmlDocument config = new System.Xml.XmlDocument();
                config.Load(@"Content/config.xml");

                FenrirGame.Instance.Properties.GraphicDeviceManager.PreferredBackBufferWidth = width;
                FenrirGame.Instance.Properties.GraphicDeviceManager.PreferredBackBufferHeight = height;
                FenrirGame.Instance.Properties.GraphicDeviceManager.ApplyChanges();

                Screen screen = Screen.PrimaryScreen;
                this.gameWindow.Position = new Microsoft.Xna.Framework.Point(screen.Bounds.Width / 2 - width / 2, screen.Bounds.Height / 2 - height / 2);

                config.GetElementsByTagName("resolutionX")[0].InnerText = width.ToString();
                config.GetElementsByTagName("resolutionY")[0].InnerText = height.ToString();

                try
                {
                    config.GetElementsByTagName("fullscreen")[0].InnerText = "0";
                    config.Save(@"Content/config.xml");
                }
                catch (Exception)
                {
                    FenrirGame.Instance.Log(LogLevel.Warn, "Failed to save fullscreen setting");
                }

                this.ResolutionChanged();
            }
        }

        /// <summary>
        /// update the component
        /// </summary>
        /// <param name="newTime"></param>
        public void Update(Microsoft.Xna.Framework.GameTime newTime)
        {
            this.currentGameTime = newTime;
            this.input.Update();
        }

        /// <summary>
        /// request a new gamemode
        /// </summary>
        /// <param name="requestedMode">the mode requested</param>
        /// <returns>true if the mode got accepted or is already active</returns>
        public Boolean RequestNewGameState(GameState requestedMode)
        {
            if(this.currentGameState == requestedMode)
                return true;

            Boolean acceptable = false;
            switch (requestedMode)
            {
                case GameState.MainMenu :
                    if (this.currentGameState == GameState.OptionMenu || this.currentGameState == GameState.LoadMenu)
                        acceptable = true;
                    break;

                case GameState.OptionMenu :
                    if (this.currentGameState == GameState.MainMenu)
                        acceptable = true;
                    break;

                case GameState.LoadGame:
                    if (this.currentGameState == GameState.MainMenu)
                        acceptable = true;
                    break;

                case GameState.InGame:
                    if (this.currentGameState == GameState.LoadGame || this.currentGameState == GameState.Paused)
                        acceptable = true;
                    break;

                case GameState.Paused:
                    if (this.currentGameState == GameState.InGame)
                        acceptable = true;
                    break;

                default: 
                    break;
            }

            if(acceptable)
                this.currentGameState = requestedMode;
            return acceptable;
        }

        /// <summary>
        /// Resolution changed handler
        /// </summary>
        protected void ResolutionChanged()
        {
            EventHandler handler = this.onResolutionChanged;
            if (handler != null)
                handler(this, new EventArgs());
        }
    }
}
