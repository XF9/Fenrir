using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fenrir.Src.Helper.UI.Alignment;

namespace Fenrir.Src.Helper
{
    /// <summary>
    /// The cursor
    /// </summary>
    class Cursor
    {
        /// <summary>
        /// Different types of cursors available
        /// </summary>
        public enum CursorType
        {
            Regular,
            Camera
        }

        private Fenrir.Src.Helper.UI.Image currentCursor;
        private Fenrir.Src.Helper.UI.Image defaultCursor;
        private Fenrir.Src.Helper.UI.Image cameraCursor;

        /// <summary>
        /// Create a cursor
        /// </summary>
        public Cursor()
        {
            this.defaultCursor = new UI.Image(DataIdentifier.textureCursorRegular, Horizontal.Left, Vertical.Top);
            this.cameraCursor = new UI.Image(DataIdentifier.textureCursorCamera, Horizontal.Center, Vertical.Top);

            this.currentCursor = defaultCursor;
        }

        public void changeCursorType(CursorType newType){
            switch (newType)
            {
                case CursorType.Camera:
                    this.currentCursor = this.defaultCursor;
                    break;
                case CursorType.Regular:
                    this.currentCursor = this.cameraCursor;
                    break;
            }
        }

        /// <summary>
        /// Update the cursor
        /// </summary>
        public void Update() 
        {
            this.defaultCursor.Position = FenrirGame.Instance.Properties.Input.CurrentMousePosition;
        }

        /// <summary>
        /// Draw the cursor
        /// </summary>
        public void Draw()
        {
            if(FenrirGame.Instance.Properties.CurrentGameState != GameState.LoadMenu && FenrirGame.Instance.Properties.CurrentGameState != GameState.LoadGame)
                FenrirGame.Instance.Renderer.Draw(this.defaultCursor);
        }
    }
}
