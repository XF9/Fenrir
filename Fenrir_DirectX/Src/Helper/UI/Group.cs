using Fenrir.Src.Helper.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Fenrir.Src.Helper.UI.Alignment;

namespace Fenrir.Src.Helper.UI
{
    /// <summary>
    /// A group of UI elements groupt together to be displayed
    /// </summary>
    class Group
    {
        protected struct Entry
        {
            public Vector2 position;
            public Element element;

            public Entry(Vector2 position, Element element)
            {
                this.position = position;
                this.element = element;
            }
        }
        /// <summary>
        /// List of elemts to be grouped
        /// </summary>
        protected List<Entry> elements;

        /// <summary>
        /// vertical alignment
        /// </summary>
        private Vertical verticalAlignment;

        /// <summary>
        /// horizontal alignment
        /// </summary>
        private Horizontal horizontalAlignment;

        /// <summary>
        /// Create an HUD element
        /// </summary>
        public Group(Horizontal horizontalAlignment, Vertical verticalAlignment)
        {
            this.elements = new List<Entry>();
            this.horizontalAlignment = horizontalAlignment;
            this.verticalAlignment = verticalAlignment;
        }

        /// <summary>
        /// Adds an UI element to the group
        /// </summary>
        /// <param name="element"></param>
        public void AddUiElement(Vector2 position, Element element)
        {
            this.elements.Add(new Entry(position, element));
        }

        public void ResetPosition(Vector2 newPos)
        {
            Vector2 origin = new Vector2();

            switch (this.verticalAlignment)
            {
                case Vertical.Top:
                    origin.Y = 0;
                    break;
                case Vertical.Middle:
                    origin.Y = FenrirGame.Instance.Properties.ScreenHeight / 2;
                    break;
                case Vertical.Bottom:
                    origin.Y = FenrirGame.Instance.Properties.ScreenHeight;
                    break;
            }

            switch (this.horizontalAlignment)
            {
                case Horizontal.Left:
                    origin.X = 0;
                    break;
                case Horizontal.Center:
                    origin.X = FenrirGame.Instance.Properties.ScreenWidth / 2;
                    break;
                case Horizontal.Right:
                    origin.X = FenrirGame.Instance.Properties.ScreenWidth;
                    break;
            }

            foreach (Entry entry in this.elements)
            {
                entry.element.Position = entry.position + origin + newPos;
            }
        }

        /// <summary>
        /// Updates the HUD element
        /// </summary>
        public void Update()
        {
            foreach (Entry entry in this.elements)
                entry.element.Update();
        }

        /// <summary>
        /// Draws the HUD element
        /// </summary>
        public void Draw()
        {
            foreach (Entry entry in this.elements)
                entry.element.Draw();
        }
    }
}
