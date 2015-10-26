using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fenrir.Src.Helper.UI.Alignment;

namespace Fenrir.Src.Helper.UI
{
    /// <summary>
    /// An button disguised as an image
    /// </summary>
    class ImageButton : Button, Element
    {
        /// <summary>
        /// Normal image
        /// </summary>
        Image normal;

        /// <summary>
        /// Hover/active image
        /// </summary>
        Image active;

        public Microsoft.Xna.Framework.Vector2 Position
        {
            get { return this.normal.Position; }
            set { 
                this.normal.Position = value;
                this.ResetPosition();
            }
        }

        /// <summary>
        /// An image button
        /// </summary>
        /// <param name="normal">the image for the normal state</param>
        /// <param name="active">the image for the active state</param>
        /// <param name="verticalAlignment">vertical alignment of the button</param>
        /// <param name="horizontalAlignment">horizontal alignment of the button</param>
        /// <param name="position">absolute position on the screen</param>
        /// <param name="toggle">if button should be toggle able or not</param>
        public ImageButton(String normal, String active, Horizontal horizontalAlignment, Vertical verticalAlignment, Boolean toggle = false, Microsoft.Xna.Framework.Vector2 position = new Microsoft.Xna.Framework.Vector2())
            : base(toggle)
        {
            this.normal = new Image(normal, horizontalAlignment, verticalAlignment, position);
            this.active = new Image(active, horizontalAlignment, verticalAlignment, position);

            this.ResetPosition();
            FenrirGame.Instance.Properties.onResolutionChanged += new EventHandler(ResetPosition);
        }

        /// <summary>
        /// Updates the bounding box and position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetPosition(object sender = null, EventArgs e = null)
        {
            this.UpdateBoundingbox(
                this.normal.ImageSpace
            );
        }

        /// <summary>
        /// Draws the button
        /// </summary>
        public override void Draw()
        {
            this.normal.Color = base.color;

            if (base.IsActive)
                this.active.Draw();
            else
                this.normal.Draw();
        }
    }
}
