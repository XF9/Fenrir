using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fenrir.Src.Helper;

namespace Fenrir.Src.LoadingScreen
{
    /// <summary>
    /// initial loading screen
    /// </summary>
    class LoadMenu : LoadingScreen
    {
        public LoadMenu() { }

        /// <summary>
        /// load all the assets
        /// </summary>
        public override void Load()
        {
            FenrirGame.Instance.Config = new Config();

            // Fonts
            FenrirGame.Instance.Properties.ContentManager.AddFontToLibrary(DataIdentifier.defaultFontSmall, "Font/Trade_Winds_Small");
            FenrirGame.Instance.Properties.ContentManager.AddFontToLibrary(DataIdentifier.defaultFont, "Font/Trade_Winds_Regular");
            FenrirGame.Instance.Properties.ContentManager.AddFontToLibrary(DataIdentifier.defaultFontLarge, "Font/Trade_Winds_Large");

            // Main Menu
            FenrirGame.Instance.Properties.ContentManager.AddTextureToLibrary(DataIdentifier.textureMainMenuBackground, "Texture/MainMenu/Background");
            FenrirGame.Instance.Properties.ContentManager.AddTextureToLibrary(DataIdentifier.textureMainMenuTitle, "Texture/MainMenu/Title");

            FenrirGame.Instance.Properties.ContentManager.ReloadLanguageFiles();

            // Create Gamestates
            FenrirGame.Instance.InGame = new InGame.ComponentManager();
            FenrirGame.Instance.Menu = new Menu.MainMenu();

            // done - go to menu
            FenrirGame.Instance.Properties.RequestNewGameState(GameState.MainMenu);
        }
    }
}
