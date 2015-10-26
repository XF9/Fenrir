using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Fenrir.Src.InGame.Entities
{
    class Building
    {
        private String model;
        
        private BoundingBox boundingbox;
        public BoundingBox Boundingbox
        {
            get { return boundingbox; }
            private set { boundingbox = value; }
        }
        private Vector3 currentPosition;

        private Boolean selected = false;
        private Boolean hover = false;

        public Boolean Hover
        {
            get { return hover; }
            set { hover = value; }
        }

        public Building(String model, Vector3 modelSize, Vector3 position)
        {
            this.model = model;
            this.currentPosition = position;
            this.boundingbox = new BoundingBox(this.currentPosition - modelSize * 2, this.currentPosition + modelSize * 2);
        }

        public void Select()
        {
            this.selected = true;
        }

        public void Deselect()
        {
            this.selected = false;
        }
        
        public void Draw()
        {
            foreach (Microsoft.Xna.Framework.Graphics.ModelMesh mesh in FenrirGame.Instance.Properties.ContentManager.getModel(this.model).Meshes)
            {
                foreach (Microsoft.Xna.Framework.Graphics.BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateTranslation(this.currentPosition) * FenrirGame.Instance.InGame.Camera.World;
                    effect.View = FenrirGame.Instance.InGame.Camera.View;
                    effect.Projection = FenrirGame.Instance.InGame.Camera.Projection;
                    effect.EnableDefaultLighting();

                    if (this.hover)
                    {
                        effect.DiffuseColor = Color.Orange.ToVector3();
                        this.hover = false;
                    }
                    else if (this.selected)
                        effect.DiffuseColor = Color.Green.ToVector3();
                    else
                        effect.DiffuseColor = Color.Red.ToVector3();
                }
                mesh.Draw();
            }
        }
    }
}