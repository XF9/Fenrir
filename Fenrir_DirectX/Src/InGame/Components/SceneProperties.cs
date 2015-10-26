using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fenrir.Src.InGame.Components
{
    /// <summary>
    /// This class contains all fixed settings for running the game
    /// </summary>
    class SceneProperties
    {
        #region block properties

        private int tileSize = 2;
        /// <summary>
        /// size of one tile
        /// </summary>
        public int TileSize
        {
            get { return tileSize; }
            private set { tileSize = value; }
        }

        private int maxBlockDepth = 5;
        /// <summary>
        /// maximum tile depth
        /// </summary>
        public int MaxBlockDepth
        {
            get { return maxBlockDepth; }
            private set { maxBlockDepth = value; }
        }

        #endregion

        #region blocks starting area
        private int startingAreaBlocksLeft = -22;
        /// <summary>
        /// the left border of the starting area blocks
        /// </summary>
        public int StartingAreaBlocksLeft
        {
            get { return startingAreaBlocksLeft; }
            private set { startingAreaBlocksLeft = value; }
        }

        private int startingAreaBlocksRight = 8;
        /// <summary>
        /// the right border of the starting area blocks
        /// </summary>
        public int StartingAreaBlocksRight
        {
            get { return startingAreaBlocksRight; }
            private set { startingAreaBlocksRight = value; }
        }

        private int startingAreaBlocksTop = 14;
        /// <summary>
        /// the top border of the starting area blocks
        /// </summary>
        public int StartingAreaBlocksTop
        {
            get { return startingAreaBlocksTop; }
            private set { startingAreaBlocksTop = value; }
        }

        private int startingAreaBlocksBottom = -2;
        /// <summary>
        /// the build border - above nothing can be build without forcing
        /// </summary>
        public int StartingAreaBlocksBottom
        {
            get { return startingAreaBlocksBottom; }
            private set { startingAreaBlocksBottom = value; }
        }

        private int startingAreaLeftBorder = -20;
        /// <summary>
        /// left border of the starting area model
        /// </summary>
        public int StartingAreaLeftBorder
        {
            get { return startingAreaLeftBorder; }
            private set { startingAreaLeftBorder = value; }
        }

        private int startingAreaRightBorder = 6;
        /// <summary>
        /// right border of the starting area model
        /// </summary>
        public int StartingAreaRightBorder
        {
            get { return startingAreaRightBorder; }
            set { startingAreaRightBorder = value; }
        }

        private int startingAreaBottomBorder = 0;
        /// <summary>
        /// bottom border of the starting area model
        /// </summary>
        public int StartingAreaBottomBorder
        {
            get { return startingAreaBottomBorder; }
            set { startingAreaBottomBorder = value; }
        }

        private int buildLevel = -2;
        /// <summary>
        /// the build level border
        /// </summary>
        public int BuildLevel
        {
            get { return buildLevel; }
            private set { buildLevel = value; }
        }

        #endregion
    }
}
