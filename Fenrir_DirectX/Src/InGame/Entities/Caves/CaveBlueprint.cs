using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Fenrir.Src.InGame.Entities.Caves
{
    /// <summary>
    /// Shape of a cave
    /// </summary>
    class CaveBlueprint : Entity
    {
        private List<Point> blockers;
        /// <summary>
        /// Blocks to be marked as blocked to prevent mining in the cave
        /// </summary>
        public List<Microsoft.Xna.Framework.Point> Blockers
        {
            get { return blockers; }
            private set { blockers = value; }
        }

        private Dictionary<Point,int> caveBlocks;
        /// <summary>
        /// Blocks that are part of the cave
        /// </summary>
        public Dictionary<Point, int> CaveBlocks
        {
            get { return caveBlocks; }
            private set { caveBlocks = value; }
        }

        private String caveModel;
        /// <summary>
        /// the model to be rendered
        /// </summary>
        public String CaveModel
        {
            get { return caveModel; }
            set { caveModel = value; }
        }
        
        /// <summary>
        /// creates a blueprint
        /// </summary>
        /// <param name="width">width of the cave</param>
        /// <param name="height">height of the cave</param>
        /// <param name="depth">depth of the cave</param>
        /// <param name="caveModel">model to fill in</param>
        public CaveBlueprint(int width, int height, int depth, String caveModel)
        {
            // We count from zero .. so yeah
            height--;

            this.caveBlocks = new Dictionary<Point,int>();
            this.blockers = new List<Point>();
            this.caveModel = caveModel;

            int x = -width / 2;


            for (int i = x; i <= x + width; i++)
                for (int j = 0; j <= height; j++)
                    this.caveBlocks.Add(new Microsoft.Xna.Framework.Point(i, j), depth);

            this.caveBlocks.Add(new Point(x - 1, 0), 1);
            this.caveBlocks.Add(new Point(x + width + 1, 0), 1);

            // surounding block blocks
            for (int i = x - 1; i <= x + width + 1; i++)
                this.blockers.Add(new Microsoft.Xna.Framework.Point(i, height + 1));
            
            for (int j = 1; j <= height; j++)
                this.blockers.Add(new Microsoft.Xna.Framework.Point(x - 1, j));

            for (int j = 1; j <= height; j++)
                this.blockers.Add(new Microsoft.Xna.Framework.Point(x + width + 1, j));
        }
    }
}
