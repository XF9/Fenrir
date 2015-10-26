using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Fenrir.Src.InGame.Entities;
using Fenrir.Src.InGame.Entities.Caves;
using Fenrir.Src.InGame.Components;

namespace Fenrir.Src.InGame.ControlModeHandler
{
    /// <summary>
    /// Handler to build a cave
    /// </summary>
    class BuildCaveHandler : IModeHandler
    {
        /// <summary>
        /// the scene to build a cave in
        /// </summary>
        Components.Scene scene;

        /// <summary>
        /// the cave to build
        /// </summary>
        CaveBlueprint cave;

        /// <summary>
        /// all the markers to be set
        /// </summary>
        private Dictionary<Point, Marker> markers;

        /// <summary>
        /// create the master of the caves!
        /// </summary>
        /// <param name="scene"></param>
        public BuildCaveHandler(Components.Scene scene)
        {
            this.scene = scene;
            this.markers = new Dictionary<Point, Marker>();
        }

        /// <summary>
        /// how to activate this mode
        /// </summary>
        /// <param name="entity"></param>
        public void Activate(Entity entity)
        {
            if (entity is CaveBlueprint)
                this.cave = (CaveBlueprint)entity;
            else
                this.scene.DisposeCurrentModeHandler();
        }

        /// <summary>
        /// handle the mode
        /// </summary>
        /// <param name="hoverPoint">current mouse over</param>
        public void Handle(Microsoft.Xna.Framework.Point hoverPoint)
        {
            if (FenrirGame.Instance.Properties.Input.RightClick)
            {
                this.scene.DisposeCurrentModeHandler();
                return;
            }

            this.markers.Clear();

            // grid snapping (y axis)
            if (hoverPoint.Y % 2 != 0)
                hoverPoint = new Microsoft.Xna.Framework.Point(hoverPoint.X, hoverPoint.Y - 1);

            Boolean buildable = true;
            Point tmpPoint;

            // check if enough space for a cave
            foreach (Microsoft.Xna.Framework.Point block in this.cave.Blockers)
            {
                tmpPoint.X = block.X + hoverPoint.X;
                tmpPoint.Y = block.Y + hoverPoint.Y;

                if (this.scene.GetBlockDepth(tmpPoint) > 0 || this.scene.Markers.ContainsKey(tmpPoint) || this.scene.IntersectsStartingArea(tmpPoint))
                {
                    buildable = false;
                    break;
                }
            }

            // check if cave parts are destructable
            if (buildable)
                foreach (Microsoft.Xna.Framework.Point block in this.cave.CaveBlocks.Keys)
                {
                    tmpPoint.X = block.X + hoverPoint.X;
                    tmpPoint.Y = block.Y + hoverPoint.Y;

                    // two stage if to prevent creation of new blocks
                    if (this.scene.GetBlockState(tmpPoint) != TileState.None)
                        if (!this.scene.GetBlock(tmpPoint).IsDestructable)
                        {
                            buildable = false;
                            break;
                        }
                }

            if (buildable && FenrirGame.Instance.Properties.Input.LeftClick)
            {
                List<Microsoft.Xna.Framework.Point> blocks = new List<Microsoft.Xna.Framework.Point>();

                // mark blocks
                foreach (Microsoft.Xna.Framework.Point block in this.cave.CaveBlocks.Keys)
                {
                    tmpPoint.X = block.X + hoverPoint.X;
                    tmpPoint.Y = block.Y + hoverPoint.Y;

                    if (this.markers.ContainsKey(tmpPoint))
                        this.markers.Remove(tmpPoint);

                    if (!this.scene.Markers.ContainsKey(tmpPoint) && this.scene.GetBlockDepth(tmpPoint) < this.cave.CaveBlocks[block])
                        this.scene.Markers.Add(tmpPoint, new Marker(MarkerType.Cave, tmpPoint, this.cave.CaveBlocks[block]));
                }

                // block surrounding -> make indestructable
                foreach (Microsoft.Xna.Framework.Point block in this.cave.Blockers)
                {
                    tmpPoint.X = block.X + hoverPoint.X;
                    tmpPoint.Y = block.Y + hoverPoint.Y;

                    this.scene.GetBlock(tmpPoint).IsDestructable = false;
                }

                this.scene.Caves.Add(new Cave(this.cave, hoverPoint));
            }
            else
            {
                // highlights!
                foreach (Microsoft.Xna.Framework.Point block in this.cave.CaveBlocks.Keys)
                {
                    tmpPoint.X = block.X + hoverPoint.X;
                    tmpPoint.Y = block.Y + hoverPoint.Y;

                    if (buildable)
                        this.markers.Add(tmpPoint, new Marker(MarkerType.Cave, tmpPoint, this.cave.CaveBlocks[block]));
                    else
                        this.markers.Add(tmpPoint, new Marker(MarkerType.Error, tmpPoint, this.cave.CaveBlocks[block]));
                }
            }
        }

        /// <summary>
        /// Draw all the helper stuff
        /// </summary>
        public void DrawHelper()
        {
            foreach (Marker maker in this.markers.Values)
                maker.Draw();
        }

        /// <summary>
        /// how to deactivate the mode
        /// </summary>
        public void Deactivate() { }
    }
}
