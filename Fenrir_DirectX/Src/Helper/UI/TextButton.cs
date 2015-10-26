using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Fenrir.Src.Helper.UI.Alignment;

namespace Fenrir.Src.Helper.UI
{
    /// <summary>
    /// A button with a textfield
    /// </summary>
    class TextButton : Button, Element
    {

        private Label label;
        /// <summary>
        /// the label for the text to be displayed
        /// </summary>
        internal Label Label
        {
            get { return label; }
            private set { label = value; }
        }

        public Microsoft.Xna.Framework.Vector2 Position
        {
            get { return this.label.Position; }
            set { 
                this.label.Position = value;
                this.ResetPosition();
            }
        }

        private String font;
        private Horizontal horizontalAlignment;
        private Vertical verticalAlignment;
        private Boolean toggle;

        /// <summary>
        /// Creates a text button
        /// </summary>
        /// <param name="text">the text</param>
        /// <param name="font">the font</param>
        /// <param name="verticalAlignment">vertical alignment of the image</param>
        /// <param name="horizontalAlignment">horizontal alignment of the image</param>
        /// <param name="position">absolute position on the screen</param>
        /// <param name="toggle">toggleable?</param>
        public TextButton(String text, String font, Horizontal horizontalAlignment, Vertical verticalAlignment, Boolean toggle = false, Vector2 position = new Vector2())
            : base(toggle)
        {
            this.label = new Label(text, font, horizontalAlignment, verticalAlignment, position);
            this.font = font;
            this.ResetPosition();
            this.toggle = toggle;

            FenrirGame.Instance.Properties.onResolutionChanged += new EventHandler(ResetPosition);
        }

        /// <summary>
        /// resets the position on resolution change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetPosition(object sender = null, EventArgs e = null)
        {
            this.UpdateBoundingbox(
                this.label.LabelSpace
            );
        }

        /// <summary>
        /// Draws the thing
        /// </summary>
        public override void Draw()
        {
            this.label.Color = base.color;
            this.label.Draw();
        }

        public TextButton Copy()
        {
            return new TextButton(this.Label.Text, this.font, this.horizontalAlignment, this.verticalAlignment, this.toggle, this.Position);
        }
    }
}
