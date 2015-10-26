using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Fenrir.Src.InGame.Entities.Units.Tasks
{
    /// <summary>
    /// Task to move a unit to a specific place
    /// </summary>
    class TaskMove : ITask
    {
        /// <summary>
        /// the point to move to
        /// </summary>
        private Point target;

        /// <summary>
        /// the unit executing this task
        /// </summary>
        private Entities.Unit executingUnit;

        /// <summary>
        /// wether the task is paused or not
        /// </summary>
        public Boolean isPaused = false;

        /// <summary>
        /// the movement speed horizontaly
        /// </summary>
        private float movementSpeedX = 0.1f;

        /// <summary>
        /// the movement speed verticaly
        /// </summary>
        private float movementSpeedY = 0.05f;

        /// <summary>
        /// the path to walk
        /// </summary>
        private LinkedList<Point> path;

        /// <summary>
        /// Create the task
        /// </summary>
        /// <param name="target">the point to move to in world coordinates</param>
        public TaskMove(Point target)
        {
            this.target = target;
        }

        /// <summary>
        /// prepare the task
        /// </summary>
        /// <param name="executingUnit"></param>
        public void Prepare(Entities.Unit executingUnit)
        {
            this.executingUnit = executingUnit;

            // compute the path to the given target
            Point currentTile = FenrirGame.Instance.InGame.Scene.PixelPositionToTilePosition(this.executingUnit.Position);
            this.path = FenrirGame.Instance.InGame.Scene.getPath(currentTile, this.target) ?? new LinkedList<Point>();

            this.executingUnit.Color = Color.CornflowerBlue;
        }

        /// <summary>
        /// execute the task
        /// </summary>
        public void Execute()
        {
            if (this.path.Count == 0 || this.executingUnit == null)
            {
                this.executingUnit.FinishCurrentTask();
                return;
            }

            // distance to the target
            Vector2 distance = new Vector2(this.path.First().X * FenrirGame.Instance.InGame.Scene.Properties.TileSize - this.executingUnit.Position.X, this.path.First().Y * FenrirGame.Instance.InGame.Scene.Properties.TileSize - this.executingUnit.Position.Y);

            if (distance.X == 0 && distance.Y == 0)
            {
                this.path.RemoveFirst();
                if (this.path.Count == 0)
                {
                    this.executingUnit.FinishCurrentTask();
                    return;
                }
            }
            
            if (distance.Y == 0)
            {
                if (distance.X < 0)
                    this.executingUnit.UpdatePosition(new Vector3(-distance.X < this.movementSpeedX ? distance.X : -this.movementSpeedX, 0, 0));
                else
                    this.executingUnit.UpdatePosition(new Vector3(distance.X < this.movementSpeedX ? distance.X : +this.movementSpeedX, 0, 0));
            }
            else
            {
                if (distance.Y < 0)
                    this.executingUnit.UpdatePosition(new Vector3(0, -distance.Y < this.movementSpeedY ? distance.Y : -this.movementSpeedY, 0));
                else
                    this.executingUnit.UpdatePosition(new Vector3(0, distance.Y < this.movementSpeedY ? distance.Y : +this.movementSpeedY, 0));
            }
        }

        /// <summary>
        /// pause the task
        /// </summary>
        public void Pause() {
            this.isPaused = true;
        }

        /// <summary>
        /// resume the task
        /// </summary>
        public void Resume()
        {
            // recompute path
            this.isPaused = false;
            this.Prepare(this.executingUnit);
        }

        /// <summary>
        /// check if the task is paused
        /// </summary>
        /// <returns></returns>
        public Boolean IsPaused()
        {
            return this.isPaused;
        }

        /// <summary>
        /// cancel the task
        /// </summary>
        public void Cancel() { }
    }
}