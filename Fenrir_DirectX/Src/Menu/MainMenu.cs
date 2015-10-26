using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fenrir.Src.Helper;
using Fenrir.Src.Helper.UI;
using Fenrir.Src.Helper.UI.Alignment;

namespace Fenrir.Src.Menu
{
    /// <summary>
    /// the main menu
    /// </summary>
    class MainMenu
    {
        Image background;

        private MainMenuItems itemsMainMenu;
        private OptionMenuItems itemsOptionMenu;
        private Header header;

        public MainMenu() 
        {
            this.background = new Image(DataIdentifier.textureMainMenuBackground, Horizontal.Left, Vertical.Top, new Microsoft.Xna.Framework.Vector2(), new Microsoft.Xna.Framework.Vector2(FenrirGame.Instance.Properties.ScreenWidth, FenrirGame.Instance.Properties.ScreenHeight));
            this.background.IsTile = true;

            this.itemsMainMenu = new MainMenuItems();
            this.itemsOptionMenu = new OptionMenuItems();
            this.header = new Header();
        }


        public void Update()
        {
            this.header.Update();

            switch (FenrirGame.Instance.Properties.CurrentGameState)
            {
                case GameState.MainMenu :
                    this.itemsMainMenu.Update();
                    break;
                  
                case GameState.OptionMenu :
                    this.itemsOptionMenu.Update();
                    break;
            }
        }

        /// <summary>
        /// draw the main menu
        /// </summary>
        public void Draw()
        {
            this.background.Draw();

            this.header.Draw();

            // Content
            switch (FenrirGame.Instance.Properties.CurrentGameState)
            {
                case GameState.MainMenu:
                    this.itemsMainMenu.Draw();
                    break;

                case GameState.OptionMenu:
                    this.itemsOptionMenu.Draw();
                    break;
            }
        }
    }
}
