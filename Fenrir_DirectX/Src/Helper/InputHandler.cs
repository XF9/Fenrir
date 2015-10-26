using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fenrir.Src.Helper
{
    /// <summary>
    /// Keeps track of all the input made
    /// </summary>
    class InputHandler
    {
        #region input parameter

        /// <summary>
        /// keyboard state from the last frame
        /// </summary>
        private Microsoft.Xna.Framework.Input.KeyboardState lastKeyboardState;

        /// <summary>
        /// keyboard state of the current frame
        /// </summary>
        private Microsoft.Xna.Framework.Input.KeyboardState currentKeyboardState;

        private Microsoft.Xna.Framework.Vector2 currentMousePosition;
        /// <summary>
        /// the current mouse position
        /// </summary>
        public Microsoft.Xna.Framework.Vector2 CurrentMousePosition
        {
            get { return currentMousePosition; }
            private set { currentMousePosition = value; }
        }


        private Microsoft.Xna.Framework.Vector3 currentMousePositionInWorld;
        /// <summary>
        /// the mouse coordinates transformed in inworld coordinates
        /// </summary>
        public Microsoft.Xna.Framework.Vector3 CurrentMousePositionInWorld
        {
            get { return currentMousePositionInWorld; }
            private set { currentMousePositionInWorld = value; }
        }

        private Microsoft.Xna.Framework.Rectangle boundingBox;
        /// <summary>
        /// boundingbox for the cursor
        /// </summary>
        public Microsoft.Xna.Framework.Rectangle BoundingBox
        {
            get { return boundingBox; }
            private set { boundingBox = value; }
        }


        private Microsoft.Xna.Framework.Vector2 mouseMovement;
        /// <summary>
        /// the mouse movement in the last frame
        /// </summary>
        public Microsoft.Xna.Framework.Vector2 MouseMovement
        {
            get { return mouseMovement; }
            private set { mouseMovement = value; }
        }

        /// <summary>
        /// starting point of the left klick to check for dragging
        /// </summary>
        private Microsoft.Xna.Framework.Vector2? dragLeftStart;

        /// <summary>
        /// starting point of the right klick to check for dragging
        /// </summary>
        private Microsoft.Xna.Framework.Vector2? dragRightStart;

        private Boolean leftClick;
        /// <summary>
        /// left nouse button is released
        /// </summary>
        public Boolean LeftClick
        {
            get { return leftClick; }
            private set { leftClick = value; }
        }

        private Boolean leftDrag;
        /// <summary>
        /// left mouse butten is pressed
        /// </summary>
        public Boolean LeftDrag
        {
            get { return leftDrag; }
            private set { leftDrag = value; }
        }

        private Boolean rightDrag;
        /// <summary>
        /// right mouse buttion is pressed
        /// </summary>
        public Boolean RightDrag
        {
            get { return rightDrag; }
            private set { rightDrag = value; }
        }

        private Boolean rightClick;
        /// <summary>
        /// right mouse button is released
        /// </summary>
        public Boolean RightClick
        {
            get { return rightClick; }
            private set { rightClick = value; }
        }

        private Boolean releaseLeft;
        /// <summary>
        /// true if the left mouse button is released
        /// </summary>
        public Boolean ReleaseLeft
        {
            get { return releaseLeft; }
            set { releaseLeft = value; }
        }

        private Boolean releaseRight;
        /// <summary>
        /// true if the right mouse button got realeased
        /// </summary>
        public Boolean ReleaseRight
        {
            get { return releaseRight; }
            set { releaseRight = value; }
        }

        private int scrollValueOld;
        private int scrollValue;
        /// <summary>
        /// value which is scrolled during the last frame
        /// </summary>
        public int ScrollValue
        {
            get { return scrollValue; }
            private set { scrollValue = value; }
        }

        private Boolean moveCamRight;
        /// <summary>
        /// move the marea to the right
        /// </summary>
        public Boolean MoveCamRight
        {
            get { return moveCamRight; }
            private set { moveCamRight = value; }
        }

        private Boolean moveCamLeft;
        /// <summary>
        /// move the camera to the right
        /// </summary>
        public Boolean MoveCamLeft
        {
            get { return moveCamLeft; }
            private set { moveCamLeft = value; }
        }

        private Boolean moveCamUp;
        /// <summary>
        /// move the cam up
        /// </summary>
        public Boolean MoveCamUp
        {
            get { return moveCamUp; }
            private set { moveCamUp = value; }
        }

        private Boolean moveCamDown;
        /// <summary>
        /// move the cam down
        /// </summary>
        public Boolean MoveCamDown
        {
            get { return moveCamDown; }
            private set { moveCamDown = value; }
        }

        private Boolean resetCamRot;
        /// <summary>
        /// reset the camera
        /// </summary>
        public Boolean ResetCamRot
        {
            get { return resetCamRot; }
            private set { resetCamRot = value; }
        }

        private Boolean endCurrentAction;
        /// <summary>
        /// signal to switch the game and menu screen
        /// </summary>
        public Boolean EndCurrentAction
        {
            get { return endCurrentAction; }
            set { endCurrentAction = value; }
        }

        private Microsoft.Xna.Framework.Ray mouseRay;
        /// <summary>
        /// the ray, defined through the cameraposition and mouseposition
        /// </summary>
        public Microsoft.Xna.Framework.Ray MouseRay
        {
            get { return mouseRay; }
            private set { mouseRay = value; }
        }
        #endregion

        /// <summary>
        /// Create an input handler
        /// </summary>
        public InputHandler()
        {
            this.currentMousePosition = new Microsoft.Xna.Framework.Vector2(0, 0);
            this.mouseMovement = new Microsoft.Xna.Framework.Vector2(0, 0);
            this.boundingBox = new Microsoft.Xna.Framework.Rectangle(0, 0, 10, 10);

            this.leftClick = false;
            this.leftDrag = false;
            this.rightClick = false;
            this.rightDrag = false;
            this.releaseLeft = false;
            this.releaseRight = false;

            Microsoft.Xna.Framework.Input.Mouse.SetPosition(0, 0);
            this.currentMousePosition = new Microsoft.Xna.Framework.Vector2(0, 0);
        }

        /// <summary>
        /// check for new updates
        /// </summary>
        public void Update()
        {
            // get the mousestate
            Microsoft.Xna.Framework.Input.MouseState ms = Microsoft.Xna.Framework.Input.Mouse.GetState();
            this.lastKeyboardState = this.currentKeyboardState;
            this.currentKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            // get the movement and position

            this.mouseMovement = new Microsoft.Xna.Framework.Vector2((FenrirGame.Instance.Properties.ScreenWidth / 2 - ms.X), (FenrirGame.Instance.Properties.ScreenHeight / 2 - ms.Y));
            this.currentMousePosition -= mouseMovement * 1.05f;
            Microsoft.Xna.Framework.Input.Mouse.SetPosition(FenrirGame.Instance.Properties.ScreenWidth / 2, FenrirGame.Instance.Properties.ScreenHeight / 2);

            if (this.currentMousePosition.X < 0)
                this.currentMousePosition.X = 0;
            else if (this.currentMousePosition.X > FenrirGame.Instance.Properties.ScreenWidth)
                this.currentMousePosition.X = FenrirGame.Instance.Properties.ScreenWidth;

            if (this.currentMousePosition.Y < 0)
                this.currentMousePosition.Y = 0;
            else if (this.currentMousePosition.Y > FenrirGame.Instance.Properties.ScreenHeight)
                this.currentMousePosition.Y = FenrirGame.Instance.Properties.ScreenHeight;

            // get the scrollwheel
            this.scrollValue = scrollValueOld - ms.ScrollWheelValue;
            this.scrollValueOld = ms.ScrollWheelValue;

            // calculate boundingbox
            this.boundingBox.X = (int)FenrirGame.Instance.Properties.Input.CurrentMousePosition.X;
            this.boundingBox.Y = (int)FenrirGame.Instance.Properties.Input.CurrentMousePosition.Y;

            // buttonstates
            Boolean leftPressed = ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            Boolean rightPressed = ms.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;

            // reset click events
            this.releaseLeft = false;
            this.releaseRight = false;
            this.leftClick = false;
            this.rightClick = false;

            // check if dragging or clicked
            if (leftPressed)
            {
                if (!this.dragLeftStart.HasValue)
                    this.dragLeftStart = this.currentMousePosition;
                else if (!this.leftDrag && (Math.Abs(this.currentMousePosition.X - this.dragLeftStart.Value.X) > 5) || (Math.Abs(this.currentMousePosition.Y - this.dragLeftStart.Value.Y) > 5))
                    this.leftDrag = true;
            }
            else
            {
                if (this.dragLeftStart.HasValue)
                {
                    this.dragLeftStart = null;
                    this.leftDrag = false;
                    this.releaseLeft = true;

                    if (!this.leftDrag)
                        this.leftClick = true;
                }
            }

            if (rightPressed)
            {
                if (!this.dragRightStart.HasValue)
                    this.dragRightStart = this.currentMousePosition;
                else if (!this.rightDrag && (Math.Abs(this.currentMousePosition.X - this.dragRightStart.Value.X) > 5) || (Math.Abs(this.currentMousePosition.Y - this.dragRightStart.Value.Y) > 5))
                    this.rightDrag = true;
            }
            else
            {
                if (this.dragRightStart.HasValue)
                {
                    this.dragRightStart = null;
                    this.rightDrag = false;
                    this.releaseRight = true;

                    if (!this.rightDrag)
                        this.rightClick = true;
                }
            }
            
            // keaboard stuff
            this.moveCamLeft = currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A);
            this.moveCamRight = currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D);
            this.moveCamUp = currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W);
            this.moveCamDown = currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S);
            this.resetCamRot = currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D0);

            if (currentKeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Escape) && !lastKeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Escape))
                this.endCurrentAction = true;
            else
                this.endCurrentAction = false;

            if (FenrirGame.Instance.Properties.CurrentGameState == GameState.InGame)
            {
                Microsoft.Xna.Framework.Vector3 nearsource = new Microsoft.Xna.Framework.Vector3(this.currentMousePosition.X, this.currentMousePosition.Y, 0f);
                Microsoft.Xna.Framework.Vector3 farsource = new Microsoft.Xna.Framework.Vector3(this.currentMousePosition.X, this.currentMousePosition.Y, 1f);

                Microsoft.Xna.Framework.Vector3 nearPoint = FenrirGame.Instance.Properties.GraphicDeviceManager.GraphicsDevice.Viewport.Unproject(
                    nearsource,
                    FenrirGame.Instance.InGame.Camera.Projection,
                    FenrirGame.Instance.InGame.Camera.View,
                    FenrirGame.Instance.InGame.Camera.World);
                Microsoft.Xna.Framework.Vector3 farPoint = FenrirGame.Instance.Properties.GraphicDeviceManager.GraphicsDevice.Viewport.Unproject(
                    farsource,
                    FenrirGame.Instance.InGame.Camera.Projection,
                    FenrirGame.Instance.InGame.Camera.View,
                    FenrirGame.Instance.InGame.Camera.World);

                Microsoft.Xna.Framework.Vector3 direction = farPoint - nearPoint;
                direction.Normalize();
                this.mouseRay = new Microsoft.Xna.Framework.Ray(nearPoint, direction);

                this.currentMousePositionInWorld = FenrirGame.Instance.Properties.GraphicDeviceManager.GraphicsDevice.Viewport.Unproject(
                    nearsource,
                    FenrirGame.Instance.InGame.Camera.Projection,
                    FenrirGame.Instance.InGame.Camera.View,
                    Microsoft.Xna.Framework.Matrix.Identity
                    );
            }
        }

        /// <summary>
        /// stops the left klick from getting through
        /// </summary>
        public void IntersectLeftClick()
        {
            this.leftClick = false;
            this.leftDrag = false;
        }

        /// <summary>
        /// stops the right klick from getting through
        /// </summary>
        public void IntersectRightKlick()
        {
            this.rightClick = false;
            this.rightDrag = false;
        }
    }
}
