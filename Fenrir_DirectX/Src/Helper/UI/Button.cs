using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Fenrir.Src.Helper.UI
{
    /// <summary>
    /// basic Button
    /// </summary>
    abstract class Button
    {
        /// <summary>
        /// the bounding box of the button
        /// </summary>
        protected Rectangle boundingbox;

        /// <summary>
        /// is toggleable?
        /// </summary>
        private Boolean toggle;

        private Boolean isActive;
        /// <summary>
        /// wether the button is toggled on or not
        /// </summary>
        public Boolean IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        /// <summary>
        /// the color of the button
        /// </summary>
        protected Color color;

        /// <summary>
        /// handler for clicks
        /// </summary>
        public event EventHandler onClick;

        /// <summary>
        /// default color for a button
        /// </summary>
        private Color defaultColor = new Color(140, 140, 140);

        /// <summary>
        /// default color for a button if hovered
        /// </summary>
        private Color defaultHover = new Color(0, 133, 188);

        /// <summary>
        /// a basic button.
        /// </summary>
        /// <param name="toggle">toggleable?</param>
        /// <param name="boundingbox"></param>
        public Button(Boolean toggle, Rectangle boundingbox = new Rectangle())
        {
            this.boundingbox = boundingbox;
            this.toggle = toggle;
            this.isActive = false;
            this.color = Color.White;
        }

        /// <summary>
        /// updates the boundingbox of the button
        /// </summary>
        /// <param name="boundingbox">the new bounding box</param>
        protected void UpdateBoundingbox(Rectangle boundingbox)
        {
            this.boundingbox = boundingbox;
        }

        /// <summary>
        /// basic button update thing
        /// </summary>
        /// <param name="active">for toggle buttons wether to stay active or not</param>
        public void Update(Boolean active = false){

            if (this.toggle)
                this.isActive = active;

            if (this.onClick != null && this.onClick.GetInvocationList().Length > 0)
            {
                if (this.boundingbox.Intersects(FenrirGame.Instance.Properties.Input.BoundingBox))
                {
                    // toogle buttons get light green as hover
                    if(this.toggle)
                        this.color = Microsoft.Xna.Framework.Color.LightBlue;
                    else
                        this.color = defaultHover;

                    if (FenrirGame.Instance.Properties.Input.LeftClick)
                    {
                        this.OnClick(new EventArgs());
                        FenrirGame.Instance.Properties.Input.IntersectLeftClick();
                    }
                }
                else
                {
                    if (this.isActive)
                        this.color = defaultHover;
                    else
                        this.color = defaultColor;
                }
            }
        }

        /// <summary>
        /// on click handle thing
        /// </summary>
        /// <param name="e">the click event</param>
        protected virtual void OnClick(EventArgs e)
        {
            if (this.toggle && !this.isActive)
                this.isActive = true;
            else if (this.toggle && this.isActive)
                this.isActive = false;

            EventHandler handler = onClick;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// basic drow method .. doing nothing - implement it! ;)
        /// </summary>
        public virtual void Draw() { }

        /// <summary>
        /// updates the button
        /// </summary>
        public virtual void Update()
        {
            this.Update(false);
        }
    }
}
