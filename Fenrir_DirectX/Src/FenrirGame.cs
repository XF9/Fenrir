using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fenrir.Src.Helper;
using Fenrir.Src.Menu;
using Fenrir.Src.InGame;

namespace Fenrir.Src
{
    enum LogLevel
    {
        Info,
        Warn,
        Error
    }
    /// <summary>
    /// The heart of the game.
    /// </summary>
    class FenrirGame
    {

        private Renderer renderer;
        /// <summary>
        /// the renderer .. to render stuff
        /// </summary>
        internal Renderer Renderer
        {
            get { return renderer; }
            set { renderer = value; }
        }

        private ComponentManager inGame;
        /// <summary>
        /// Access to the ingame properties
        /// </summary>
        internal ComponentManager InGame
        {
            get { return inGame; }
            set { inGame = value; }
        }

        private MainMenu menu;
        /// <summary>
        /// Access to the mainmenu
        /// </summary>
        internal MainMenu Menu
        {
            get { return menu; }
            set { menu = value; }
        }

        private GameProperties properties;
        /// <summary>
        /// General game properties
        /// </summary>
        internal GameProperties Properties
        {
            get { return properties; }
            private set { properties = value; }
        }

        private Config config;
        /// <summary>
        /// Configuration values
        /// </summary>
        internal Config Config
        {
            get { return config; }
            set { if (config == null) { config = value; } }
        }

        private static FenrirGame instance;
        /// <summary>
        /// Access to the game instance
        /// </summary>
        internal static FenrirGame Instance
        {
            get 
            {
                // return new instance if not set
                if (FenrirGame.instance == null)
                    FenrirGame.instance = new FenrirGame();
                return instance; 
            }
            set { 
                instance = value; 
            }
        }

        private Cursor cursor;
        /// <summary>
        /// the cursor
        /// </summary>
        internal Cursor Cursor
        {
            get { return cursor; }
            set { cursor = value; }
        }

        /// <summary>
        /// loading screen for the menu
        /// </summary>
        LoadingScreen.LoadMenu menuLoader;

        /// <summary>
        /// loading screen for the main game
        /// </summary>
        LoadingScreen.LoadGame gameLoader;

        /// <summary>
        /// XNA Game instance
        /// </summary>
        private Microsoft.Xna.Framework.Game game;


        private FenrirGame(){ }

        /// <summary>
        /// initialize verything
        /// required to run this
        /// </summary>
        /// <param name="contentManager">the XNA content Manager</param>
        /// <param name="graphicDeviceManager">the XNA graphic manager</param>
        /// /// <param name="displayModes">possible display modes</param>
        public void Run(Microsoft.Xna.Framework.Content.ContentManager contentManager, Microsoft.Xna.Framework.GraphicsDeviceManager graphicDeviceManager, Microsoft.Xna.Framework.GameWindow window, Microsoft.Xna.Framework.Game game)
        {
            this.Properties = new GameProperties(contentManager, graphicDeviceManager, window);

            // load bare media to initialize loadingscreen
            // Cursor textures
            FenrirGame.Instance.Properties.ContentManager.AddTextureToLibrary(DataIdentifier.textureCursorRegular, "Texture/Cursor/Default");
            FenrirGame.Instance.Properties.ContentManager.AddTextureToLibrary(DataIdentifier.textureCursorCamera, "Texture/Cursor/Camera");

            this.game = game;

            this.renderer = new Renderer();

            this.cursor = new Cursor();

            this.menuLoader = new LoadingScreen.LoadMenu();
            this.gameLoader = new LoadingScreen.LoadGame();
        }

        /// <summary>
        /// Distributes the update call
        /// </summary>
        /// <param name="gameTime">the current gamne time</param>
        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //if (this.init && this.game.IsActive)
            if (this.game.IsActive)
            {
                // update the properties in any case
                this.Properties.Update(gameTime);
                this.cursor.Update();

                // update the in game handler or the menu handler depending on the current game state
                switch (this.Properties.CurrentGameState)
                {
                    case GameState.LoadMenu:
                        this.menuLoader.Load();
                        break;
                    case GameState.LoadGame:
                        this.gameLoader.Load();
                        break;
                    case GameState.MainMenu:
                    case GameState.OptionMenu:
                        this.Menu.Update();
                        break;
                    case GameState.InGame:
                    case GameState.Paused:
                        this.InGame.Update();
                        break;
                }
            }
        }

        public void Draw()
        {
            this.renderer.Prepare();

            // update the in game handler or the menu handler depending on the current game state
            switch (this.Properties.CurrentGameState)
            {
                case GameState.LoadMenu:
                    this.menuLoader.Draw();
                    break;
                case GameState.MainMenu:
                case GameState.OptionMenu:
                    this.Menu.Draw();
                    break;
                case GameState.InGame:
                case GameState.Paused:
                    this.inGame.Draw();
                    break;
            }

            this.Cursor.Draw();

            this.renderer.Flush();
        }

        /// <summary>
        /// Quit the game
        /// </summary>
        public void Exit()
        {
            this.game.Exit();
        }

        /// <summary>
        /// Log a certain event or message
        /// </summary>
        /// <param name="level">the loglevel</param>
        /// <param name="message">the message to be logged</param>
        public void Log(LogLevel level, String message)
        {
            String levelString = "";
            switch (level)
            {
                case LogLevel.Info:
                    levelString = "INFO"; break;
                case LogLevel.Warn:
                    levelString = "WARN"; break;
                case LogLevel.Error:
                    levelString = "ERROR"; break;
            }
            System.Diagnostics.Debug.WriteLine(this.properties.CurrentGameTime.TotalGameTime.TotalSeconds + " -- " + levelString + ": " + message);
        }
    }
}
