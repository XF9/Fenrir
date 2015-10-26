using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fenrir.Src.Helper;
using Fenrir.Src.Helper.UI;
using Fenrir.Src.Helper.UI.Alignment;

namespace Fenrir.Src.InGame.Components
{
    /// <summary>
    /// the HUD for the game
    /// </summary>
    class Hud
    {
        //TODO: extract the pause menu [FEN-15]

        /// <summary>
        /// Toolbar field
        /// </summary>
        private Group toolBar;

        /// <summary>
        /// Block for displaying informations
        /// top - right corner
        /// </summary>
        private Group infoBlock;
        /// <summary>
        /// Block for displaying informations
        /// top - right corner
        /// </summary>
        internal Group InfoBlock
        {
            get { return infoBlock; }
            set { infoBlock = value; }
        }

        Helper.UI.TextButton resumeButton;
        Helper.UI.TextButton exitButton;
        List<Helper.UI.Image> backgroundTiles;
        Helper.UI.Image pasueMenuHeader;

        /// <summary>
        /// Create the HUD
        /// </summary>
        public Hud()
        {
            this.toolBar = new HUD.ToolBar();

            this.resumeButton = new Helper.UI.TextButton(
                "#hud_resume#",
                DataIdentifier.defaultFontLarge,
                Horizontal.Center, Vertical.Top);

            this.exitButton = new Helper.UI.TextButton(
                "#hud_exit#",
                DataIdentifier.defaultFontLarge,
                Horizontal.Center, Vertical.Top);
            
            this.resumeButton.onClick += new EventHandler(this.handleResumeClick);
            this.exitButton.onClick += new EventHandler(this.handleExitClick);

            this.pasueMenuHeader = new Helper.UI.Image(DataIdentifier.texturePauseMenuTitle, Horizontal.Center, Vertical.Top, new Microsoft.Xna.Framework.Vector2(0, 50));

            this.backgroundTiles = new List<Helper.UI.Image>();
            this.GenerateBackground();
            FenrirGame.Instance.Properties.onResolutionChanged += new EventHandler(GenerateBackground);
        }

        /// <summary>
        /// Generates the background tiles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateBackground(object sender = null, EventArgs e = null)
        {
            this.backgroundTiles.Clear();
            for (int x = 0; x <= Math.Floor((float)FenrirGame.Instance.Properties.ScreenWidth / FenrirGame.Instance.Properties.ContentManager.GetTexture(DataIdentifier.texturePauseMenuBackground).Width); x++)
                for (int y = 0; y <= Math.Floor((float)FenrirGame.Instance.Properties.ScreenHeight / FenrirGame.Instance.Properties.ContentManager.GetTexture(DataIdentifier.texturePauseMenuBackground).Height); y++)
                    this.backgroundTiles.Add(
                        new Helper.UI.Image(
                            DataIdentifier.texturePauseMenuBackground,
                            Horizontal.Left, Vertical.Top,
                            new Microsoft.Xna.Framework.Vector2(
                                FenrirGame.Instance.Properties.ContentManager.GetTexture(DataIdentifier.texturePauseMenuBackground).Width * x,
                                FenrirGame.Instance.Properties.ContentManager.GetTexture(DataIdentifier.texturePauseMenuBackground).Height * y)));
        }

        /// <summary>
        /// Update the HUD
        /// </summary>
        public void Update()
        {
            if (FenrirGame.Instance.Properties.CurrentGameState == GameState.InGame)
            {
                this.toolBar.Update();
                if (this.infoBlock != null) { this.infoBlock.Update(); }
            }
            else if (FenrirGame.Instance.Properties.CurrentGameState == GameState.Paused)
            {
                this.resumeButton.Update();
                this.exitButton.Update();
            }
        }

        /// <summary>
        /// Draw the HUDs
        /// </summary>
        public void Draw() {

            if (FenrirGame.Instance.Properties.CurrentGameState == GameState.InGame)
            {
                this.toolBar.Draw();
                if (this.infoBlock != null) { this.infoBlock.Draw(); }
            }
            else if (FenrirGame.Instance.Properties.CurrentGameState == GameState.Paused)
            {
                // Background
                foreach (Helper.UI.Image tile in this.backgroundTiles)
                    tile.Draw();

                this.pasueMenuHeader.Draw();
                this.resumeButton.Draw();
                this.exitButton.Draw();
            }
        }

        private void handleResumeClick(object sender, EventArgs e)
        {
            FenrirGame.Instance.Properties.RequestNewGameState(GameState.InGame);
        }

        private void handleExitClick(object sender, EventArgs e)
        {
            FenrirGame.Instance.Exit();
        }
    }
}
