using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Fenrir.Src.Helper.UI;

namespace Fenrir.Src.Helper
{
    class Renderer
    {
        private Boolean isPrepared = false;

        /// <summary>
        /// starts the renderer
        /// </summary>
        public void Prepare()
        {
            // reset all the spritebatch could have messed up
            FenrirGame.Instance.Properties.GraphicDeviceManager.GraphicsDevice.BlendState = BlendState.Opaque;
            FenrirGame.Instance.Properties.GraphicDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            FenrirGame.Instance.Properties.GraphicDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            // set camera and projection matrix for 3D rendering if ingame
            if (FenrirGame.Instance.Properties.CurrentGameState == GameState.InGame || FenrirGame.Instance.Properties.CurrentGameState == GameState.Paused)
            {
                FenrirGame.Instance.Properties.BaseShader.Parameters["View"].SetValue(FenrirGame.Instance.InGame.Camera.View);
                FenrirGame.Instance.Properties.BaseShader.Parameters["Projection"].SetValue(FenrirGame.Instance.InGame.Camera.Projection);
            }

            // prepare spritebatch
            FenrirGame.Instance.Properties.SpriteBatch.Begin(
                SpriteSortMode.Deferred, 
                BlendState.NonPremultiplied, 
                SamplerState.LinearWrap, 
                null, 
                null);

            this.isPrepared = true;
        }

        /// <summary>
        /// Renders an image
        /// </summary>
        /// <param name="image">the image to render</param>
        public void Draw(Image image)
        {
            if (!this.isPrepared)
            {
                FenrirGame.Instance.Log(LogLevel.Warn, "tried to render " + image.TextureName + " without beeing prepared - aborting");
                return;
            }

            if(image.IsTile)
                FenrirGame.Instance.Properties.SpriteBatch.Draw(
                    FenrirGame.Instance.Properties.ContentManager.GetTexture(image.TextureName),
                    image.Position,
                    image.ImageSpace,
                    image.Color,
                    0,
                    Vector2.Zero,
                    1,
                    SpriteEffects.None,
                    0
                );
            else
                FenrirGame.Instance.Properties.SpriteBatch.Draw(
                    FenrirGame.Instance.Properties.ContentManager.GetTexture(image.TextureName),
                    image.ImageSpace,
                    image.Color
                );
        }

        /// <summary>
        /// Renders a Label
        /// </summary>
        /// <param name="label">the label to be rendered</param>
        public void Draw(Label label)
        {
            if (!this.isPrepared)
            {
                FenrirGame.Instance.Log(LogLevel.Warn, "tried to render " + label.Text + " without beeing prepared - aborting");
                return;
            }

            FenrirGame.Instance.Properties.SpriteBatch.DrawString(
               label.Font,
               FenrirGame.Instance.Properties.ContentManager.getLocalization(label.Text),
               label.RenderPosition,
               label.Color,
               0,
               new Vector2(),
               1,
               Microsoft.Xna.Framework.Graphics.SpriteEffects.None,
               1);
        }

        /// <summary>
        /// Draws an model
        /// </summary>
        /// <param name="modelname">the name of the model</param>
        /// <param name="world">the world position</param>
        public void Draw(String modelname, Matrix world)
        {
            if (!this.isPrepared)
            {
                FenrirGame.Instance.Log(LogLevel.Warn, "tried to render " + modelname + " without beeing prepared - aborting");
                return;
            }

            foreach (Microsoft.Xna.Framework.Graphics.ModelMesh mesh in FenrirGame.Instance.Properties.ContentManager.getModel(modelname).Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = FenrirGame.Instance.Properties.BaseShader;

                    FenrirGame.Instance.Properties.BaseShader.Parameters["World"].SetValue(world * FenrirGame.Instance.InGame.Camera.World);
                }
                mesh.Draw();
            }
        }

        /// <summary>
        /// flushes everything that is left and closes the draw cycle
        /// </summary>
        public void Flush()
        {
            FenrirGame.Instance.Properties.SpriteBatch.End();
            this.isPrepared = false;
        }
    }
}
