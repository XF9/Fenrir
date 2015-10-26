using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Fenrir.Src.Helper;

namespace Fenrir.Src.InGame.Components
{
    /// <summary>
    /// things tha can be marked
    /// </summary>
    enum MarkerType
    {
        Tunnel,
        Cave,
        Info,
        Error
    }

    /// <summary>
    /// A Marker for marking markings
    /// </summary>
    class Marker
    {
        private MarkerType type;
        /// <summary>
        /// Markertype
        /// </summary>
        internal MarkerType Type
        {
            get { return type; }
            set { type = value; }
        }

        Point position;

        int depth;
        /// <summary>
        /// the depth to be build
        /// </summary>
        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        /// <summary>
        /// color for undefined marker
        /// </summary>
        private Vector3 colorDefault = new Vector3(1, 1, 1);

        /// <summary>
        /// color for info marker
        /// </summary>
        private Vector3 colorInfo = new Vector3(0.8f, 0.8f, 0.8f);

        /// <summary>
        /// color for a tunnel marker
        /// </summary>
        private Vector3 colorTunnel = new Vector3(0.3f, 0.6f, 0.3f);

        /// <summary>
        /// color for a cave marker
        /// </summary>
        private Vector3 colorCave = new Vector3(0.4f, 0.58f, 0.93f);

        /// <summary>
        /// color for an error marker
        /// </summary>
        private Vector3 colorError = new Vector3(0.6f, 0.3f, 0.3f);

        /// <summary>
        /// a fully functional marker
        /// </summary>
        /// <param name="type">markertype</param>
        /// <param name="modelName">modelname</param>
        /// <param name="x">x pos</param>
        /// <param name="y">y pos</param>
        /// <param name="tileSize">size</param>
        public Marker(MarkerType type, Point position, int depth = 1)
        {
            this.type = type;
            this.position = position;
            this.depth = depth;
        }

        /// <summary>
        /// Draw the marker
        /// </summary>
        public void Draw()
        {
            foreach (ModelMesh mesh in FenrirGame.Instance.Properties.ContentManager.getModel(DataIdentifier.modelMarker).Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateRotationX(Microsoft.Xna.Framework.MathHelper.ToRadians(-90)) * Matrix.CreateRotationY(MathHelper.ToRadians(180)) * Matrix.CreateTranslation(new Vector3(this.position.X * FenrirGame.Instance.InGame.Scene.Properties.TileSize, this.position.Y * FenrirGame.Instance.InGame.Scene.Properties.TileSize, 0)) * FenrirGame.Instance.InGame.Camera.World;
                    effect.View = FenrirGame.Instance.InGame.Camera.View;
                    effect.Projection = FenrirGame.Instance.InGame.Camera.Projection;
                    //effect.EnableDefaultLighting();
                    effect.GraphicsDevice.BlendState = BlendState.AlphaBlend;

                    switch (this.type)
                    {
                        case MarkerType.Tunnel:
                            effect.DiffuseColor = this.colorTunnel;
                            break;

                        case MarkerType.Cave:
                            effect.DiffuseColor = this.colorCave;
                            break;

                        case MarkerType.Info:
                            effect.DiffuseColor = this.colorInfo;
                            break;

                        case MarkerType.Error:
                            effect.DiffuseColor = this.colorError;
                            break;
                        default:
                            effect.DiffuseColor = this.colorDefault;
                            break;
                    }
                }

                mesh.Draw();
            }
        }
    }
}
