using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Fenrir.Src.InGame.Entities
{
    /// <summary>
    /// a basic cave
    /// </summary>
    class Cave : Entity
    {
        /// <summary>
        /// list of blocker blocks around to prevent mining in
        /// </summary>
        private List<Point> blockers;

        /// <summary>
        /// list of blocks to mine in order to build a cave
        /// </summary>
        private List<Point> caveBlocksToMine;
        /// <summary>
        /// list of blocks to mine in order to build a cave
        /// </summary>
        public List<Point> CaveBlocksToMine
        {
            get { return caveBlocksToMine; }
            set { caveBlocksToMine = value; }
        }

        /// <summary>
        /// modelname of the cave once done
        /// </summary>
        private String modelName;

        /// <summary>
        /// the inworld position of the cave
        /// </summary>
        private Vector3 cavePosition;

        /// <summary>
        /// Creates a cave
        /// </summary>
        /// <param name="blueprint">the shape of the cave</param>
        /// <param name="offset"></param>
        public Cave(Caves.CaveBlueprint blueprint, Point offset)
        {
            this.blockers = new List<Point>();
            this.caveBlocksToMine = new List<Point>();
            this.modelName = blueprint.CaveModel;

            foreach (Point blocker in blueprint.Blockers)
                this.blockers.Add(new Point(blocker.X + offset.X, blocker.Y + offset.Y));

            foreach (Point cavePart in blueprint.CaveBlocks.Keys)
                this.caveBlocksToMine.Add(new Point(cavePart.X + offset.X, cavePart.Y + offset.Y));

            this.cavePosition = new Vector3(offset.X * FenrirGame.Instance.InGame.Scene.Properties.TileSize, offset.Y * FenrirGame.Instance.InGame.Scene.Properties.TileSize, -1);
        }

        /// <summary>
        /// mark a block to be mined
        /// </summary>
        /// <param name="minedBlock"></param>
        public void CaveBlockMined(Point minedBlock){
            if (this.caveBlocksToMine.Contains(minedBlock))
            {
                this.caveBlocksToMine.Remove(minedBlock);

                List<Point> deadMarker = new List<Point>();
                foreach (Point maker in this.caveBlocksToMine)
                    if (!FenrirGame.Instance.InGame.Scene.IsMarked(maker))
                        deadMarker.Add(maker);

                foreach (Point marker in deadMarker)
                    this.caveBlocksToMine.Remove(marker);

                if (this.caveBlocksToMine.Count == 0)
                    FenrirGame.Instance.Log(LogLevel.Info, "Cave Fully Mined");
            }
        }

        /// <summary>
        /// draw the cave
        /// </summary>
        public void Draw()
        {
            if (this.caveBlocksToMine.Count == 0)
                FenrirGame.Instance.Renderer.Draw(this.modelName, Matrix.CreateRotationX(MathHelper.ToRadians(-90)) * Matrix.CreateRotationY(MathHelper.ToRadians(-90)) * Matrix.CreateTranslation(this.cavePosition));
        }
    }
}
