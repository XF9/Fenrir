using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fenrir.Src.Helper;
using Fenrir.Src.Helper.UI;
using Fenrir.Src.Helper.UI.Alignment;

namespace Fenrir.Src.InGame.Components.HUD
{
    /// <summary>
    /// the HUD toolbar with the build tools
    /// </summary>
    class ToolBar : Group
    {
        /// <summary>
        /// Button to open the menu
        /// </summary>
        TextButton menuButton;

        /// <summary>
        /// Button to mine blocks
        /// </summary>
        ImageButton buildButton;

        /// <summary>
        /// Button to build a small cave
        /// </summary>
        ImageButton buildCaveSmallButton;

        /// <summary>
        /// Button to build a medium cave
        /// </summary>
        ImageButton buildCaveMediumButton;

        /// <summary>
        /// Button to build a large cave
        /// </summary>
        ImageButton buildCaveLargeButton;

        /// <summary>
        /// Button to remove build orders
        /// </summary>
        TextButton clearButton;

        /// <summary>
        /// Create a toolbar
        /// </summary>
        public ToolBar()
            : base(Horizontal.Left, Vertical.Bottom)
        {
            this.menuButton = new Helper.UI.TextButton(
                    "#hud_menu#",
                    DataIdentifier.defaultFont,
                    Horizontal.Left, Vertical.Bottom);

            this.buildButton = new Helper.UI.ImageButton(
                DataIdentifier.textureIconTunnel, 
                DataIdentifier.textureIconTunnelHover,
                Horizontal.Left, Vertical.Bottom,
                true);

            this.buildCaveSmallButton = new Helper.UI.ImageButton(
                DataIdentifier.textureIconSmallCave, 
                DataIdentifier.textureIconSmallCaveHover,
                Horizontal.Left, Vertical.Bottom,
                true);

            this.buildCaveMediumButton = new Helper.UI.ImageButton(
                DataIdentifier.textureIconMediumCave, 
                DataIdentifier.textureIconMediumCaveHover,
                Horizontal.Left, Vertical.Bottom,
                true);

            this.buildCaveLargeButton = new Helper.UI.ImageButton(
                DataIdentifier.textureIconLargeCave, 
                DataIdentifier.textureIconLargeCaveHover,
                Horizontal.Left, Vertical.Bottom,
                true);

            this.clearButton = new Helper.UI.TextButton(
                "#hud_clear_tunnel#",
                DataIdentifier.defaultFont,
                Horizontal.Left, Vertical.Bottom,
                true);

            this.buildButton.onClick += new EventHandler(this.handleBuildTunnelClick);
            this.buildCaveSmallButton.onClick += new EventHandler(this.handleBuildCaveSmallClick);
            this.buildCaveMediumButton.onClick += new EventHandler(this.handleBuildCaveMediumClick);
            this.buildCaveLargeButton.onClick += new EventHandler(this.handleBuildCaveLargeClick);
            this.clearButton.onClick += new EventHandler(this.handleClearTunnelClick);
            this.menuButton.onClick += new EventHandler(this.handleMenuClick);

            this.AddUiElement(new Microsoft.Xna.Framework.Vector2(50, -40), this.buildButton);
            this.AddUiElement(new Microsoft.Xna.Framework.Vector2(140, -40), this.buildCaveSmallButton);
            this.AddUiElement(new Microsoft.Xna.Framework.Vector2(210, -40), this.buildCaveMediumButton);
            this.AddUiElement(new Microsoft.Xna.Framework.Vector2(280, -40), this.buildCaveLargeButton);
            this.AddUiElement(new Microsoft.Xna.Framework.Vector2(360, -40), this.clearButton);
            this.AddUiElement(new Microsoft.Xna.Framework.Vector2(460, -40), this.menuButton);

            this.ResetPosition(new Microsoft.Xna.Framework.Vector2(0, 0));
        }

        private void handleBuildTunnelClick(object sender, EventArgs e)
        {
            FenrirGame.Instance.InGame.Scene.ActivateModeHandler(ModeHandler.BuildTunnel);
        }

        private void handleBuildCaveSmallClick(object sender, EventArgs e)
        {
            FenrirGame.Instance.InGame.Scene.ActivateModeHandler(ModeHandler.BuildCaveSmall);
        }

        private void handleBuildCaveMediumClick(object sender, EventArgs e)
        {
            FenrirGame.Instance.InGame.Scene.ActivateModeHandler(ModeHandler.BuildCaveMedium);
        }

        private void handleBuildCaveLargeClick(object sender, EventArgs e)
        {
            FenrirGame.Instance.InGame.Scene.ActivateModeHandler(ModeHandler.BuildCaveLarge);
        }

        private void handleClearTunnelClick(object sender, EventArgs e)
        {
            FenrirGame.Instance.InGame.Scene.ActivateModeHandler(ModeHandler.ClearTunnel);
        }

        private void handleMenuClick(object sender, EventArgs e)
        {
            if (FenrirGame.Instance.Properties.CurrentGameState == GameState.InGame)
                FenrirGame.Instance.Properties.RequestNewGameState(GameState.Paused);
        }
    }
}
