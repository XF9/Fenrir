using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Fenrir.Src.InGame.Entities
{
    /// <summary>
    /// A unit
    /// </summary>
    class Unit : Entity
    {
        #region variables
        /// <summary>
        /// the model name
        /// </summary>
        private String model;

        /// <summary>
        /// size of the model
        /// </summary>
        private Vector3 size;

        /// <summary>
        /// boundingbox for the model
        /// </summary>
        private BoundingBox boundingbox;
        /// <summary>
        /// boundingbox for the model
        /// </summary>
        public BoundingBox Boundingbox
        {
            get { return boundingbox; }
            private set { boundingbox = value; }
        }

        /// <summary>
        /// position of the model
        /// </summary>
        private Vector3 position;
        /// <summary>
        /// position of the model
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            private set { position = value; }
        }

        /// <summary>
        /// statistics for the unit
        /// </summary>
        private Units.Stats stats;
        /// <summary>
        /// statistics for the unit
        /// </summary>
        internal Units.Stats Stats
        {
            get { return stats; }
            private set { stats = value; }
        }

        /// <summary>
        /// checked if the unit is currently hovered
        /// </summary>
        private Boolean hover = false;
        /// <summary>
        /// checked if the unit is currently hovered
        /// </summary>
        public Boolean Hover
        {
            get { return hover; }
            set { hover = value; }
        }

        /// <summary>
        /// checked if the unit is currently Selected
        /// </summary>
        private Boolean selected = false;
        /// <summary>
        /// checked if the unit is currently Selected
        /// </summary>
        public Boolean Selected
        {
            get { return selected; }
            set { 
                selected = value;
                if (selected)
                    FenrirGame.Instance.InGame.Hud.InfoBlock = this.stats.Panel;
                else
                    FenrirGame.Instance.InGame.Hud.InfoBlock = null;
            }
        }

        /// <summary>
        /// list of tasks to perform
        /// </summary>
        private LinkedList<Units.ITask> tasklist;

        /// <summary>
        /// color of the unit
        /// </summary>
        private Microsoft.Xna.Framework.Color color;
        /// <summary>
        /// color of the unit
        /// </summary>
        public Microsoft.Xna.Framework.Color Color
        {
            get { return color; }
            set { color = value; }
        }

        #endregion

        /// <summary>
        /// Unit creation
        /// </summary>
        /// <param name="model">the model</param>
        /// <param name="playersize">the size</param>
        /// <param name="position">the position inworld</param>
        /// <param name="name">the name</param>
        public Unit(String model, Vector3 playersize, Vector3 position, String name)
        {
            this.position = position;
            this.model = model;
            this.size = playersize;
            this.boundingbox = new BoundingBox(this.position - this.size * 2, this.position + this.size * 2);
            this.tasklist = new LinkedList<Units.ITask>();
            this.stats = new Units.Stats();
            this.stats.Name = name;
            this.color = Color.White;
        }

        /// <summary>
        /// Updates the unit
        /// </summary>
        public void Update()
        {
            if (this.tasklist.Count == 0)
                this.Idle();
            else
                this.tasklist.First.Value.Execute();

            if (this.Selected)
                this.Stats.Panel.Update();
        }

        /// <summary>
        /// updates the unit if in idle mode - move around etc.
        /// </summary>
        private void Idle()
        {
            Point current = FenrirGame.Instance.InGame.Scene.PixelPositionToTilePosition(this.position);
            Point below = current;
            below.Y--;

            if (FenrirGame.Instance.InGame.Scene.GetBlockDepth(below) >= 2)
            {
                this.ChainTask(new Units.Tasks.TaskMove(below));
            }
            else if (FenrirGame.Instance.Properties.ContentManager.randomize(0, 1000) > 990)
            {
                // else try to make a random move
                int direction = FenrirGame.Instance.Properties.ContentManager.randomize(0, 4);
                switch (direction)
                {
                    case 0:
                        current.X--;
                        break;
                    case 1:
                        current.X++;
                        break;
                    case 2:
                        current.Y--;
                        break;
                    case 3:
                        // only move up if not in a cave
                        if (FenrirGame.Instance.InGame.Scene.GetBlockDepth(current) < 2)
                            current.Y++;
                        break;
                    default:
                        break;
                }

                // check if walkable and if so do it
                Components.TileState state = FenrirGame.Instance.InGame.Scene.GetBlockState(current);
                if (state == Components.TileState.Walkable || state == Components.TileState.Climbable)
                    this.ChainTask(new Units.Tasks.TaskMove(current));
            }
        }

        /// <summary>
        /// Draw the unit
        /// </summary>
        public void Draw()
        {
            foreach (Microsoft.Xna.Framework.Graphics.ModelMesh mesh in FenrirGame.Instance.Properties.ContentManager.getModel(this.model).Meshes)
            {
                foreach (Microsoft.Xna.Framework.Graphics.BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateTranslation(this.position) * Matrix.CreateTranslation(0, -0.5f, 0) * FenrirGame.Instance.InGame.Camera.World;
                    effect.View = FenrirGame.Instance.InGame.Camera.View;
                    effect.Projection = FenrirGame.Instance.InGame.Camera.Projection;
                    effect.EnableDefaultLighting();
                    effect.DiffuseColor = this.color.ToVector3();

                    if (this.selected)
                    {
                        effect.DiffuseColor = (Color.Orange.ToVector3());
                        this.hover = false;
                    }

                    if (this.hover)
                    {
                        effect.DiffuseColor = (Color.LightGreen.ToVector3());
                        this.hover = false;
                    }

                    mesh.Draw();
                }
            }
        }

        /// <summary>
        /// moves the unit around - pixelwise
        /// to really move them use the walk task!
        /// </summary>
        /// <param name="newPosition"></param>
        public void UpdatePosition(Vector3 newPosition)
        {
            this.position += newPosition;
            this.boundingbox = new BoundingBox(this.position - this.size * 2, this.position + this.size * 2);
        }

        /// <summary>
        /// removes the current task from the list
        /// </summary>
        /// <param name="force">removes the task even if not finished</param>
        public void FinishCurrentTask(Boolean force = false)
        {
            if (this.tasklist.Count > 0)
            {

                if (force)
                    this.tasklist.First.Value.Cancel();

                this.tasklist.RemoveFirst();

                // if the next task is just paused resume it
                if (this.tasklist.Count > 0 && this.tasklist.First.Value.IsPaused())
                    this.tasklist.First.Value.Resume();
            }
            
            this.color = Color.White;
        }

        /// <summary>
        /// add a task to the tasklist
        /// </summary>
        /// <param name="task">the task to be added</param>
        public void ChainTask(Units.ITask task)
        {
            task.Prepare(this);
            this.tasklist.AddLast(task);
        }

        /// <summary>
        /// Executes a task at instance and removes all other tasks
        /// </summary>
        /// <param name="task">the task to be executed</param>
        public void ExecuteTask(Units.ITask task)
        {
            task.Prepare(this);

            foreach (Units.ITask ttask in this.tasklist)
                ttask.Cancel();

            this.tasklist.Clear();
            this.ChainTask(task);
        }

        /// <summary>
        /// starts a task and move the others back
        /// </summary>
        /// <param name="task">the task to add</param>
        public void AddProrizedTask(Units.ITask task)
        {
            task.Prepare(this);

            if(this.tasklist.First != null)
                this.tasklist.First.Value.Pause();
            this.tasklist.AddFirst(task);
        }
    }
}
