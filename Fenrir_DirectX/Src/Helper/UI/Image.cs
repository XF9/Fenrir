using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Fenrir.Src.Helper.UI.Alignment;

namespace Fenrir.Src.Helper.UI
{
    /// <summary>
    /// An Image to render
    /// </summary>
    class Image : Element
    {
        /// <summary>
        /// Name of the image texture
        /// </summary>
        private String textureName;

        public String TextureName
        {
            get { return textureName; }
            private set { textureName = value; }
        }

        private Rectangle imagespace;
        /// <summary>
        /// space the image takes
        /// </summary>
        public Rectangle ImageSpace
        {
            get { return this.imagespace; }
            private set { this.imagespace = value; }
        }

        private Vector2 renderPosition;
        /// <summary>
        /// position of the image to be rendered
        /// </summary>
        public Vector2 RenderPosition
        {
            get { return renderPosition; }
            private set { renderPosition = value; }
        }

        private Vector2 position;
        /// <summary>
        /// the position of the image
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { 
                position = value;
                this.ResetPosition();
            }
        }

        /// <summary>
        /// Size of the image
        /// </summary>
        private Vector2 size;

        public Vector2 Size
        {
            get { return size; }
            set { 
                size = value;
                this.ResetPosition();
            }
        }

        /// <summary>
        /// vertical alignment
        /// </summary>
        private Vertical verticalAlignment;

        /// <summary>
        /// horizontal alignment
        /// </summary>
        private Horizontal horizontalAlignment;

        private Color color;
        /// <summary>
        /// tint of the image
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// wether the image should be tiled or streched
        /// </summary>
        private Boolean isTile = false;

        public Boolean IsTile
        {
            get { return isTile; }
            set { isTile = value; }
        }

        /// <summary>
        /// An image to be rendered
        /// </summary>
        /// <param name="imageName">the name of the texture</param>
        /// <param name="verticalAlignment">vertical alignment of the image</param>
        /// <param name="horizontalAlignment">horizontal alignment of the image</param>
        /// <param name="position">absolute position on the screen</param>
        public Image(String imageName, Horizontal horizontalAlignment, Vertical verticalAlignment, Vector2 position = new Vector2(), Vector2? size = null)
        {
            this.verticalAlignment = verticalAlignment;
            this.horizontalAlignment = horizontalAlignment;
            this.textureName = imageName;

            if (size.HasValue)
                this.size = size.Value;
            else
                this.size = size ?? new Microsoft.Xna.Framework.Vector2(
                    FenrirGame.Instance.Properties.ContentManager.GetTexture(imageName).Width,
                    FenrirGame.Instance.Properties.ContentManager.GetTexture(imageName).Height);

            this.position = position;
            this.renderPosition = new Microsoft.Xna.Framework.Vector2();

            this.color = Color.White;

            this.ResetPosition();
        }

        /// <summary>
        /// Resets the position of the image
        /// called when the resolution changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetPosition()
        {
            switch (this.verticalAlignment)
            {
                case Vertical.Top:
                    this.renderPosition.Y = this.position.Y;
                    break;
                case Vertical.Middle:
                    this.renderPosition.Y = this.position.Y - this.size.Y / 2;
                    break;
                case Vertical.Bottom:
                    this.renderPosition.Y = this.position.Y - this.size.Y;
                    break;
            }

            switch (this.horizontalAlignment)
            {
                case Horizontal.Left:
                    this.renderPosition.X = this.position.X;
                    break;
                case Horizontal.Center:
                    this.renderPosition.X = this.position.X - this.size.X / 2;
                    break;
                case Horizontal.Right:
                    this.renderPosition.X = this.position.X - this.size.X;
                    break;
            }

            this.ImageSpace = new Rectangle(
                    (int)this.renderPosition.X,
                    (int)this.renderPosition.Y,
                    (int)this.size.X,
                    (int)this.size.Y);
        }

        /// <summary>
        /// Draws the image
        /// </summary>
        public void Draw()
        {
            FenrirGame.Instance.Renderer.Draw(this);
        }

        /// <summary>
        /// Updates the image .. why would you do that?
        /// </summary>
        public void Update() { }
    }
}
