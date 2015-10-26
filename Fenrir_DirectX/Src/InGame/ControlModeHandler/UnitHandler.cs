using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fenrir.Src.InGame.Entities;
using Fenrir.Src.InGame.Entities.Units;
using Fenrir.Src.InGame.Components;

namespace Fenrir.Src.InGame.ControlModeHandler
{
    /// <summary>
    /// Handle Unit things
    /// </summary>
    class UnitHandler : IModeHandler
    {
        /// <summary>
        /// the scene to operate in
        /// </summary>
        private Fenrir.Src.InGame.Components.Scene scene;

        /// <summary>
        /// the selected unit
        /// </summary>
        private Unit selectedUnit;

        /// <summary>
        /// Create the Handler
        /// </summary>
        /// <param name="scene"></param>
        public UnitHandler(Fenrir.Src.InGame.Components.Scene scene)
        {
            this.scene = scene;
        }

        public void Activate(Entity entity)
        {
            if (entity is Unit)
            {
                this.selectedUnit = (Unit)entity;
                this.selectedUnit.Selected = true;
            }
            else
                this.scene.DisposeCurrentModeHandler();
        }

        /// <summary>
        /// update the handler
        /// </summary>
        /// <param name="hoverPoint"></param>
        public void Handle(Microsoft.Xna.Framework.Point hoverPoint)
        {
            // left klick and not hovered -> deselect
            if (FenrirGame.Instance.Properties.Input.LeftClick && FenrirGame.Instance.Properties.Input.MouseRay.Intersects(this.selectedUnit.Boundingbox) == null)
                this.scene.DisposeCurrentModeHandler();
            
            // selected and right click -> move/mine
            if (FenrirGame.Instance.Properties.Input.RightClick)
            {
                // block to mine if needed
                TileState targetBlockState = this.scene.GetBlockState(hoverPoint);

                if (targetBlockState == TileState.Solid)
                    this.selectedUnit.ExecuteTask(new Entities.Units.Tasks.TaskMine(hoverPoint));

                // movin
                else if (targetBlockState == TileState.Walkable || targetBlockState == TileState.Climbable)
                    this.selectedUnit.ExecuteTask(new Entities.Units.Tasks.TaskMove(hoverPoint));
            }
        }

        /// <summary>
        /// Deactivate this handler
        /// </summary>
        public void Deactivate()
        {
            this.selectedUnit.Selected = false;
            this.selectedUnit = null;
        }

        /// <summary>
        /// Draw helper .. not used
        /// </summary>
        public void DrawHelper() { }
    }
}
