using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Fenrir.Src.Helper;

namespace Fenrir.Src.InGame.Components
{
    /// <summary>
    /// The camera
    /// </summary>
    class Camera
    {
        private Matrix world;
        /// <summary>
        /// the world matrix
        /// </summary>
        public Matrix World
        {
            get { return world; }
            set { world = value; }
        }

        private Matrix view;
        /// <summary>
        /// the view matrix
        /// </summary>
        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }

        private Matrix projection;
        /// <summary>
        /// the projection matrix
        /// </summary>
        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }

        private float cameraDistance;
        /// <summary>
        /// the camera distance to the world
        /// </summary>
        public float CameraDistance
        {
            get { return cameraDistance; }
            set { cameraDistance = value; }
        }

        /// <summary>
        /// Blocks the ability to move the camera
        /// </summary>
        public Boolean blockCameraMovement = false;

        /// <summary>
        /// where to look at
        /// </summary>
        private Vector2 lookAt;

        /// <summary>
        /// the aspect ratio
        /// </summary>
        private float aspectRatio;

        /// <summary>
        /// the field of view
        /// </summary>
        private float fov;

        /// <summary>
        /// default rotation radius around the x axis
        /// </summary>
        private float defaultRotX = 90;

        /// <summary>
        /// default rotation radius around the y axis
        /// </summary>
        private float defaultRotY = 270;

        /// <summary>
        /// minimum rotation value for x axis
        /// </summary>
        private float minRotX;

        /// <summary>
        /// minimum rotation value for y axis
        /// </summary>
        private float minRotY;

        /// <summary>
        /// maximum rotation value for x axis
        /// </summary>
        private float maxRotX;

        /// <summary>
        /// maximum rotation value for y axis
        /// </summary>
        private float maxRotY;

        /// <summary>
        /// current rotation value for x axis
        /// </summary>
        private float currentRotX;

        /// <summary>
        /// current rotation value for y axis
        /// </summary>
        private float currentRotY;

        /// <summary>
        /// maximum zoom value
        /// </summary>
        private float maxZoom = 100;

        /// <summary>
        /// minimum zuoom value
        /// </summary>
        private float minZoom = 10;

        /// <summary>
        /// Create the camera
        /// </summary>
        public Camera()
        {
            // use float to prevent integer div :/
            this.aspectRatio = ((float)FenrirGame.Instance.Properties.ScreenWidth) / ((float)FenrirGame.Instance.Properties.ScreenHeight);
            this.fov = MathHelper.ToRadians(45);

            this.currentRotY = this.defaultRotY;
            this.currentRotX = this.defaultRotX;

            this.minRotX = this.defaultRotX - 35;
            this.maxRotX = this.defaultRotX + 25;

            this.minRotY = this.defaultRotY - 35;
            this.maxRotY = this.defaultRotY + 35;

            this.cameraDistance = 40.0f;
            this.lookAt = new Vector2(-3, 8);

            this.world = Matrix.CreateScale(new Vector3(1, 1, 1));
            this.view = Matrix.CreateLookAt(new Vector3(0.1f, 0, 0), new Vector3(0, 0, 0), Vector3.UnitY);
            this.projection = Matrix.CreatePerspectiveFieldOfView(this.fov, this.aspectRatio, 0.1f, 400f);
        }

        /// <summary>
        /// Update the camera
        /// </summary>
        public void Update()
        {
            if (FenrirGame.Instance.Properties.CurrentGameState == GameState.InGame)
            {

                this.cameraDistance += FenrirGame.Instance.Properties.Input.ScrollValue / 10;
                if (this.cameraDistance < this.minZoom)
                    this.cameraDistance = this.minZoom;
                else if (this.cameraDistance > this.maxZoom)
                    this.cameraDistance = this.maxZoom;

                if (FenrirGame.Instance.Properties.Input.ReleaseLeft || FenrirGame.Instance.Properties.Input.ReleaseRight)
                    FenrirGame.Instance.Cursor.changeCursorType(Cursor.CursorType.Regular);

                if (FenrirGame.Instance.Properties.Input.RightDrag)
                {
                    FenrirGame.Instance.Cursor.changeCursorType(Cursor.CursorType.Camera);

                    this.currentRotY -= FenrirGame.Instance.Properties.Input.MouseMovement.X * 0.5f;   // moving left and right rotates around Y axis
                    this.currentRotX -= FenrirGame.Instance.Properties.Input.MouseMovement.Y * 0.5f;

                    if (this.currentRotX < this.minRotX)
                        this.currentRotX = this.minRotX;
                    else if (currentRotX > this.maxRotX)
                        this.currentRotX = this.maxRotX;

                    if (this.currentRotY < this.minRotY)
                        this.currentRotY = this.minRotY;
                    else if (currentRotY > this.maxRotY)
                        this.currentRotY = this.maxRotY;
                }

                if (FenrirGame.Instance.Properties.Input.LeftDrag && !this.blockCameraMovement)
                {
                    FenrirGame.Instance.Cursor.changeCursorType(Cursor.CursorType.Camera);

                    this.lookAt.X += FenrirGame.Instance.Properties.Input.MouseMovement.X * 0.1f;
                    this.lookAt.Y -= FenrirGame.Instance.Properties.Input.MouseMovement.Y * 0.1f;
                }
                else
                {
                    if (FenrirGame.Instance.Properties.Input.CurrentMousePosition.X < 20)
                        this.lookAt.X -= 5 / (FenrirGame.Instance.Properties.Input.CurrentMousePosition.X + 5);
                    else if (FenrirGame.Instance.Properties.Input.CurrentMousePosition.X > FenrirGame.Instance.Properties.ScreenWidth - 20)
                        this.lookAt.X += 5 / (FenrirGame.Instance.Properties.ScreenWidth - FenrirGame.Instance.Properties.Input.CurrentMousePosition.X + 5);

                    if (FenrirGame.Instance.Properties.Input.CurrentMousePosition.Y < 20)
                        this.lookAt.Y += 5 / (FenrirGame.Instance.Properties.Input.CurrentMousePosition.Y + 5);
                    else if (FenrirGame.Instance.Properties.Input.CurrentMousePosition.Y > FenrirGame.Instance.Properties.ScreenHeight - 20)
                        this.lookAt.Y -= 5 / (FenrirGame.Instance.Properties.ScreenHeight - FenrirGame.Instance.Properties.Input.CurrentMousePosition.Y + 5);
                }

                if (FenrirGame.Instance.Properties.Input.MoveCamLeft)
                    this.lookAt.X -= 1;
                if (FenrirGame.Instance.Properties.Input.MoveCamRight)
                    this.lookAt.X += 1;
                if (FenrirGame.Instance.Properties.Input.MoveCamUp)
                    this.lookAt.Y += 1;
                if (FenrirGame.Instance.Properties.Input.MoveCamDown)
                    this.lookAt.Y -= 1;
                if (FenrirGame.Instance.Properties.Input.ResetCamRot)
                {
                    this.currentRotX = this.defaultRotX;
                    this.currentRotY = this.defaultRotY;
                }

                float latitude = Microsoft.Xna.Framework.MathHelper.ToRadians(this.currentRotX);
                float longitude = Microsoft.Xna.Framework.MathHelper.ToRadians(this.currentRotY);

                float camX = this.cameraDistance * (float)Math.Sin(latitude) * (float)Math.Cos(longitude);
                float camY = this.cameraDistance * (float)Math.Cos(latitude);
                float camZ = -this.cameraDistance * (float)Math.Sin(latitude) * (float)Math.Sin(longitude);

                Vector3 look = new Vector3(this.lookAt.X, this.lookAt.Y, 0);
                Vector3 pos = new Vector3(camX, camY, camZ);

                this.view = Matrix.CreateLookAt(pos + look, look, Vector3.UnitY);

                this.blockCameraMovement = false;
            }
        }
    }
}
