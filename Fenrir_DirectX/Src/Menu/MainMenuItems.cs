using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fenrir.Src.Helper;
using Fenrir.Src.Helper.UI;
using Fenrir.Src.Helper.UI.Alignment;

namespace Fenrir.Src.Menu
{
    class MainMenuItems : Group
    {
        /// <summary>
        /// button to start a new game
        /// </summary>
        TextButton newGameButton;

        /// <summary>
        /// button to go to the options
        /// </summary>
        TextButton optionsButton;

        /// <summary>
        /// button to quit the game
        /// </summary>
        TextButton exitButton;

        public MainMenuItems()
            : base(Horizontal.Center, Vertical.Top)
        {
            this.newGameButton = new TextButton(
                "#mainmenu_newgame#",
                DataIdentifier.defaultFontLarge,
                Horizontal.Center, Vertical.Top);

            this.optionsButton = new TextButton(
                "#mainmenu_options#",
                DataIdentifier.defaultFontLarge,
                Horizontal.Center, Vertical.Top);

            this.exitButton = new TextButton(
                "#mainmenu_exit#",
                DataIdentifier.defaultFontLarge,
                Horizontal.Center, Vertical.Top);

            this.newGameButton.onClick += new EventHandler(HandleNewGameMenuClick);
            this.optionsButton.onClick += new EventHandler(HandleOptionMenuClick);
            this.exitButton.onClick += new EventHandler(HandleExitMenuClick);

            this.AddUiElement(new Microsoft.Xna.Framework.Vector2(0, 0), this.newGameButton);
            this.AddUiElement(new Microsoft.Xna.Framework.Vector2(0, 60), this.optionsButton);
            this.AddUiElement(new Microsoft.Xna.Framework.Vector2(0, 120), this.exitButton);


            this.ResetPosition(new Microsoft.Xna.Framework.Vector2(0, 430));
        }

        private void HandleNewGameMenuClick(object sender, EventArgs e)
        {
            FenrirGame.Instance.Properties.RequestNewGameState(GameState.LoadGame);
        }

        private void HandleOptionMenuClick(object sender, EventArgs e)
        {
            FenrirGame.Instance.Properties.RequestNewGameState(GameState.OptionMenu);
        }

        private void HandleExitMenuClick(object sender, EventArgs e)
        {
            FenrirGame.Instance.Exit();
        }
    }
}
