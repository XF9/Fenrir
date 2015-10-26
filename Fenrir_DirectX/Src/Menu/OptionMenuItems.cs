using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fenrir.Src.Helper;
using Fenrir.Src.Helper.UI;
using Fenrir.Src.Helper.UI.Alignment;

namespace Fenrir.Src.Menu
{
    class OptionMenuItems : Group
    {
        /// <summary>
        /// selection list for the language
        /// </summary>
        private SelectionList languageList;

        /// <summary>
        /// selection list for the resolution
        /// </summary>
        private SelectionList resolutionList;

        /// <summary>
        /// selection list to toggle fullscreen
        /// </summary>
        private SelectionList windowModeList;

        /// <summary>
        /// button to accept the changes made
        /// </summary>
        private TextButton applyButton;

        /// <summary>
        /// button to go back
        /// </summary>
        private TextButton backButton;

        public OptionMenuItems()
            : base(Horizontal.Center, Vertical.Top)
        {
            // language
            int defaultLanguage = FenrirGame.Instance.Properties.ContentManager.Languages.IndexOf(FenrirGame.Instance.Config.Language);

            this.languageList = new SelectionList(
                DataIdentifier.labelOptionMenuLanguage,
                DataIdentifier.defaultFont,
                FenrirGame.Instance.Properties.ContentManager.Languages,
                defaultLanguage,
                Horizontal.Center,
                Vertical.Top
                );

            this.languageList.onSelected += new EventHandler<ItemSelectedEventArgs>(HandelLanguageChange);

            // resolution
            List<String> resolutions = new List<String>();
            resolutions.Add("1024 x 768");
            resolutions.Add("1280 x 720");
            resolutions.Add("1920 x 1080");
            resolutions.Add("2560 x 1440");

            String defaultResolutionString = FenrirGame.Instance.Config.ResolutionX + " x " + FenrirGame.Instance.Config.ResolutionY;
            if (!resolutions.Contains(defaultResolutionString))
                resolutions.Add(defaultResolutionString);
            int defaultResolution = resolutions.IndexOf(defaultResolutionString);

            this.resolutionList = new SelectionList(
                DataIdentifier.labelOptionMenuResolution,
                DataIdentifier.defaultFont,
                resolutions,
                defaultResolution,
                Horizontal.Center,
                Vertical.Top
                );

            // fullscree
            List<String> windowModes = new List<String>();
            windowModes.Add(DataIdentifier.labelOptionMenuWindowModeFullscreen);
            windowModes.Add(DataIdentifier.labelOptionMenuWindowModeBorderless);
            windowModes.Add(DataIdentifier.labelOptionMenuWindowModeWindowed);

            int defaultWindowmode = FenrirGame.Instance.Properties.IsFullscreen ? 0 : 1;
            this.windowModeList = new SelectionList(
                DataIdentifier.labelOptionMenuWindowMode,
                DataIdentifier.defaultFont,
                windowModes,
                defaultWindowmode,
                Horizontal.Center,
                Vertical.Top
                );

            this.applyButton = new TextButton(
                DataIdentifier.labelApply, DataIdentifier.defaultFont,
                Horizontal.Center, Vertical.Top);

            this.backButton = new TextButton(
                DataIdentifier.labelBack, DataIdentifier.defaultFont,
                Horizontal.Center, Vertical.Top);

            this.applyButton.onClick += new EventHandler(HandleOptionMenuApplyClick);
            this.backButton.onClick += new EventHandler(HandleOptionMenuBackClick);

            this.AddUiElement(new Microsoft.Xna.Framework.Vector2(0, 0), this.languageList);
            this.AddUiElement(new Microsoft.Xna.Framework.Vector2(0, 60), this.resolutionList);
            this.AddUiElement(new Microsoft.Xna.Framework.Vector2(0, 120), this.windowModeList);
            this.AddUiElement(new Microsoft.Xna.Framework.Vector2(-100, 180), this.applyButton);
            this.AddUiElement(new Microsoft.Xna.Framework.Vector2(100, 180), this.backButton);

            this.ResetPosition(new Microsoft.Xna.Framework.Vector2(0, 430));
        }

        private void HandelLanguageChange(object sender, EventArgs e)
        {
            FenrirGame.Instance.Properties.SelectedLanguage = this.languageList.CurrentValue;
        }


        private void HandleOptionMenuApplyClick(object sender, EventArgs e)
        {
            // save resolution settings
            int split = this.resolutionList.CurrentValue.IndexOf(" x ");
            int width = Convert.ToInt32(this.resolutionList.CurrentValue.Substring(0, split));
            int height = Convert.ToInt32(this.resolutionList.CurrentValue.Substring(split + 3, this.resolutionList.CurrentValue.Length - split - 3));

            FenrirGame.Instance.Properties.changeResolution(width, height);

            FenrirGame.Instance.Config.Language = this.languageList.CurrentValue;
            FenrirGame.Instance.Config.ResolutionX = width;
            FenrirGame.Instance.Config.ResolutionY = height;
            FenrirGame.Instance.Config.WindowMode = this.windowModeList.Index;

            FenrirGame.Instance.Properties.RequestNewGameState(GameState.MainMenu);

            // TODO
            //FenrirGame.Instance.Properties.toggleFullscreen(this.fullscreenButton.CurrentValue == FenrirGame.Instance.Properties.ContentManager.getLocalization("#enabled#"));
        }

        private void HandleOptionMenuBackClick(object sender, EventArgs e)
        {
            this.languageList.Index = FenrirGame.Instance.Properties.ContentManager.Languages.IndexOf(FenrirGame.Instance.Config.Language);
            FenrirGame.Instance.Properties.SelectedLanguage = this.languageList.CurrentValue;
            FenrirGame.Instance.Properties.RequestNewGameState(GameState.MainMenu);

            //this.resolutionList.Index = this.resolutionList.IndexOf(FenrirGame.Instance.Config.ResolutionX + " x " + FenrirGame.Instance.Config.ResolutionY);
            //this.fullscreenButton.Index = this.defaultFullscreen;
        }
    }
}
