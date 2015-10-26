using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Fenrir.Src.Helper;

namespace Fenrir.Src.InGame.Entities
{
    /// <summary>
    /// custom block destruct event
    /// </summary>
    class BlockDestructEvent : EventArgs
    {
        public Boolean silent = false;
    }

    /// <summary>
    /// a basic block.
    /// </summary>
    class Block
    {
        #region variables

        /// <summary>
        /// position to be drawn at
        /// </summary>
        private Vector3 absolutePosition;

        private Point gridPosition;
        /// <summary>
        /// the position of this block in the grid
        /// </summary>
        public Point GridPosition
        {
            get { return gridPosition; }
            private set { gridPosition = value; }
        }

        private Vector3 drawColor;
        /// <summary>
        /// basic color of the object
        /// </summary>
        public Vector3 DrawColor
        {
            get { return drawColor; }
            set { drawColor = value; }
        }

        private Vector3? highLightColor;
        /// <summary>
        /// single frmae highlight color
        /// </summary>
        public Vector3? HighLightColor
        {
            get { return highLightColor; }
            set { highLightColor = value; }
        }

        private Boolean isDestructable;
        /// <summary>
        /// determines if a block can be destructed or not
        /// </summary>
        public Boolean IsDestructable
        {
            get { return isDestructable; }
            set { isDestructable = value; }
        }

        /// <summary>
        /// the amount of hitpoints hte block has
        /// </summary>
        private float hitpoints;

        public Boolean IsMined
        {
            get { return (this.hitpoints < 0); }
            private set { }
        }

        private int depth;
        /// <summary>
        /// depth of a block
        /// </summary>
        public int Depth
        {
            get { return depth; }
            private set { depth = value; }
        }

        private Boolean hidden = false;

        /// <summary>
        /// eventhandler that gets called once the block gets destructed
        /// </summary>
        public event EventHandler<BlockDestructEvent> destruction;

        #endregion

        /// <summary>
        /// Creates a new block
        /// </summary>
        /// <param name="gridPosition">position of the block in the grid</param>
        /// <param name="depth">z position of the block in the grid</param>
        /// <param name="baseColor">base color used for rendering</param>
        /// <param name="destructable">wether the block should be marked as indestructable or not</param>
        public Block(Microsoft.Xna.Framework.Point gridPosition, int depth, Boolean destructable)
        {
            this.depth = depth;
            this.gridPosition = gridPosition;
            int size = FenrirGame.Instance.InGame.Scene.Properties.TileSize;
            this.absolutePosition = new Microsoft.Xna.Framework.Vector3(gridPosition.X * size, gridPosition.Y * size, - (depth * size + size / 2));

            this.drawColor = new Vector3(0.14f, 0.14f, 0.14f);
            this.isDestructable = destructable;
            this.hitpoints = 100;
        }

        /// <summary>
        /// gets called if the block is mined
        /// call to mine a block immediately
        /// </summary>
        /// <param name="silent">wether the block should be hidden or not</param>
        public void Destruct(Boolean silent = false)
        {
            // set hitpoints to minus one
            this.hitpoints = -1;

            // call onDestruction event
            BlockDestructEvent destructevent = new BlockDestructEvent();
            destructevent.silent = silent;
            this.OnDestruction(destructevent);
        }

        /// <summary>
        /// mine this block
        /// </summary>
        /// <param name="power"></param>
        /// <returns>true if the mining operation is valid</returns>
        public Boolean Mine(float power)
        {
            if (this.isDestructable)
            {
                this.hitpoints -= power;

                if (this.hitpoints < 0)
                    this.Destruct();

                return true;
            }
            return false;
        }

        /// <summary>
        /// stops the block from being rendered
        /// </summary>
        public void Hide()
        {
            this.hidden = true;
        }

        /// <summary>
        /// lets draw this block
        /// </summary>
        public void Draw()
        {
            if (!this.hidden)
                FenrirGame.Instance.Renderer.Draw(DataIdentifier.modelBlock, Matrix.CreateTranslation(this.absolutePosition));
        }

        /// <summary>
        /// to be called to raise the destruction event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDestruction(BlockDestructEvent e)
        {
            EventHandler<BlockDestructEvent> handler = this.destruction;
            if (handler != null)
                handler(this, e);
        }
    }
}
