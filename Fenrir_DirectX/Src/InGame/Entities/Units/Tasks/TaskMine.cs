using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Fenrir.Src.InGame.Entities.Units.Tasks
{
    /// <summary>
    /// Task to mine a set of blocks
    /// </summary>
    class TaskMine : ITask
    {
        /// <summary>
        /// the block that is currently mined
        /// </summary>
        private Entities.Block currentBlock;

        /// <summary>
        /// the blocks to be mined
        /// </summary>
        private List<Point> blocks;

        /// <summary>
        /// task paused?
        /// </summary>
        private Boolean isPaused = false;

        /// <summary>
        /// the unit to execute the task
        /// </summary>
        private Unit executingUnit;

        /// <summary>
        /// Creates the task
        /// </summary>
        /// <param name="point">block from the group to be mined</param>
        public TaskMine(Point point)
        {
            this.blocks = this.getMineArea(point);
        }

        /// <summary>
        /// prepares the execution
        /// </summary>
        /// <param name="executingUnit"></param>
        public void Prepare(Unit executingUnit)
        {
            this.executingUnit = executingUnit;
            this.moveToNextBlock();
            this.executingUnit.Color = Color.LimeGreen;
        }

        /// <summary>
        /// Execute the mining
        /// </summary>
        public void Execute()
        {
            if (this.currentBlock != null)
            {
                // check if still not mined and still marked -> mine!
                //if (!this.currentBlock.IsMined && FenrirGame.Instance.WorldComponents.World.IsMarked(this.currentBlock.GridPosition))
                if (!this.currentBlock.IsMined)
                {
                    Point unitPosition = FenrirGame.Instance.InGame.Scene.PixelPositionToTilePosition(this.executingUnit.Position);
                    float divX = Math.Abs(this.currentBlock.GridPosition.X - unitPosition.X);
                    float divY = Math.Abs(this.currentBlock.GridPosition.Y - unitPosition.Y);

                    // near enough to build?
                    if (divX <= 1 && divY <= 1)
                    {
                        this.currentBlock.Mine(this.executingUnit.Stats.Mining / 10 + 1.0f);
                        this.executingUnit.Stats.Mining++;
                    }
                    else
                    {
                        // we run in an error here -> end this task
                        FenrirGame.Instance.Log(LogLevel.Error, "try to mine a block out of reach!");
                        this.executingUnit.FinishCurrentTask();
                    }
                }
                else
                {
                    // check if we have an adjacent block to the current block that is marked
                    Point markedTop = new Point(this.currentBlock.GridPosition.X, this.currentBlock.GridPosition.Y + 1);
                    Point markedBottom = new Point(this.currentBlock.GridPosition.X, this.currentBlock.GridPosition.Y - 1);
                    Point markedLeft = new Point(this.currentBlock.GridPosition.X + 1, this.currentBlock.GridPosition.Y);
                    Point markedRight = new Point(this.currentBlock.GridPosition.X - 1, this.currentBlock.GridPosition.Y);

                    Point? mineTarget = null;

                    if (FenrirGame.Instance.InGame.Scene.IsMarked(this.currentBlock.GridPosition))
                        mineTarget = this.currentBlock.GridPosition;
                    else if (FenrirGame.Instance.InGame.Scene.IsMarked(markedLeft))
                        mineTarget = markedLeft;
                    else if (FenrirGame.Instance.InGame.Scene.IsMarked(markedRight))
                        mineTarget = markedRight;
                    else if (FenrirGame.Instance.InGame.Scene.IsMarked(markedTop))
                        mineTarget = markedTop;
                    else if (FenrirGame.Instance.InGame.Scene.IsMarked(markedBottom))
                        mineTarget = markedBottom;

                    // do we have a marked one?
                    if (mineTarget.HasValue)
                    {
                        this.executingUnit.AddProrizedTask(new TaskMove(this.currentBlock.GridPosition));
                        this.currentBlock = FenrirGame.Instance.InGame.Scene.GetBlock(mineTarget.Value);
                    }
                    else
                    {
                        // check if still something to mine
                        if (this.blocks.Count > 0)
                            this.moveToNextBlock();
                        else
                            this.executingUnit.FinishCurrentTask();
                    }
                }
            }
            else
                this.executingUnit.FinishCurrentTask();
        }

        /// <summary>
        /// cancel the task
        /// </summary>
        public void Cancel() { }

        /// <summary>
        /// pause the task
        /// </summary>
        public void Pause() { this.isPaused = true; }

        /// <summary>
        /// resume the task
        /// </summary>
        public void Resume() { this.isPaused = false; }

        /// <summary>
        /// check if paused
        /// </summary>
        /// <returns></returns>
        public Boolean IsPaused() { return this.isPaused; }

        /// <summary>
        /// get all blocks to be mined from the starting block give
        /// </summary>
        /// <param name="startingPoint">random block from the group to be mined</param>
        /// <returns></returns>
        private List<Point> getMineArea(Point startingPoint)
        {
            // mining
            if (FenrirGame.Instance.InGame.Scene.IsMarked(startingPoint))
            {
                // block belonging to the selected group
                List<Point> group = new List<Point>();
                // blocks beloning to the group but not checked yet
                List<Point> toBeChecked = new List<Point>();
                // add the hover point as startvalue
                toBeChecked.Add(startingPoint);

                Point tmpCheck;

                Point[] pointer = { new Point(), new Point(), new Point(), new Point() };

                while (toBeChecked.Count > 0)
                {
                    tmpCheck = toBeChecked.First();
                    toBeChecked.RemoveAt(0);
                    group.Add(tmpCheck);

                    pointer[0].X = tmpCheck.X;
                    pointer[0].Y = tmpCheck.Y + 1;

                    pointer[1].X = tmpCheck.X;
                    pointer[1].Y = tmpCheck.Y - 1;

                    pointer[2].X = tmpCheck.X - 1;
                    pointer[2].Y = tmpCheck.Y;

                    pointer[3].X = tmpCheck.X + 1;
                    pointer[3].Y = tmpCheck.Y;

                    foreach (Point neighbour in pointer)
                        if (FenrirGame.Instance.InGame.Scene.IsMarked(neighbour) && !group.Contains(neighbour) && !toBeChecked.Contains(neighbour))
                            toBeChecked.Add(neighbour);
                }

                if (group.Count > 0)
                    return group;
            }

            // nothing to do here
            return new List<Point>();
        }

        /// <summary>
        /// move the unit to the next block
        /// </summary>
        private void moveToNextBlock()
        {
            // filter blocks that have been mined
            List<Point> reducedGroup = new List<Point>();

            foreach (Point entry in this.blocks)
                if (FenrirGame.Instance.InGame.Scene.IsMarked(entry))
                    reducedGroup.Add(entry);

            this.blocks = reducedGroup;

            // find posibble starting blocks
            // entry point -> mining block
            Dictionary<Point, Point> entryPoints = new Dictionary<Point, Point>();
            Point[] pointer = { new Point(), new Point(), new Point(), new Point() };

            foreach (Point pos in reducedGroup)
            {
                pointer[0].Y = pos.Y;
                pointer[0].X = pos.X - 1;

                pointer[1].Y = pos.Y;
                pointer[1].X = pos.X + 1;

                pointer[2].Y = pos.Y - 1;
                pointer[2].X = pos.X;

                pointer[3].Y = pos.Y + 1;
                pointer[3].X = pos.X;

                foreach (Point possibleStartingPoint in pointer)
                {
                    Components.TileState state = FenrirGame.Instance.InGame.Scene.GetBlockState(possibleStartingPoint);
                    if ((state == Components.TileState.Walkable || state == Components.TileState.Climbable) && !entryPoints.ContainsKey(possibleStartingPoint))
                    {
                        entryPoints.Add(possibleStartingPoint, pos);
                    }
                }
            }

            if (entryPoints.Count > 0)
            {
                LinkedList<Point> route = FenrirGame.Instance.InGame.Scene.getPath(FenrirGame.Instance.InGame.Scene.PixelPositionToTilePosition(this.executingUnit.Position), new List<Point>(entryPoints.Keys));

                if(route != null && route.Count > 0)
                    foreach(Point key in entryPoints.Keys)
                        if (key.X == route.Last.Value.X && key.Y == route.Last.Value.Y)
                        {
                            this.currentBlock = FenrirGame.Instance.InGame.Scene.GetBlock(entryPoints[key]);
                            this.executingUnit.AddProrizedTask(new TaskMove(key));
                            return;
                        }
            }

            // no block to reach - cancel quest
            this.currentBlock = null;
        }
    }
}
