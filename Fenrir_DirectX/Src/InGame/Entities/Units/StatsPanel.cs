using Fenrir.Src.Helper;
using Fenrir.Src.Helper.UI;
using Fenrir.Src.Helper.UI.Alignment;
using Fenrir.Src.InGame.Components.HUD;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fenrir.Src.InGame.Entities.Units
{
    /// <summary>
    /// statisc panel for units
    /// to be used in the hud infopanel
    /// </summary>
    class StatsPanel : InfoPanel
    {
        private Stats data;

        private Label nameLabel;
        private Label nameValue;

        private Label ageLabel;
        private Label ageValue;

        private Label miningLabel;
        private Label miningValue;

        public StatsPanel(Stats data)
        {
            this.data = data;

            this.nameLabel = new Label("#unit_name#", DataIdentifier.defaultFontSmall, Horizontal.Left, Vertical.Top);
            this.nameValue = new Label("", DataIdentifier.defaultFontSmall, Horizontal.Left, Vertical.Top);

            this.ageLabel = new Label("#unit_age#", DataIdentifier.defaultFontSmall, Horizontal.Left, Vertical.Top);
            this.ageValue = new Label("", DataIdentifier.defaultFontSmall, Horizontal.Left, Vertical.Top);

            this.miningLabel = new Label("#unit_mining#", DataIdentifier.defaultFontSmall, Horizontal.Left, Vertical.Top);
            this.miningValue = new Label("", DataIdentifier.defaultFontSmall, Horizontal.Left, Vertical.Top);

            this.AddUiElement(new Vector2(-200, 100), nameLabel);
            this.AddUiElement(new Vector2(-100, 100), nameValue);

            this.AddUiElement(new Vector2(-200, 130), ageLabel);
            this.AddUiElement(new Vector2(-100, 130), ageValue);

            this.AddUiElement(new Vector2(-200, 160), miningLabel);
            this.AddUiElement(new Vector2(-100, 160), miningValue);

            this.ResetPosition(new Vector2());
        }

        public void Update()
        {
            base.Update();

            this.nameValue.Text = this.data.Name;
            this.ageValue.Text = this.data.Age.ToString();
            this.miningValue.Text = this.data.Mining.ToString();
        }
    }
}
