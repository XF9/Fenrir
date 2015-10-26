using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Fenrir.Src.Helper.UI.Alignment;

namespace Fenrir.Src.Helper.UI
{
    /// <summary>
    /// A label .. you can put it on things
    /// </summary>
    class Label : Element
    {
        private String text;
        /// <summary>
        /// The text to be displayed
        /// </summary>
        public String Text
        {
            get { return text; }
            set { 
                text = value;
                this.ResetPosition();
            }
        }

        /// <summary>
        /// The font to be used
        /// </summary>
        private SpriteFont font;

        public SpriteFont Font
        {
            get { return font; }
            private set { font = value; }
        }

        /// <summary>
        /// vertical alignment
        /// </summary>
        private Vertical verticalAlignment;

        /// <summary>
        /// horizontal alignment
        /// </summary>
        private Horizontal horizontalAlignment;

        private Rectangle labelSpace;
        /// <summary>
        /// space the image takes
        /// </summary>
        public Rectangle LabelSpace
        {
            get { return this.labelSpace; }
            private set { this.labelSpace = value; }
        }

        /// <summary>
        /// Offset from the given origins
        /// </summary>
        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { 
                position = value;
                this.ResetPosition();
            }
        }

        /// <summary>
        /// The position of the label on the screen
        /// </summary>
        private Vector2 renderPosition = new Vector2(0, 0);

        public Vector2 RenderPosition
        {
            get { return renderPosition; }
            set { renderPosition = value; }
        }

        /// <summary>
        /// The default color
        /// </summary>
        private Color color = new Color(140, 140, 140);

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// Creates a label
        /// </summary>
        /// <param name="text">the text</param>
        /// <param name="font">the font</param>
        /// <param name="verticalAlignment">vertical alignment of the label</param>
        /// <param name="horizontalAlignment">horizontal alignment of the label</param>
        /// <param name="position">the absolute position on the screen</param>
        public Label(String text, String font, Horizontal horizontalAlignment, Vertical verticalAlignment, Vector2 position = new Vector2())
        {
            this.text = text;
            this.font = FenrirGame.Instance.Properties.ContentManager.GetFont(font);
            this.position = position;
            this.horizontalAlignment = horizontalAlignment;
            this.verticalAlignment = verticalAlignment;
            this.ResetPosition();
        }

        /// <summary>
        /// Reloads the text and position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetPosition()
        {
            text = FenrirGame.Instance.Properties.ContentManager.getLocalization(this.text);

            switch (this.verticalAlignment)
            {
                case Vertical.Top:
                    this.renderPosition.Y = this.position.Y;
                    break;
                case Vertical.Middle:
                    this.renderPosition.Y = this.position.Y - this.font.MeasureString(text).Y / 2;
                    break;
                case Vertical.Bottom:
                    this.renderPosition.Y = this.position.Y - this.font.MeasureString(text).Y;
                    break;
            }

            switch (this.horizontalAlignment)
            {
                case Horizontal.Left:
                    this.renderPosition.X = this.position.X;
                    break;
                case Horizontal.Center:
                    this.renderPosition.X = this.position.X - this.font.MeasureString(text).X / 2;
                    break;
                case Horizontal.Right:
                    this.renderPosition.X = this.position.X - this.font.MeasureString(text).X;
                    break;
            }

            this.LabelSpace = new Rectangle(
                    (int)this.renderPosition.X,
                    (int)this.renderPosition.Y + 15,
                    (int)this.font.MeasureString(text).X,
                    (int)this.font.MeasureString(text).Y - 25);
        }

        /// <summary>
        /// Draws the label
        /// </summary>
        public void Draw()
        {
            FenrirGame.Instance.Renderer.Draw(this);
        }

        /// <summary>
        /// Updates the label .. nothing to do here
        /// </summary>
        public void Update() { }
    }
}
